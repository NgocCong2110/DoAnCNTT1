import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE{
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-danh-sach-cong-ty',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-cong-ty.html',
  styleUrls: ['./trang-danh-sach-cong-ty.css']
})
export class TrangDanhSachCongTy implements OnInit {

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef) { }

  danh_sach_cong_ty_full: any[] = [];
  danh_sach_cong_ty: any[] = [];
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  ngOnInit() {
    this.layDanhSachCongTy();
  }

  layDanhSachCongTy() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachCongTy', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cong_ty_full = data.danh_sach;
            this.tongTrang = Math.ceil(this.danh_sach_cong_ty_full.length / this.soLuongMoiTrang);
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
    this.danh_sach_cong_ty = this.danh_sach_cong_ty_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
    this.cd.markForCheck();
  }
}
