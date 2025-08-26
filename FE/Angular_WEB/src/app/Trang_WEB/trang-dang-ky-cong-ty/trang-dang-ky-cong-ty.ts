import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trang-dang-ky-cong-ty',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './trang-dang-ky-cong-ty.html',
  styleUrl: './trang-dang-ky-cong-ty.css'
})
export class TrangDangKyCongTy {

  constructor(private router: Router) { }

  form = {
    ten_CongTy: '',
    ten_Dn: '',
    mat_Khau: '',
    xac_Nhan_MatKhau: '',
    nguoi_DaiDien: '',
    ma_SoThue: '',
    ngay_ThanhLap: '',
    dia_Chi: '',
    dien_Thoai: '',
    email: '',
    loaiHinh: 'CongTyTNHH'
  };

  getThongTinDangKy() {
    return {
      ten_Cong_Ty: this.form.ten_CongTy,
      ten_Dn: this.form.ten_Dn,
      mat_Khau: this.form.mat_Khau,
      vai_Tro: 'cong_Ty'
    };
  }

  dangKy(formRef: any) {
    if (formRef.valid && this.form.mat_Khau === this.form.xac_Nhan_MatKhau) {
      console.log('Dữ liệu form:', this.getThongTinDangKy());
    }
  }

  thong_bao: string = '';

  async kiemTraEmail() {
    const response = await fetch("http://localhost:65001/api/API_WEB/xacThucGmail", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        email: this.form.email,
      })
    });
    const data = await response.json();
    if (data.success) {
      const themThongTin_CongTy = await fetch("http://localhost:65001/api/API_WEB/themThongTinCongTy", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(this.getThongTinDangKy())
      });
      const data2 = await themThongTin_CongTy.json();
      if (data2.success) {
        this.thong_bao = 'Thêm thông tin công ty thành công!';
        setTimeout(() => {
          this.router.navigate(['/trang-chu']);
        }, 2000);
      } else {
        this.thong_bao = 'Thêm thông tin công ty thất bại!';
      }
    }
  }
}
