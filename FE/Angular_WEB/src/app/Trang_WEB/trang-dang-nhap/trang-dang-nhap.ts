import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';

interface thongTinDangNhap {
  email: string;
  password: string;
  vai_Tro?: string;
  ten_dang_nhap?: string;
}

@Component({
  selector: 'app-trang-dang-nhap',
  imports: [RouterLink, FormsModule],
  templateUrl: './trang-dang-nhap.html',
  styleUrls: ['./trang-dang-nhap.css']
})

export class TrangDangNhap{
  email_DN: string = '';
  password_DN: string = '';
  thong_bao: string = '';
  constructor(private router: Router, private auth: Auth) {}
  async dangNhap(event: Event) {
    event.preventDefault();
    let thongTin_DangNhap: thongTinDangNhap ={
      email: this.email_DN,
      password: this.password_DN
    }
    const response = await fetch("http://localhost:65001/api/API_WEB/xacThucNguoiDung", {
      method : "POST",
      headers: {
        "Content-Type" : "application/json"
      },
      body: JSON.stringify(thongTin_DangNhap)
    })
    const data = await response.json();
    if(data.success) {
      thongTin_DangNhap.vai_Tro = data.vai_Tro;
      thongTin_DangNhap.ten_dang_nhap = data.ten_dang_nhap;
      this.auth.dangNhap(thongTin_DangNhap);
      this.router.navigate(['/trang-chu']);
    }
    else{
      const response_qtri = await fetch("http://localhost:65001/api/API_WEB/xacThucQuanTriVien", {
        method : "POST",
        headers: {
          "Content-Type" : "application/json"
        },
        body: JSON.stringify(thongTin_DangNhap)
      })
      const data_qtri = await response_qtri.json();
      if(data_qtri.success) {
        thongTin_DangNhap.vai_Tro = data_qtri.vai_Tro;
        this.auth.dangNhap(thongTin_DangNhap);
        this.router.navigate(['/trang-chu']);
      }
    }
  }
}
