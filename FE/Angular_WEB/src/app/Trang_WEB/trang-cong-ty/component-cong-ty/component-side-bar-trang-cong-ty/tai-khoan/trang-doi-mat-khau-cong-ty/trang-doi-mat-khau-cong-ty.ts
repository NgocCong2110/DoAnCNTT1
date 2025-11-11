import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-doi-mat-khau-cong-ty',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './trang-doi-mat-khau-cong-ty.html',
  styleUrls: ['./trang-doi-mat-khau-cong-ty.css']
})
export class TrangDoiMatKhauCongTy {
  constructor(private httpClient: HttpClient, private auth: Auth) {}

  mat_khau_cu = '';
  mat_khau_moi = '';
  xac_nhan_mat_khau = '';

  guiForm() {
    if (!this.mat_khau_cu || !this.mat_khau_moi || !this.xac_nhan_mat_khau) {
      alert('Vui lòng nhập đầy đủ thông tin.');
      return;
    }

    if (this.mat_khau_moi.length < 6) {
      alert('Mật khẩu mới phải có ít nhất 6 ký tự.');
      return;
    }

    if (this.mat_khau_moi !== this.xac_nhan_mat_khau) {
      alert('Mật khẩu xác nhận không khớp.');
      return;
    }
    const thong_tin = {
      mat_khau_dn_cong_ty: this.mat_khau_cu,
      ma_cong_ty: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.cong_ty.ma_cong_ty
    }
    this.httpClient
      .post<any>('http://localhost:7000/api/API_WEB/kiemTraMatKhauCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.capNhatMatKhauCongTy(this.mat_khau_moi);
          } else {
            alert('Mật khẩu cũ không chính xác.');
          }
        },
        error: (err) => {
          console.error('Lỗi kết nối server:', err);
          alert('Không thể kết nối đến server.');
        }
      });
  }

  capNhatMatKhauCongTy(mat_khau_moi: string) {
    const thong_tin = {
      mat_khau_dn_cong_ty: mat_khau_moi,
      ma_cong_ty: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.cong_ty.ma_cong_ty
    }
    this.httpClient
      .post<any>('http://localhost:7000/api/API_WEB/capNhatMatKhauCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            alert('Cập nhật mật khẩu thành công!');
          } else {
            alert('Cập nhật thất bại!');
          }
        },
        error: (err) => {
          console.error('Lỗi kết nối server:', err);
          alert('Không thể kết nối đến server.');
        }
      });
  }
}
