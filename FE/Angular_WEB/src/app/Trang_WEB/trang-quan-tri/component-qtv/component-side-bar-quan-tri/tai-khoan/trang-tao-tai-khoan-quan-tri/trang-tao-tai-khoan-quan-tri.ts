import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trang-tao-tai-khoan-quan-tri',
  imports: [FormsModule],
  templateUrl: './trang-tao-tai-khoan-quan-tri.html',
  styleUrl: './trang-tao-tai-khoan-quan-tri.css'
})
export class TrangTaoTaiKhoanQuanTri {
  email_quan_tri: string = '';
  matkhau_quan_tri: string = '';
  async taoTaiKhoanQuanTri() {
    const response = await fetch('http://localhost:65001/api/API_WEB/themThongTinQuanTriVien', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        email: this.email_quan_tri,
        matkhau: this.matkhau_quan_tri
      })
    });
    const data = await response.json();
    if(data.success) {
      alert("ok baybi")
    } else {
      alert("Lỗi: " + data.message);
    }
  }
}
