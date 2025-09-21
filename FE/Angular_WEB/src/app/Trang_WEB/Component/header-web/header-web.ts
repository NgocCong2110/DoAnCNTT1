import { Component, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header-web',
  imports: [RouterLink, CommonModule],
  templateUrl: './header-web.html',
  styleUrls: ['./header-web.css']
})
export class HeaderWEB implements OnInit {
  nguoi_Dung: any;

  constructor(public auth: Auth, public router: Router) {}

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    }
  }

  vaiTro() {
    this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    let kieu_nguoi_dung = this.nguoi_Dung?.kieu_nguoi_dung;

    if (kieu_nguoi_dung == "quan_Tri_Vien") {
      this.router.navigate(['/trang-quan-tri']);
    } else if (kieu_nguoi_dung == "nguoi_Tim_Viec") {
      this.router.navigate(['/trang-nguoi-tim-viec']);
    } else if (kieu_nguoi_dung == "cong_Ty") {
      this.router.navigate(['/trang-cong-ty']);
    }
  }

  dangXuat() {
    this.auth.dangXuat();
    setTimeout(() => {
      this.router.navigate(['/dang-nhap']);
    }, 1500);
  }
}
