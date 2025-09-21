import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderWEB } from '../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-trang-dang-ky-cong-ty',
  imports: [RouterLink, CommonModule, FormsModule, HeaderWEB],
  templateUrl: './trang-dang-ky-cong-ty.html',
  styleUrl: './trang-dang-ky-cong-ty.css'
})
export class TrangDangKyCongTy {

  constructor(
    private router: Router,
    private http: HttpClient
  ) { }

  form = {
    ten_CongTy: '',
    ten_Dang_Ky: '',
    mat_Khau: '',
    xac_Nhan_MatKhau: '',
    nguoi_DaiDien: '',
    maSo_Thue: '',
    ngay_ThanhLap: '',
    dia_Chi: '',
    dien_Thoai: '',
    email: '',
    loaiHinh: 'CongTyTNHH'
  };

  thong_bao: string = '';
  hien_thong_bao = false;

  getThongTinDangKy() {
    return {
      ten_Cong_Ty: this.form.ten_CongTy,
      ten_Dn: this.form.ten_Dang_Ky,
      mat_Khau: this.form.mat_Khau,
      vai_Tro: 'cong_Ty'
    };
  }

  dangKy(formRef: any) {
    if (formRef.valid && this.form.mat_Khau === this.form.xac_Nhan_MatKhau) {
      console.log('Dữ liệu form:', this.getThongTinDangKy());
    }
  }

  kiemTraEmail() {
    this.http.post<any>('http://localhost:65001/api/API_WEB/xacThucGmail', {
      email: this.form.email
    }).subscribe({
      next: (data) => {
        if (data.success) {
          this.http.post<any>('http://localhost:65001/api/API_WEB/themThongTinCongTy',
            this.getThongTinDangKy()
          ).subscribe({
            next: (data2) => {
              if (data2.success) {
                this.thong_bao = 'Đăng ký thông tin công ty thành công!';
                this.hien_thong_bao = true;
                setTimeout(() => {
                  this.hien_thong_bao = false;
                  this.router.navigate(['/trang-chu']);
                }, 2000);
              } else {
                this.thong_bao = 'Đăng ký thông tin công ty thất bại!';
                this.hien_thong_bao = true;
                setTimeout(() => this.hien_thong_bao = false, 1500);
              }
            },
            error: () => {
              this.thong_bao = 'Lỗi khi thêm thông tin công ty!';
              this.hien_thong_bao = true;
              setTimeout(() => this.hien_thong_bao = false, 1500);
            }
          });
        } else {
          this.thong_bao = 'Email không hợp lệ hoặc đã tồn tại!';
          this.hien_thong_bao = true;
          setTimeout(() => this.hien_thong_bao = false, 1500);
        }
      },
      error: () => {
        this.thong_bao = 'Lỗi khi kiểm tra email!';
        this.hien_thong_bao = true;
        setTimeout(() => this.hien_thong_bao = false, 1500);
      }
    });
  }
}
