import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE
{
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-lich-su-ung-tuyen',
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-lich-su-ung-tuyen.html',
  styleUrl: './trang-lich-su-ung-tuyen.css'
})
export class TrangLichSuUngTuyen implements OnInit{

  thongTin: any;

  loading = true;

  error = true;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  danh_sach_lich_su_ung_tuyen: any[] = [];

  pop_up_lay_thong_tin_that_bai = false;
  
  ngOnInit(): void {
    this.layDanhSachLichSuUngTuyen();
  }

  async layDanhSachLichSuUngTuyen(){
    this.danh_sach_lich_su_ung_tuyen = [];
    this.loading = true;
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachLichSuUngTuyen', { ma_nguoi_tim_viec: this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec })
      .subscribe({
        next: (data) => {
          this.loading = false;
          this.error = false;
          if(data.success){
            this.danh_sach_lich_su_ung_tuyen = data.danh_sach;
          }
          else{
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            },1500);
          }
          this.cd.detectChanges();
        }
      })
  }

}
