import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header-web',
  imports: [RouterLink, CommonModule],
  templateUrl: './header-web.html',
  styleUrl: './header-web.css'
})
export class HeaderWEB {
  nguoi_Dung: any;
  constructor(public auth: Auth, public router: Router) {}

  ngOnInit() {
    if(typeof Window === "undefined") {
      this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    }
  }

  //Lay vai tro nguoi dung
  vaiTro(){
    this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    let vaiTro = this.nguoi_Dung?.vai_Tro;
    if(vaiTro == "admin") {
      this.router.navigate(['/trang-quan-tri']);
    }
    else if(vaiTro == "nguoi_Tim_Viec") {
      this.router.navigate(['/trang-nguoi-tim-viec']);
    }
    else if(vaiTro == "cong_Ty"){
      this.router.navigate(['/trang-cong-ty']);
    }
  }

  dangXuat() {
    this.auth.dangXuat();
    setTimeout(() => {
      this.router.navigate(['/trang-dang-nhap']);
    }, 1500);
  }
}
