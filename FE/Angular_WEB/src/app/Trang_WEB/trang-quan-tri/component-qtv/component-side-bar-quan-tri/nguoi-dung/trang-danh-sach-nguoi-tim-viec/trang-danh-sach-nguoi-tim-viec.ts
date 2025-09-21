import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE{
  success: boolean;
  danh_Sach: any[];
}

@Component({
  selector: 'app-trang-danh-sach-nguoi-tim-viec',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-nguoi-tim-viec.html',
  styleUrls: ['./trang-danh-sach-nguoi-tim-viec.css']
})
export class TrangDanhSachNguoiTimViec implements OnInit {

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef) {}

  danh_sach_nguoi_tim_viec_full: any[] = []; 
  danh_sach_nguoi_tim_viec: any[] = [];      
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  ngOnInit() {
    this.layDanhSachNguoiTimViec();
  }

  layDanhSachNguoiTimViec() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachNguoiTimViec', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_nguoi_tim_viec_full = data.danh_Sach || [];
            this.tongTrang = Math.ceil(this.danh_sach_nguoi_tim_viec_full.length / this.soLuongMoiTrang);
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
    this.danh_sach_nguoi_tim_viec = this.danh_sach_nguoi_tim_viec_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
    this.cd.markForCheck();
  }
}
