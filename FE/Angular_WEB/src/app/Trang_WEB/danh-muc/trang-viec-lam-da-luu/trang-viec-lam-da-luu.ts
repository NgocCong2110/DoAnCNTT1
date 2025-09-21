import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderWEB } from '../../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE {
  success: boolean,
  danh_sach: []
}

@Component({
  selector: 'app-trang-viec-lam-da-luu',
  imports: [CommonModule, FormsModule, HeaderWEB],
  templateUrl: './trang-viec-lam-da-luu.html',
  styleUrl: './trang-viec-lam-da-luu.css'
})
export class TrangViecLamDaLuu implements OnInit {
  danh_sach_bai_dang_da_luu: any[] = [];
  thongTin: any;
  loading: boolean = true;
  error : boolean = false;

  pop_up_lay_thong_tin_that_bai = false;
  
  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }
  
  ngOnInit(): void {
    this.layDanhSachBaiDangDaLuu();
  }

  layDanhSachBaiDangDaLuu() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachBaiDangDaLuu', { ma_nguoi_luu: this.thongTin?.thong_tin_chi_tiet.ma_nguoi_dung })
      .subscribe({
        next: (data) => {
          this.loading = false;
          if(data.success){
            this.danh_sach_bai_dang_da_luu = data.danh_sach;
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
}
