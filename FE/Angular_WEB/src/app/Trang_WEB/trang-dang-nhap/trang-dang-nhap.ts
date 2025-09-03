import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';

interface thongTinDangNhap {
  email: string;
  mat_khau: string;
  vai_Tro?: string;
  ten_dang_nhap?: string;
  kieu_nguoi_dung?: string;
}

@Component({
  selector: 'app-trang-dang-nhap',
  imports: [RouterLink, FormsModule],
  templateUrl: './trang-dang-nhap.html',
  styleUrls: ['./trang-dang-nhap.css']
})

export class TrangDangNhap{
  email_DN: string = '';
  mat_khau: string = '';
  thong_bao: string = '';
  constructor(private router: Router, private auth: Auth) {}
  async dangNhap(event: Event) {
  event.preventDefault();

  let thongTin_DangNhap: thongTinDangNhap = {
    email: this.email_DN,
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
        thongTin_DangNhap.ten_dang_nhap = data.ten_dang_nhap;
        if(data.vai_Tro === 1){
          thongTin_DangNhap.kieu_nguoi_dung = "nguoi_Tim_Viec";
        }
        else if(data.vai_Tro === 2){
          thongTin_DangNhap.kieu_nguoi_dung = "cong_Ty";
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
        if(data_qtri.vai_Tro === 1){
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
