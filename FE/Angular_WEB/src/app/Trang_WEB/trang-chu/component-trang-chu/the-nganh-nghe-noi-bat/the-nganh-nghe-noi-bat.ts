import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';

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
   @ViewChild('scrollContainer') scrollContainer!: ElementRef;
  danh_sach_viec_lam_noi_bat: any[] = [];
  showLeftBtn = false;
  showRightBtn = true;

  ngOnInit() {
    this.layDanhSachViecLamNoiBat();
  }

  layDanhSachViecLamNoiBat() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layNganhNgheNoiBat', {}).
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
    "tai_chinh": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_tai_chinh.png",
    "khac": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_nganh_nghe_khac.png",
    "bat_dong_san": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_bat_dong_san.png",
    "co_khi_dien_dien_tu": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_co_khi_dien_tu.png",
    "cong_tac_xa_hoi": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_cong_tac_xa_hoi.png",
    "du_lich": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_du_lich.png",
    "giao_duc": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_giao_duc.png",
    "hanh_chinh": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_hanh_chinh.png",
    "luat": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_luat.png",
    "nhan_su": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_nhan_su.png",
    "nong_lam_ngu_nghiep": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_nong_lam_ngu_nghiep.png",
    "san_xuat": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_san_xuat.png",
    "thiet_ke": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_thiet_ke.png",
    "truyen_thong": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_truyen_thong.png",
    "van_tai": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_van_tai.png",
    "xay_dung": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_xay_dung .png",
    "y_te": "anh_WEB/anh_icon_WEB/anh_icon_viec_lam/anh_icon_y_te.png",
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
  scrollLeft() {
    const container = this.scrollContainer.nativeElement;
    container.scrollBy({ left: -300, behavior: 'smooth' });
    setTimeout(() => this.checkScrollButtons(), 300);
  }

  scrollRight() {
    const container = this.scrollContainer.nativeElement;
    container.scrollBy({ left: 300, behavior: 'smooth' });
    setTimeout(() => this.checkScrollButtons(), 300);
  }

  @HostListener('window:resize')
  onResize() {
    this.checkScrollButtons();
  }

  checkScrollButtons() {
    if (!this.scrollContainer) return;
    
    const container = this.scrollContainer.nativeElement;
    this.showLeftBtn = container.scrollLeft > 10;
    this.showRightBtn = container.scrollLeft < (container.scrollWidth - container.clientWidth - 10);
    this.cdr.detectChanges();
  }

  @HostListener('scroll', ['$event.target'])
  onScroll(element: any) {
    this.checkScrollButtons();
  }
}
