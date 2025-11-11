import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../services/auth';
import { HeaderWEB } from '../../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-thong-bao',
  imports: [CommonModule, HeaderWEB],
  templateUrl: './trang-thong-bao.html',
  styleUrl: './trang-thong-bao.css'
})
export class TrangThongBao implements OnInit{

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {}

  danh_sach_thong_bao: any[] = [];

  pop_up_lay_thong_tin_that_bai = false;

  loading = true;

  error = false;  

  thongTin: any;

  chiTietThongBao: any = null;

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao(){
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const thong_tin = {
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung()?.kieu_nguoi_dung ?? 'nguoi_tim_viec',
      ma_nguoi_tim_viec: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec ?? null
    }
    
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachThongBao', thong_tin )
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
            },1500);
        }
        this.cd.detectChanges();
      }
    });
  }

  chonThongBao(event: any){
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const value = event.target.value;
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_nguoi_tim_viec || 0;
    const ma_cong_ty = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_cong_ty || 0;
    const ma_quan_tri = this.auth.layThongTinNguoiDung().ma_quan_tri || 0;
    if(value === 'toan_Bo'){
      this.danhSachThongBao();
      return;
    }
    
    const value_num = Number(value);
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/chonThongBaoCoDinh', { loai_thong_bao : value_num, ma_nguoi_tim_viec, ma_cong_ty, ma_quan_tri })
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
            },1500);
          }
          this.cd.detectChanges();
        }
      });
  }

  xemChiTiet(tb: any) {
    this.chiTietThongBao = tb;
  }

  hienToastLoi() {
    this.pop_up_lay_thong_tin_that_bai = true;
    setTimeout(() => {
      this.pop_up_lay_thong_tin_that_bai = false;
    }, 1500);
  }
}
