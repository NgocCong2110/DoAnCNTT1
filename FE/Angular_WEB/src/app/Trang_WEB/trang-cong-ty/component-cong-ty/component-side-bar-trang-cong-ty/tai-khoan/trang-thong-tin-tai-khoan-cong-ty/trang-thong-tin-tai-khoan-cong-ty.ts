import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../../services/auth';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan-cong-ty',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-tin-tai-khoan-cong-ty.html',
  styleUrls: ['./trang-thong-tin-tai-khoan-cong-ty.css']
})
export class TrangThongTinTaiKhoanCongTy implements OnInit {
  thongTin: any;
  formDangMo = false;
  duLieuSua = '';
  giaTriMoi: any = '';
  giaTriCu: any = '';

  fileLogo: File | null = null;
  previewLogo: string | null = null;

  constructor(private auth: Auth, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thongTin = duLieu?.thong_tin_chi_tiet || {};
  }

  moForm(field: string) {
    this.duLieuSua = field;
    this.giaTriMoi = this.thongTin?.cong_ty[field] || '';
    this.giaTriCu = this.giaTriMoi;
    this.formDangMo = true;
  }

  dongForm() {
    this.formDangMo = false;
    this.duLieuSua = '';
    this.giaTriMoi = '';
    this.giaTriCu = '';
    this.fileLogo = null;
    this.previewLogo = null;
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

  async luuForm() {
    if (!this.thongTin?.cong_ty) return;

    if (this.duLieuSua === 'logo' && this.fileLogo) {
      await this.capNhatLogo();
      return;
    }

    this.dongForm();
  }

  capNhatThongTin(thongTin: any) {
    console.log(thongTin.cong_ty.logo);
  }


  private async capNhatLogo() {
    const formData = new FormData();
    formData.append('ma_cong_ty', this.thongTin.cong_ty.ma_cong_ty);
    formData.append('logo', this.fileLogo!);

    try {
      const res = await fetch('http://localhost:65001/api/API_WEB/capNhatLogoCongTy', {
        method: 'POST',
        body: formData
      });
      const data = await res.json();

      if (data.success) {
        this.thongTin.cong_ty.logo = data.url ? `http://localhost:65001/${data.url}` : this.previewLogo;
        const duLieu = this.auth.layThongTinNguoiDung();;
        duLieu.thong_tin_chi_tiet.cong_ty.logo = data.url;
        this.auth.dangNhap(duLieu);

        this.cdr.detectChanges();
        alert(' Cập nhật logo thành công!');
      } else {
        alert(' Cập nhật logo thất bại.');
      }
    } catch (err) {
      console.error('Lỗi upload logo:', err);
      alert(' Không thể tải ảnh lên server.');
    } finally {
      this.dongForm();
    }
  }

  taoDuongDanLogo(duongDan: string): string {
    if (!duongDan) return '';
    if (duongDan.startsWith('http')) return duongDan;
    return `http://localhost:65001/${duongDan}`;
  }
}
