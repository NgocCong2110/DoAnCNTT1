import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trang-them-goi-dich-vu-moi-quan-tri',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-them-goi-dich-vu-moi-quan-tri.html',
  styleUrl: './trang-them-goi-dich-vu-moi-quan-tri.css'
})
export class TrangThemGoiDichVuMoiQuanTri {
  ten_dich_vu = "";
  mo_ta = "";
  gia = 0.0;
  constructor(public httpclient: HttpClient){}

  taoDichVuMoi(){
    const thong_tin = {
      ten_dich_vu: this.ten_dich_vu,
      mo_ta: this.mo_ta,
      gia: this.gia
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/taoDichVuMoi', thong_tin)
      .subscribe({
        next: (data) => {
          if(data.success){
            alert("Thêm thông tin quản trị mới thành công");
          }
          else{
            alert("Thêm thông tin quản trị mới không thành công");
          }
        },
        error: (err) => {
          alert("Khong ket noi duoc API")
        }
      })
  }
}
