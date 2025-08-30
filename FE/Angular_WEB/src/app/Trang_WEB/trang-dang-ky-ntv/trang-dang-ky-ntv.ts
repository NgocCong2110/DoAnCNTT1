import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { response } from 'express';

@Component({
  selector: 'app-trang-dang-ky-ntv',
  imports: [RouterLink, FormsModule],
  templateUrl: './trang-dang-ky-ntv.html',
  styleUrls: ['./trang-dang-ky-ntv.css']
})
export class TrangDangKyNTV {
  username_DK = '';
  email_DK = '';
  password_DK = '';
  confirm_password_DK = '';
  thong_bao = '';

  constructor(private router: Router) { }

  getThongTinDangKy(){
    return {
      ten_dang_nhap: this.username_DK,
      email: this.email_DK,
      mat_khau: this.password_DK,
      vai_tro: 'nguoi_tim_viec'
    };
  }

  kiemTraMatKhau(event: Event) {
    event.preventDefault();
    if (this.password_DK !== this.confirm_password_DK) {
      this.thong_bao = 'Mật khẩu không khớp!';
    } else {
      this.thong_bao = '';
      this.kiemTraEmail();
    }
  }
  async kiemTraEmail() {
    const response = await fetch("http://localhost:65001/api/API_WEB/xacThucGmail", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        email: this.email_DK,
      })
    });
    const data = await response.json();
    if (data.success) {
      const themThongTin_NTV = await fetch("http://localhost:65001/api/API_WEB/themThongTinNguoiTimViec", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(this.getThongTinDangKy())
      });
      const data2 = await themThongTin_NTV.json();
      if (data2.success) {
        this.thong_bao = 'Thêm thông tin người tìm việc thành công!';
        setTimeout(() => {
          this.router.navigate(['/trang-chu']);
        }, 2000);
      } else {
        this.thong_bao = 'Thêm thông tin người tìm việc thất bại!';
      }
    }
  }
}
