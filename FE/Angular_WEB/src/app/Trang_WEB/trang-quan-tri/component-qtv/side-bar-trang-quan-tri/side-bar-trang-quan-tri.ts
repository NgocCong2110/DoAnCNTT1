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
  trangThai = false;
  chuyenDoi_TrangThai(){
    this.trangThai = !this.trangThai;
  }
}
