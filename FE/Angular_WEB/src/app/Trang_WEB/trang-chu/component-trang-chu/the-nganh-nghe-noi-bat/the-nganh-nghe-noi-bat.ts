import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Route, RouterModule } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';

interface API_RESPONSE {
  success: boolean,
  danh_sach: any[]
}
@Component({
  selector: 'app-the-nganh-nghe-noi-bat',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './the-nganh-nghe-noi-bat.html',
  styleUrl: './the-nganh-nghe-noi-bat.css'
})
export class TheNganhNgheNoiBat {
  constructor(private httpclient: HttpClient, private cdr: ChangeDetectorRef, private router: Router) { }

  danh_sach_viec_lam_noi_bat: any[] = [];

  ngOnInit() {
    this.layDanhSachViecLamNoiBat();
  }

  layDanhSachViecLamNoiBat() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layNganhNgheNoiBat', {}).
      subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_viec_lam_noi_bat = data.danh_sach;
            this.cdr.markForCheck();
          }
        }
      });
  }

  icon_viec_lam: { [key: string]: string } = {
    "cong_nghe_thong_tin": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_cntt.webp",
    "ke_toan": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_ke_toan.webp",
    "marketing": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_marketing.webp",
    "sales": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_sales.webp",
    "cham_soc_khach_hang": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_cham_soc_khach_hang.webp",
  }

  nganh_nghe_map: { [key: string]: string } = {
    cong_nghe_thong_tin: 'Công nghệ thông tin',
    cham_soc_khach_hang: 'Chăm sóc khách hàng',
    sales: 'Sales',
    tai_chinh: 'Tài chính',
    marketing: 'Marketing',
    ban_hang: 'Bán hàng',
    san_xuat: 'Sản xuất',
    giao_duc: 'Giáo dục',
    y_te: 'Y tế',
    hanh_chinh: 'Hành chính',
    xay_dung: 'Xây dựng',
    luat: 'Luật - Pháp lý',
    bat_dong_san: 'Bất động sản',
    du_lich: 'Du lịch',
    nong_nghiep: 'Nông nghiệp',
    nghe_thuat: 'Nghệ thuật',
    van_tai: 'Vận tải'
  }

  layTenNganhNghe(ten_nganh_nghe: string): string {
    return this.nganh_nghe_map[ten_nganh_nghe] || "Ngành nghề khác";
  }

  xemDanhSachViecLam(nganh_nghe: any) {
    this.router.navigate(['trang-tim-viec-theo-tu-khoa'], {
      queryParams: { nganh: nganh_nghe.nganh_nghe }
    });
  }
}
