import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-side-bar-trang-quan-tri',
  imports: [RouterLink, CommonModule],
  templateUrl: './side-bar-trang-quan-tri.html',
  styleUrls: ['./side-bar-trang-quan-tri.css']
})
export class SideBarTrangQuanTri {
  trangThai_QLNguoiDung = false;
  trangThai_QLBaiDang = false;
  trangThai_QLDanhMuc = false;
  trangThai_QLDichVu = false;
  trangThai_ThongKe = false;
  trangThai_ThongBaoAdmin = false;
  trangThai_TaiKhoan = false;
  chuyenDoi_TTQLNguoiDung(){
    this.trangThai_QLNguoiDung = !this.trangThai_QLNguoiDung;
  }
  chuyenDoi_TTQLBaiDang(){
    this.trangThai_QLBaiDang = !this.trangThai_QLBaiDang;
  }
  chuyenDoi_TTQLDanhMuc(){
    this.trangThai_QLDanhMuc = !this.trangThai_QLDanhMuc;
  }
  chuyenDoi_TTQLDichVu(){
    this.trangThai_QLDichVu = !this.trangThai_QLDichVu
  }
  chuyenDoi_TTThongKe(){
    this.trangThai_ThongKe = !this.trangThai_ThongKe;
  }
  chuyenDoi_TTThongBaoAdmin(){
    this.trangThai_ThongBaoAdmin = !this.trangThai_ThongBaoAdmin;
  }
  chuyenDoi_TTTaiKhoan(){
    this.trangThai_TaiKhoan = !this.trangThai_TaiKhoan;
  }
}
