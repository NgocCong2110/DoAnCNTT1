import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { HeaderWEB } from '../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';

interface thongTinDangNhap {
  email: string;
  mat_khau: string;
  ma_dang_nhap?: string;
  vai_Tro?: string;
  ten_dang_nhap?: string;
  kieu_nguoi_dung?: string;
  ho_ten?: string;
  thong_tin_chi_tiet?: any;
}

@Component({
  selector: 'app-trang-dang-nhap',
  imports: [RouterLink, FormsModule, HeaderWEB],
  templateUrl: './trang-dang-nhap.html',
  styleUrls: ['./trang-dang-nhap.css']
})
export class TrangDangNhap {
  email_Dang_Nhap: string = '';
  mat_khau: string = '';
  thong_bao: string = '';

  constructor(public router: Router, public auth: Auth,public http: HttpClient) { }

  dangNhap(event: Event) {
    event.preventDefault();

    let thongTin_DangNhap: thongTinDangNhap = {
      email: this.email_Dang_Nhap,
      mat_khau: this.mat_khau
    };

    this.http.post<any>('http://localhost:65001/api/API_WEB/xacThucNguoiDung', thongTin_DangNhap)
      .subscribe({
        next: (data) => {
          if (data.success) {
            thongTin_DangNhap.ma_dang_nhap = data.thong_tin[0].ma_nguoi_dung;
            thongTin_DangNhap.ten_dang_nhap = data.thong_tin[0].ten_dang_nhap;

            if (data.thong_tin[0].loai_nguoi_dung === 1) {
              thongTin_DangNhap.kieu_nguoi_dung = "nguoi_Tim_Viec";
              thongTin_DangNhap.thong_tin_chi_tiet = {
                ...data.thong_tin[0],
                ...data.thong_tin[0].nguoi_tim_viec
              };
            } else if (data.thong_tin[0].loai_nguoi_dung === 2) {
              thongTin_DangNhap.kieu_nguoi_dung = "cong_Ty";
              thongTin_DangNhap.thong_tin_chi_tiet = {
                ...data.thong_tin[0],
                ...data.thong_tin[0].cong_ty
              };
            }

            this.auth.dangNhap(thongTin_DangNhap);
            setTimeout(() => this.router.navigate(['/trang-chu']), 1500);
            return;
          }

          this.http.post<any>('http://localhost:65001/api/API_WEB/xacThucQuanTriVien', thongTin_DangNhap)
            .subscribe({
              next: (data_qtri) => {
                if (data_qtri.success) {
                  thongTin_DangNhap.ma_dang_nhap = data_qtri.ma_quan_tri;
                  thongTin_DangNhap.ten_dang_nhap = data_qtri.ten_dang_nhap;

                  if (data_qtri.vai_tro === 1) {
                    thongTin_DangNhap.kieu_nguoi_dung = "quan_Tri_Vien";
                  }

                  this.auth.dangNhap(thongTin_DangNhap);
                  setTimeout(() => this.router.navigate(['/trang-chu']), 1500);
                  return;
                }
                this.thong_bao = "Email hoặc mật khẩu không đúng";
              },
              error: () => {
                this.thong_bao = "Không thể kết nối đến máy chủ";
              }
            });
        },
        error: () => {
          this.thong_bao = "Không thể kết nối đến máy chủ";
        }
      });
  }
}
