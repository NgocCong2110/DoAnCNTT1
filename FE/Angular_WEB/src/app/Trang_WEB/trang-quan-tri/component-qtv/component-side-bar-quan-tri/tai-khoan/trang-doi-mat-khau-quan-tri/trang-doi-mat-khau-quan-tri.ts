import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-doi-mat-khau-quan-tri',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './trang-doi-mat-khau-quan-tri.html',
  styleUrl: './trang-doi-mat-khau-quan-tri.css'
})
export class TrangDoiMatKhauQuanTri {
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
      mat_khau: this.mat_khau_cu,
      ma_quan_tri: this.auth.layThongTinNguoiDung()?.ma_quan_tri
    }
    this.httpClient
      .post<any>('http://localhost:7000/api/API_WEB/kiemTraMatKhauQuanTri', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.capNhatMatKhauQuanTri(this.mat_khau_moi);
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

  capNhatMatKhauQuanTri(mat_khau_moi: string) {
    const thong_tin = {
      mat_khau: mat_khau_moi,
      ma_quan_tri: this.auth.layThongTinNguoiDung()?.ma_quan_tri
    }
    this.httpClient
      .post<any>('http://localhost:7000/api/API_WEB/capNhatMatKhauQuanTri', thong_tin)
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
