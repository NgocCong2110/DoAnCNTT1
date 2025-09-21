import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE{
  success: boolean,
  danh_sach: any
}

@Component({
  selector: 'app-trang-thong-bao-ntv',
  imports: [CommonModule],
  templateUrl: './trang-thong-bao-ntv.html',
  styleUrl: './trang-thong-bao-ntv.css'
})
export class TrangThongBaoNtv implements OnInit{

  thongTin: any;

  constructor(public auth: Auth, public httpclient: HttpClient) {}

  danh_sach_thong_bao: any[] = [];
  pop_up_lay_thong_tin_that_bai = false;
  loading = true;

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao(){
    this.danh_sach_thong_bao = [];
    this.loading = true;
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachThongBao',{})
    .subscribe({
      next: (data) =>{
        this.loading = false;
        if(data.success){
          this.danh_sach_thong_bao = data.danh_sach;
        }
        else{
          this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            },1500)
        }
      }
    });
  }
  chonThongBao(event: any){
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const value = event.target.value;
    if(value == "toan_Bo"){
      this.danhSachThongBao();
      return;
    }
    const value_num = Number(value);
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/chonThongBaoCoDinh', { loai_thong_bao : value_num})
      .subscribe({
        next: (data) => {
          this.loading = false;
          if(data.success){
            this.danh_sach_thong_bao = data.danh_sach;
          }
          else{
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            },1500)
          }
        }
      });
  }
}
