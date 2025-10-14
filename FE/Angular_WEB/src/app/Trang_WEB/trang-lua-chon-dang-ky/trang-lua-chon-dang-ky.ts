import { Component } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-lua-chon-dang-ky',
  imports: [FormsModule, CommonModule, HeaderWEB],
  templateUrl: './trang-lua-chon-dang-ky.html',
  styleUrls: ['./trang-lua-chon-dang-ky.css']
})
export class TrangLuaChonDangKy {
  loading: string = "";
  constructor(private router: Router) {}
  chonVaiTro(vaiTro: string) {
    this.loading = vaiTro;
    setTimeout(() => {
      if (vaiTro === 'cong-ty') {
        this.router.navigate(['/dang-ky-cong-ty']);
      } else {
        this.router.navigate(['/dang-ky']);
      }
    }, 500);
  }
}
