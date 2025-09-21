import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { HeaderWEB } from '../Component/header-web/header-web';

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
  constructor(private router: Router, private auth: Auth) { }
  async dangNhap(event: Event) {
    event.preventDefault();

    let thongTin_DangNhap: thongTinDangNhap = {
      email: this.email_Dang_Nhap,
      mat_khau: this.mat_khau
    };

    try {
      const response = await fetch("http://localhost:65001/api/API_WEB/xacThucNguoiDung", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(thongTin_DangNhap)
      });

      if (response.ok) {
        const data = await response.json();
        if (data.success) {
          thongTin_DangNhap.ma_dang_nhap = data.thong_tin[0].ma_nguoi_dung;
          thongTin_DangNhap.ten_dang_nhap = data.thong_tin[0].ten_dang_nhap;
          if (data.thong_tin[0].loai_nguoi_dung === 1) {
            thongTin_DangNhap.kieu_nguoi_dung = "nguoi_Tim_Viec";
            thongTin_DangNhap.thong_tin_chi_tiet = {
              ...data.thong_tin[0],
              ...data.thong_tin[0].nguoi_tim_viec
            };
          }
          else if (data.thong_tin[0].loai_nguoi_dung === 2) {
            thongTin_DangNhap.kieu_nguoi_dung = "cong_Ty";
            thongTin_DangNhap.thong_tin_chi_tiet = {
              ...data.thong_tin[0],
              ...data.thong_tin[0].cong_ty
            };
          }
          this.auth.dangNhap(thongTin_DangNhap);
          setTimeout(() => {
            this.router.navigate(['/trang-chu']);
          }, 1500);
          return;
        }
      }

      const response_qtri = await fetch("http://localhost:65001/api/API_WEB/xacThucQuanTriVien", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(thongTin_DangNhap)
      });

      if (response_qtri.ok) {
        const data_qtri = await response_qtri.json();
        if (data_qtri.success) {
          thongTin_DangNhap.ma_dang_nhap = data_qtri.ma_quan_tri;
          if (data_qtri.vai_tro === 1) {
            thongTin_DangNhap.kieu_nguoi_dung = "quan_Tri_Vien";
          }
          thongTin_DangNhap.ten_dang_nhap = data_qtri.ten_dang_nhap;
          this.auth.dangNhap(thongTin_DangNhap);
          setTimeout(() => {
            this.router.navigate(['/trang-chu']);
          }, 1500);
          return;
        }
      }

      this.thong_bao = "Email hoặc mật khẩu không đúng";

    }
    catch (error) {
      console.error("Lỗi đăng nhập:", error);
      this.thong_bao = "Không thể kết nối đến máy chủ";
    }
  }
}
