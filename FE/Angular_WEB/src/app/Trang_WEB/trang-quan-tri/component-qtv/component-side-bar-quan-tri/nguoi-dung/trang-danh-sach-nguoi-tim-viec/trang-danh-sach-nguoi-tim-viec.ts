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
  nguoi_tim_viec_chon: any | null = null;
  error = '';

  index = 0;

  showXacNhanXoa = false;
  formXoa = false;

  trangHienTai = 1;
  soLuongMoiTrang = 15;
  tongTrang = 1;

  ngOnInit() {
    this.layDanhSachNguoiTimViec();
  }

  layDanhSachNguoiTimViec() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachNguoiTimViec', {})
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

  moXacNhanXoa(nguoi_tim_viec: any, index: number) {
    this.showXacNhanXoa = true;
    this.nguoi_tim_viec_chon = nguoi_tim_viec
    this.index = index;
  }

  xoaNguoiTimViec() {
    const ma_nguoi_tim_viec = this.nguoi_tim_viec_chon?.ma_nguoi_tim_viec;
    if (!ma_nguoi_tim_viec) return;
    this.danh_sach_nguoi_tim_viec_full.splice(this.index, 1);
    this.loadTrang(this.trangHienTai);
    this.showXacNhanXoa = false;
    this.cd.detectChanges();
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaNguoiTimViec', ma_nguoi_tim_viec)
      .subscribe({
        next: (data) => {
          if (!data.success) {
            alert('Xóa thất bại, sẽ tải lại danh sách!');
            this.layDanhSachNguoiTimViec();
          }
        },
        error: () => {
          alert('Lỗi kết nối, tải lại danh sách!');
          this.layDanhSachNguoiTimViec();
        }
      });
  }

  huyXoa() {
    this.showXacNhanXoa = false;
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
