import { Component } from '@angular/core';

@Component({
  selector: 'app-side-bar-trang-cong-ty',
  imports: [],
  templateUrl: './side-bar-trang-cong-ty.html',
  styleUrl: './side-bar-trang-cong-ty.css'
})
export class SideBarTrangCongTy {
  trangThai = false;
  chuyenDoiTrangThai(){
    this.trangThai = !this.trangThai;
  }
}
