import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan-ntv',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-tin-tai-khoan-ntv.html',
  styleUrls: ['./trang-thong-tin-tai-khoan-ntv.css']
})
export class TrangThongTinTaiKhoanNtv implements OnInit {
  thong_tin: any = {};
  editingSection: string | null = null;
  editingValues: Record<string, any> = {};

  fileAvatar: File | null = null;
  previewAvatar: string | null = null;

  constructor(
    private auth: Auth,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thong_tin = duLieu?.thong_tin_chi_tiet || {};
    console.log(this.thong_tin);
  }

  enableEdit(section: string) {
    this.editingSection = section;
    this.editingValues[section] = this.thong_tin[section] || '';
  }

  cancelEdit() {
    this.editingSection = null;
    this.editingValues = {};
    this.fileAvatar = null;
    this.previewAvatar = null;
  }

  saveEdit() {
    if (!this.editingSection) return;

    if (this.editingSection === 'anh_dai_dien' && this.fileAvatar) {
      this.capNhatAnhDaiDien();
      return;
    }

    const payload = {
      ma_nguoi_tim_viec: this.thong_tin.ma_nguoi_tim_viec,
      truong: this.editingSection,
      gia_tri: this.editingValues[this.editingSection]
    };

    this.http.patch<any>('http://localhost:65001/api/API_WEB/capNhatThongTinNguoiTimViec', payload)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.thong_tin[this.editingSection!] = this.editingValues[this.editingSection!];

            const userData = this.auth.layThongTinNguoiDung();
            if (userData?.thong_tin_chi_tiet) {
              userData.thong_tin_chi_tiet[this.editingSection!] = this.editingValues[this.editingSection!];
              this.auth.dangNhap(userData);
            }
            this.cdr.detectChanges();

            alert('Cập nhật thành công!');
          } else {
            alert('Cập nhật thất bại!');
          }
          this.cancelEdit();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi:', err);
          alert('Không thể kết nối server.');
          this.cancelEdit();
        }
      });
  }

  chonAnhDaiDien(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    if(file.size > 1024 * 1024 * 5){
      alert("Kích cỡ ảnh quá lớn");
      return;
    }

    this.fileAvatar = file;
    const reader = new FileReader();
    reader.onload = () => this.previewAvatar = reader.result as string;
    reader.readAsDataURL(file);
  }

  private capNhatAnhDaiDien() {
    const formData = new FormData();
    formData.append('ma_nguoi_tim_viec', this.thong_tin.nguoi_tim_viec.ma_nguoi_tim_viec);
    formData.append('anh_dai_dien', this.fileAvatar!);
    console.log(this.thong_tin.nguoi_tim_viec.ma_nguoi_tim_viec)
    this.http.post<any>('http://localhost:65001/api/API_WEB/capNhatAnhDaiDienNguoiTimViec', formData)
      .subscribe({
        next: (data) => {
          if (data.success) {
            const duLieu = this.auth.layThongTinNguoiDung();
            duLieu.thong_tin_chi_tiet.nguoi_tim_viec.anh_dai_dien = data.url;
            this.auth.dangNhap(duLieu);
            this.previewAvatar = this.taoDuongDanAnh(data.url);
            this.thong_tin.nguoi_tim_viec.anh_dai_dien = data.url;
            this.cdr.detectChanges();
            alert('Cập nhật ảnh đại diện thành công!');
          } else {
            alert('Cập nhật ảnh thất bại.');
          }
          this.cancelEdit();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi upload:', err);
          alert('Không thể tải ảnh lên.');
          this.cancelEdit();
        }
      });
  }

  taoDuongDanAnh(duongDan: string): string {
    if (!duongDan) return '';
    return duongDan.startsWith('http') ? duongDan : `http://localhost:65001/${duongDan}`;
  }
}