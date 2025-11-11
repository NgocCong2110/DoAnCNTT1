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
    nam_ThanhLap: '',
    dia_Chi: '',
    dien_Thoai: '',
    email: '',
    loaiHinh: 1
  };

  thong_bao: string = '';
  hien_thong_bao = false;

  getThongTinDangKy() {
    return {
      ten_cong_ty: this.form.ten_CongTy,
      ten_dn_cong_ty: this.form.ten_Dang_Ky,
      mat_khau_dn_cong_ty: this.form.mat_Khau,
      nguoi_dai_dien: this.form.nguoi_DaiDien,
      ma_so_thue: this.form.maSo_Thue,
      dia_chi: this.form.dia_Chi,
      email: this.form.email,
      loai_hinh_cong_ty: this.form.loaiHinh,  
      nam_thanh_lap: this.form.nam_ThanhLap,
      trang_thai: 1
    };
  }

  dangKy(formRef: any) {
    if (formRef.valid && this.form.mat_Khau === this.form.xac_Nhan_MatKhau) {
      this.kiemTraEmail();
    }
  }

  kiemTraEmail() {
    this.http.post<any>('http://localhost:7000/api/API_WEB/kiemTraTaiKhoanDangKy', {
      email: this.form.email
    }).subscribe({
      next: (data) => {
        if (data.success) {
          this.http.post<any>('http://localhost:7000/api/API_WEB/xacThucMaSoThue', {
            ma_so_thue: this.form.maSo_Thue
          }).subscribe({
            next: (data2) => {
              if (data2.success) {
                this.http.post<any>(
                  'http://localhost:7000/api/API_WEB/themThongTinCongTy',
                  this.getThongTinDangKy()
                ).subscribe({
                  next: (data3) => {
                    if (data3.success) {
                      this.thong_bao = 'Đăng ký thông tin công ty thành công!';
                      this.hien_thong_bao = true;
                      setTimeout(() => {
                        this.hien_thong_bao = false;
                        this.router.navigate(['/dang-nhap']);
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
                this.thong_bao = data2.message || 'Mã số thuế không hợp lệ!';
                this.hien_thong_bao = true;
                setTimeout(() => {
                  this.hien_thong_bao = false;
                }, 2000);
              }
            },
            error: () => {
              this.thong_bao = 'Lỗi khi xác thực mã số thuế!';
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
