import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-trang-danh-sach-cong-ty',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-cong-ty.html',
  styleUrls: ['./trang-danh-sach-cong-ty.css']
})
export class TrangDanhSachCongTy implements OnInit {
  danh_sach_cong_ty_full: any[] = []; 
  danh_sach_cong_ty: any[] = [];     
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  async ngOnInit() {
    await this.layDanhSachCongTy();
  }

  async layDanhSachCongTy() {
    try {
      const response = await fetch('http://localhost:65001/api/API_WEB/layDanhSachCongTy', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
      });

      const data = await response.json();
      if (data.success) {
        this.danh_sach_cong_ty_full = data.danh_sach;
        this.tongTrang = Math.ceil(this.danh_sach_cong_ty_full.length / this.soLuongMoiTrang);
        this.loadTrang(this.trangHienTai);
      } else {
        this.error = data.message || 'Lỗi khi tải dữ liệu';
      }
    } catch (err: any) {
      this.error = err.message || 'Không thể kết nối API';
    } finally {
      this.loading = false;
    }
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
  }
}
