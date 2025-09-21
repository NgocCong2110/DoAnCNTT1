import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-dang-ky-ntv',
  imports: [RouterLink, FormsModule, CommonModule, HeaderWEB],
  templateUrl: './trang-dang-ky-ntv.html',
  styleUrls: ['./trang-dang-ky-ntv.css']
})
export class TrangDangKyNTV {
  ten_dang_ky = '';
  email_dang_ky = '';
  password_dang_ky = '';
  confirm_password_dang_ky = '';
  thong_bao = '';
  hien_thong_bao = false;

  constructor(private router: Router) { }

  getThongTinDangKy() {
    return {
      ten_dang_ky: this.ten_dang_ky,
      email: this.email_dang_ky,
      mat_khau: this.password_dang_ky,
      vai_tro: 'nguoi_tim_viec'
    };
  }

  kiemTraMatKhau(event: Event) {
    event.preventDefault();

    if (/\s/.test(this.ten_dang_ky)) {
      this.thong_bao = 'Tên đăng ký không được chứa khoảng trắng!';
      return;
    }


    if (this.password_dang_ky.length < 6) {
      this.thong_bao = 'Mật khẩu phải có ít nhất 6 ký tự!';
      return;
    }

    if (this.password_dang_ky !== this.confirm_password_dang_ky) {
      this.thong_bao = 'Mật khẩu xác nhận không khớp!';
      return;
    }

    this.thong_bao = '';
    this.kiemTraEmail();
  }

  async kiemTraEmail() {
    try {
      const response = await fetch("http://localhost:65001/api/API_WEB/xacThucGmail", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email: this.email_dang_ky })
      });
      const data = await response.json();

      if (data.success) {
        const themThongTin_NTV = await fetch("http://localhost:65001/api/API_WEB/themThongTinNguoiTimViec", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(this.getThongTinDangKy())
        });

        const data2 = await themThongTin_NTV.json();
        if (data2.success) {
          this.thong_bao = 'Đăng ký thông tin thành công!';
          this.hien_thong_bao =  true;
          setTimeout(() => {
            this.hien_thong_bao = false;
            this.router.navigate(['/trang-chu']);
          }, 2000);
        } else {
          this.thong_bao = 'Đăng ký thông tin thất bại!';
        }
      } else {
        this.thong_bao = 'Email không hợp lệ!';
      }
    } catch (err) {
      console.error(err);
    }
  }
}
