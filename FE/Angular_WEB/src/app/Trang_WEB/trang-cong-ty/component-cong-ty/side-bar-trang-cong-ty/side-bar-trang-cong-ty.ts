import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-side-bar-trang-cong-ty',
  imports: [RouterLink, RouterModule],
  templateUrl: './side-bar-trang-cong-ty.html',
  styleUrls: ['./side-bar-trang-cong-ty.css']
})
export class SideBarTrangCongTy {
  trangThai_BaiDang = false;
  trangThai_UngVien = false;
  trangThai_ThongBao = false;
  trangThai_TaiKhoan = false;
  trangThai_DichVu = false;
  chuyenDoi_TTBaiDang() {
    this.trangThai_BaiDang = !this.trangThai_BaiDang;
  }
  chuyenDoi_TTUngVien() {
    this.trangThai_UngVien = !this.trangThai_UngVien;
  }
  chuyenDoi_TTThongBao() {
    this.trangThai_ThongBao = !this.trangThai_ThongBao;
  }
  chuyenDoi_TTTaiKhoan() {
    this.trangThai_TaiKhoan = !this.trangThai_TaiKhoan;
  }
  chuyenDoi_TTDichVu() {
    this.trangThai_DichVu = !this.trangThai_DichVu;
  }
}
