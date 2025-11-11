import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule],
  templateUrl: './trang-thong-tin-tai-khoan.html',
  styleUrls: ['./trang-thong-tin-tai-khoan.css']
})
export class TrangThongTinTaiKhoan implements OnInit {

  thong_tin: any = {};
  editingSection: string | null = null;
  editingValues: any = {};
  previewAvatar: string | null = null;
  fileAvatar: File | null = null;

  danhSachTruong = [
    { label: 'Họ tên', key: 'ho_ten', type: 'text' },
    { label: 'Email', key: 'email', type: 'email' },
    { label: 'Số điện thoại', key: 'dien_thoai', type: 'text' },
    { label: 'Ngày sinh', key: 'ngay_sinh', type: 'date' },
    { label: 'Địa chỉ', key: 'dia_chi', type: 'text' }
  ];

  constructor(
    private auth: Auth,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thong_tin = duLieu || {};
    console.log(this.thong_tin)
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
      ma_quan_tri: this.thong_tin.ma_quan_tri,
      truong: this.editingSection,
      gia_tri: this.editingValues[this.editingSection]
    };

    this.http.patch<any>('http://localhost:7000/api/API_WEB/capNhatThongTinQuanTri', payload)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.thong_tin[this.editingSection!] = this.editingValues[this.editingSection!];

            const userData = this.auth.layThongTinNguoiDung();
            if (userData?.thong_tin_chi_tiet?.nguoi_tim_viec) {
              userData.thong_tin_chi_tiet.nguoi_tim_viec[this.editingSection!] = this.editingValues[this.editingSection!];
              this.auth.dangNhap(userData);
            }

            this.cdr.detectChanges();
            alert('✅ Cập nhật thành công!');
          } else {
            alert('❌ Cập nhật thất bại!');
          }

          this.cancelEdit();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi:', err);
          alert('⚠️ Không thể kết nối server.');
          this.cancelEdit();
        }
      });
  }

  chonAnhDaiDien(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    if (file.size > 5 * 1024 * 1024) {
      alert('⚠️ Kích thước ảnh tối đa 5MB');
      return;
    }

    this.fileAvatar = file;
    const reader = new FileReader();
    reader.onload = () => this.previewAvatar = reader.result as string;
    reader.readAsDataURL(file);
  }

  private capNhatAnhDaiDien() {
    const formData = new FormData();
    formData.append('ma_quan_tri', this.thong_tin.ma_quan_tri);
    formData.append('anh_dai_dien', this.fileAvatar!);

    this.http.post<any>('http://localhost:7000/api/API_WEB/capNhatAnhDaiDienQuanTriVien', formData)
      .subscribe({
        next: (data) => {
          if (data.success) {
            const duLieu = this.auth.layThongTinNguoiDung();
            duLieu.duong_dan_anh_dai_dien = data.url;
            this.auth.dangNhap(duLieu);

            this.thong_tin.duong_dan_anh_dai_dien = data.url;
            this.previewAvatar = this.taoDuongDanAnh(data.url);

            this.cdr.detectChanges();
            alert('✅ Cập nhật ảnh đại diện thành công!');
          } else {
            alert('❌ Cập nhật ảnh thất bại.');
          }

          this.cancelEdit();
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi upload:', err);
          alert('⚠️ Không thể tải ảnh lên.');
          this.cancelEdit();
        }
      });
  }

  taoDuongDanAnh(duongDan: string): string {
    if (!duongDan) return '';
    return duongDan.startsWith('http') ? duongDan : `http://localhost:7000/${duongDan}`;
  }
}
