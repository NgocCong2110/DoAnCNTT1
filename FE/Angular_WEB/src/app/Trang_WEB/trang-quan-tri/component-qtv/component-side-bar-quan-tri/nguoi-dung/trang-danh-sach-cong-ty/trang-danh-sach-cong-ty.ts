import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE {
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
  cong_ty_chon: any | null = null;
  loading = true;
  error = '';

  index = 0;

  showXacNhanXoa = false;
  formXoa = false;

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

  moXacNhanXoa(cong_ty: any, index: number) {
    this.showXacNhanXoa = true;
    this.cong_ty_chon = cong_ty
    this.index = index;
  }

  xoaCongTy() {
    const ma_cong_ty = this.cong_ty_chon?.ma_cong_ty;
    if (!ma_cong_ty) return;
    this.danh_sach_cong_ty_full.splice(this.index, 1);
    this.loadTrang(this.trangHienTai);
    this.showXacNhanXoa = false;
    this.cd.detectChanges();
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/xoaCongTy', ma_cong_ty)
      .subscribe({
        next: (data) => {
          if (!data.success) {
            alert('Xóa thất bại, sẽ tải lại danh sách!');
            this.layDanhSachCongTy();
          }
        },
        error: () => {
          alert('Lỗi kết nối, tải lại danh sách!');
          this.layDanhSachCongTy();
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
    this.danh_sach_cong_ty = this.danh_sach_cong_ty_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
    this.cd.markForCheck();
  }
}
