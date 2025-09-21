import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-trang-danh-sach-nguoi-tim-viec',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-nguoi-tim-viec.html',
  styleUrls: ['./trang-danh-sach-nguoi-tim-viec.css']
})
export class TrangDanhSachNguoiTimViec implements OnInit {
  danh_sach_nguoi_tim_viec_full: any[] = []; 
  danh_sach_nguoi_tim_viec: any[] = [];      
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  async ngOnInit() {
    await this.layDanhSachNguoiTimViec();
  }

  async layDanhSachNguoiTimViec() {
    try {
      const response = await fetch('http://localhost:65001/api/API_WEB/layDanhSachNguoiTimViec', {
        method: "POST",
        headers: { 'Content-Type': 'application/json' }
      });

      const data = await response.json();
      if (data.success) {
        this.danh_sach_nguoi_tim_viec_full = data.danh_Sach || [];
        this.tongTrang = Math.ceil(this.danh_sach_nguoi_tim_viec_full.length / this.soLuongMoiTrang);
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
    this.danh_sach_nguoi_tim_viec = this.danh_sach_nguoi_tim_viec_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }
}
