import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan-cong-ty',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-tin-tai-khoan-cong-ty.html',
  styleUrls: ['./trang-thong-tin-tai-khoan-cong-ty.css']
})
export class TrangThongTinTaiKhoanCongTy implements OnInit {
  thongTin: any = { cong_ty: {} };
  editingSection: string | null = null;
  editingValues: Record<string, any> = {}; 

  fileLogo: File | null = null;
  previewLogo: string | null = null;

  isCompanyOwner = true; 

  constructor(
    private auth: Auth,
    private cdr: ChangeDetectorRef,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thongTin = duLieu?.thong_tin_chi_tiet || { cong_ty: {} };
    this.thongTin.cong_ty = this.thongTin.cong_ty || {};
  }

  enableEdit(section: string) {
    this.editingSection = section;
    this.editingValues[section] = this.thongTin.cong_ty[section] || '';
  }

  cancelEdit() {
    this.editingSection = null;
    this.editingValues = {};
    this.fileLogo = null;
    this.previewLogo = null;
  }

  saveEdit() {
    if (!this.editingSection) return;

    if (this.editingSection === 'logo' && this.fileLogo) {
      this.capNhatLogo();
      return;
    }

    const key = this.editingSection;
    const payload = {
      ma_cong_ty: this.thongTin.cong_ty.ma_cong_ty,
      truong: key,
      gia_tri: this.editingValues[key]
    };

    this.http.patch<any>('http://localhost:7000/api/API_WEB/capNhatThongTinCongTy', payload)
      .subscribe({
        next: data => {
          if (data.success) {
            this.thongTin.cong_ty[key] = this.editingValues[key];

            const duLieu = this.auth.layThongTinNguoiDung();
            duLieu.thong_tin_chi_tiet.cong_ty[key] = this.editingValues[key];
            this.auth.dangNhap(duLieu);

            alert('Cập nhật thành công!');
          } else {
            alert('Cập nhật thất bại!');
          }
          this.cancelEdit();
        },
        error: err => {
          console.error('Lỗi kết nối server:', err);
          alert('Không thể kết nối server.');
          this.cancelEdit();
        }
      });
  }

  chonLogo(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    this.fileLogo = file;
    const reader = new FileReader();
    reader.onload = () => (this.previewLogo = reader.result as string);
    reader.readAsDataURL(file);
  }

  private capNhatLogo() {
    const formData = new FormData();
    formData.append('ma_cong_ty', this.thongTin.cong_ty.ma_cong_ty);
    formData.append('logo', this.fileLogo!);

    this.http.post<any>('http://localhost:7000/api/API_WEB/capNhatLogoCongTy', formData)
      .subscribe({
        next: data => {
          if (data.success) {
            this.thongTin.cong_ty.logo = data.url ? `http://localhost:7000/${data.url}` : this.previewLogo;

            const duLieu = this.auth.layThongTinNguoiDung();
            duLieu.thong_tin_chi_tiet.cong_ty.logo = data.url;
            this.auth.dangNhap(duLieu);

            this.cdr.detectChanges();
            alert('Cập nhật logo thành công!');
          } else {
            alert('Cập nhật logo thất bại.');
          }
          this.cancelEdit();
        },
        error: err => {
          console.error('Lỗi upload logo:', err);
          alert('Không thể tải ảnh lên server.');
          this.cancelEdit();
        }
      });
  }

  taoDuongDanLogo(duongDan: string): string {
    if (!duongDan) return '';
    if (duongDan.startsWith('http')) return duongDan;
    return `http://localhost:7000/${duongDan}`;
  }

  layTenLoaiHinh(value: number): string {
    switch (value) {
      case 1: return 'Công Ty TNHH';
      case 2: return 'Công Ty Cổ Phần';
      case 3: return 'Doanh Nghiệp Tư Nhân';
      case 4: return 'Công Ty Hợp Danh';
      case 5: return 'Khác';
      default: return '';
    }
  }
}
