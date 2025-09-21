import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-danh-sach-ung-vien',
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-danh-sach-ung-vien.html',
  styleUrl: './trang-danh-sach-ung-vien.css'
})
export class TrangDanhSachUngVien implements OnInit {

  thongTin: any = null;
  constructor(private auth: Auth) { 
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  danh_sach_ung_vien_full: any[] = [];
  danh_sach_ung_vien: any[] = [];
  loading = true;
  error = '';

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  ngOnInit() {
    this.layDanhSachUngVien();
  }

  async layDanhSachUngVien() {
    try {
      const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;

      if (!ma_Cong_Ty) {
        this.error = "Không tìm thấy thông tin công ty (vui lòng đăng nhập lại)";
        this.loading = false;
        return;
      }

      const response = await fetch("http://localhost:65001/api/API_WEB/layDanhSachUngVien", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ ma_cong_ty: ma_Cong_Ty })
      });

      const data = await response.json();
      if (data.success) {
        this.danh_sach_ung_vien_full = data.danh_sach;
        this.tongTrang = Math.ceil(this.danh_sach_ung_vien_full.length / this.soLuongMoiTrang);
        this.loadTrang(this.trangHienTai);
      } else {
        this.error = data.thong_bao;
      }
    } catch (err) {
      console.error(err);
      this.error = "Lỗi khi tải dữ liệu";
    } finally {
      this.loading = false;
    }
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
