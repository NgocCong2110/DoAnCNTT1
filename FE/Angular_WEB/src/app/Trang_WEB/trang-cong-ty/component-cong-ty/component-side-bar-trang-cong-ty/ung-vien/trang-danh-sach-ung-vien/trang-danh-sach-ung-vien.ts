import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-danh-sach-ung-vien',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-ung-vien.html',
  styleUrls: ['./trang-danh-sach-ung-vien.css']
})
export class TrangDanhSachUngVien implements OnInit {

  thongTin: any = null;

  danh_sach_ung_vien_full: any[] = [];
  danh_sach_ung_vien: any[] = [];
  loading = true;
  error = '';

  pop_up_lay_thong_tin_that_bai = false;

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef){
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.layDanhSachUngVien();
  }

  layDanhSachUngVien() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;

    if (!ma_Cong_Ty) {
      this.error = "Không tìm thấy thông tin công ty (vui lòng đăng nhập lại)";
      this.loading = false;
      return;
    }

    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachUngVien',{ ma_cong_ty: ma_Cong_Ty })
      .subscribe({
      next: (data) => {
        if (data.success) {
          this.danh_sach_ung_vien_full = data.danh_sach;
          this.tongTrang = Math.ceil(this.danh_sach_ung_vien_full.length / this.soLuongMoiTrang);
          this.loadTrang(this.trangHienTai);
        } else {
          this.pop_up_lay_thong_tin_that_bai = true;
          setTimeout(() => this.pop_up_lay_thong_tin_that_bai = false, 1500);
        }
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Lỗi khi gọi API:', err);
        this.error = "Lỗi khi tải dữ liệu";
        this.loading = false;
        this.cd.detectChanges();
      }
    });
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danh_sach_ung_vien = this.danh_sach_ung_vien_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }
}
