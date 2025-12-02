import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThongTinViecLam } from '../../../../services/thong-tin-viec-lam-service/thong-tin-viec-lam';

interface API_RESPONSE {
  success: boolean;
  thong_tin: any;
  message: string;
  danh_sach: any;
}

@Component({
  selector: 'app-thanh-tim-kiem',
  imports: [FormsModule, CommonModule],
  templateUrl: './thanh-tim-kiem.html',
  styleUrl: './thanh-tim-kiem.css'
})

export class ThanhTimKiem implements OnInit{

  du_lieu = false;

  pop_up_lay_thong_tin_that_bai = false;

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef, public vl: ThongTinViecLam) { }

  ngOnInit(): void {
    this.layDanhSachTinhThanh();
  }

  tu_khoa_tim_kiem = "";

  danh_sach_tinh_thanh: any[] = [];
  ma_tinh_duoc_chon: number = 0;

  thongTinTim() {
    const thong_tin = {
      tu_khoa: this.tu_khoa_tim_kiem,
      ma_tinh: this.ma_tinh_duoc_chon
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/duaRaDeXuat', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.du_lieu = true;
            this.vl.capNhatDuLieu(data.thong_tin);
          }
          else {
            this.pop_up_lay_thong_tin_that_bai = true;
          }
          this.cd.detectChanges();
        },
        error: (err) => {
          this.pop_up_lay_thong_tin_that_bai = true;
          this.cd.detectChanges();
        }
      })
  }

  layDanhSachTinhThanh() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachTinhThanh', {})
      .subscribe({
        next: (data) => {
          if (data.success && data.danh_sach) {

            this.danh_sach_tinh_thanh = data.danh_sach;

            this.ma_tinh_duoc_chon = 0;

            this.cd.detectChanges();
          } else {
            console.error('Lấy danh sách tỉnh thành thất bại:', data.message);
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi kết nối khi lấy tỉnh thành:', err);
          this.pop_up_lay_thong_tin_that_bai = true;
          this.cd.detectChanges();
        }
      });
  }
}
