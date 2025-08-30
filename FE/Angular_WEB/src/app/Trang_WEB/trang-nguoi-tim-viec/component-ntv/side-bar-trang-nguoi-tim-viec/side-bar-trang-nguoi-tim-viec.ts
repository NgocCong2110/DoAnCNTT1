import { Component } from '@angular/core';
import { RouterLink } from '@angular/router'; 

@Component({
  selector: 'app-side-bar-trang-nguoi-tim-viec',
  imports: [RouterLink],
  templateUrl: './side-bar-trang-nguoi-tim-viec.html',
  styleUrl: './side-bar-trang-nguoi-tim-viec.css'
})
export class SideBarTrangNguoiTimViec {
  trangThai_BaiDang = false;
  trangThai_HoSo = false;
  trangThai_ThongBao = false;
  trangThai_TaiKhoan = false;
  chuyenDoi_TTBaiDang() {
    this.trangThai_BaiDang = !this.trangThai_BaiDang;
  }
  chuyenDoi_TTHoSo() {
    this.trangThai_HoSo = !this.trangThai_HoSo;
  }
  chuyenDoi_TTThongBao() {
    this.trangThai_ThongBao = !this.trangThai_ThongBao;
  }
  chuyenDoi_TTTaiKhoan() {
    this.trangThai_TaiKhoan = !this.trangThai_TaiKhoan;
  }
}
