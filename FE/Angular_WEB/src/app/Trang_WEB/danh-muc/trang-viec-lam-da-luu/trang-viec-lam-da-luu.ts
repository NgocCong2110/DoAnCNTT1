import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderWEB } from '../../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { FooterWeb } from '../../Component/footer-web/footer-web';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-viec-lam-da-luu',
  imports: [CommonModule, FormsModule, HeaderWEB, FooterWeb],
  templateUrl: './trang-viec-lam-da-luu.html',
  styleUrl: './trang-viec-lam-da-luu.css'
})
export class TrangViecLamDaLuu implements OnInit {
  danh_sach_bai_dang_da_luu: any[] = [];
  thongTin: any;
  loading: boolean = true;
  error : boolean = false;

  pop_up_lay_thong_tin_that_bai = false;
  
  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef, private router: Router) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }
  
  ngOnInit(): void {
    this.layDanhSachBaiDangDaLuu();
  }

  layDanhSachBaiDangDaLuu() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachBaiDangDaLuu', { ma_nguoi_luu: this.thongTin?.thong_tin_chi_tiet?.nguoi_tim_viec.ma_nguoi_tim_viec })
      .subscribe({
        next: (data) => {
          this.loading = false;
          if(data.success){
            this.danh_sach_bai_dang_da_luu = data.danh_sach || [];
          }
          this.cd.detectChanges();
        },
        error: (err) => {
          this.pop_up_lay_thong_tin_that_bai = true;
          this.loading = false;
          this.danh_sach_bai_dang_da_luu = [];
          this.cd.detectChanges();
        }
      });
  }
  dieuHuongToiTrangViecLam(bai: any){
    this.router.navigate(['trang-chi-tiet-viec-lam'], { queryParams: {ma_bai_dang: bai.ma_bai_dang}})
  }

  huyLuuBaiDang(bai: any, index: number){
    const ma_nguoi_luu = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.nguoi_tim_viec?.ma_nguoi_tim_viec;
    const thong_tin = {
      ma_nguoi_luu: ma_nguoi_luu,
      ma_bai_dang: bai.ma_bai_dang
    }
    console.log(thong_tin)
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/huyLuuBaiDang', thong_tin)
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_bai_dang_da_luu.splice(index, 1);
            alert("Huy luu bai dang thanh cong");
            this.cd.detectChanges();
          }
          else{
            alert(data.message);
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
        }
      })
  }
}
