import { Component } from '@angular/core';

@Component({
  selector: 'app-side-bar-trang-nguoi-tim-viec',
  imports: [],
  templateUrl: './side-bar-trang-nguoi-tim-viec.html',
  styleUrl: './side-bar-trang-nguoi-tim-viec.css'
})
export class SideBarTrangNguoiTimViec {
  trangThai = false;
  chuyenDoiTrangThai(){
    this.trangThai = !this.trangThai
  }
}
