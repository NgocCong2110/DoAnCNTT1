import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

interface API_RESPONSE{
  success: boolean;
  danh_Sach: any[];
}

@Component({
  selector: 'app-trang-lich-su-giao-dich',
  imports: [CommonModule, CurrencyPipe],
  templateUrl: './trang-lich-su-giao-dich.html',
  styleUrl: './trang-lich-su-giao-dich.css'
})
export class TrangLichSuGiaoDich {
  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef) {}

  danh_sach_thanh_toan: any[] = []; 
  danh_sach_thanh_toan_full: any[] = [];      
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  ngOnInit() {
    this.layDanhSachNguoiTimViec();
  }

  layDanhSachNguoiTimViec() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layLichSuThanhToan', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_thanh_toan_full = data.danh_Sach || [];
            this.tongTrang = Math.ceil(this.danh_sach_thanh_toan_full.length / this.soLuongMoiTrang);
            this.loadTrang(this.trangHienTai);
          } else {
            this.error = 'Lỗi khi tải dữ liệu';
          }
          this.loading = false;
          this.cd.markForCheck();
        },
        error: (err) => {
          this.error = err.message || 'Không thể kết nối API';
          this.loading = false;
          this.cd.markForCheck();
        }
      });
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danh_sach_thanh_toan = this.danh_sach_thanh_toan_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
    this.cd.markForCheck();
  }
}
