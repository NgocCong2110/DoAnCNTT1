import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
}

@Component({
  selector: 'app-trang-tao-tai-khoan-quan-tri',
  imports: [FormsModule],
  templateUrl: './trang-tao-tai-khoan-quan-tri.html',
  styleUrl: './trang-tao-tai-khoan-quan-tri.css'
})
export class TrangTaoTaiKhoanQuanTri {

  constructor(public httpclient: HttpClient) { }

  email_quan_tri: string = '';
  matkhau_quan_tri: string = '';
  taoTaiKhoanQuanTri() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/themThongTinQuanTriVien', {email: this.email_quan_tri, mat_khau: this.matkhau_quan_tri})
      .subscribe({
        next: (data) => {
          if(data.success){
            alert("Thêm thông tin quản trị mới thành công");
          }
          else{
            alert("Thêm thông tin quản trị mới không thành công");
          }
        }
      })
    }
}
