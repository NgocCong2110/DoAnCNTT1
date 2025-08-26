import { Component } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { SideBarTrangCongTy } from './component-cong-ty/side-bar-trang-cong-ty/side-bar-trang-cong-ty';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-trang-cong-ty',
  imports: [HeaderWEB, SideBarTrangCongTy, RouterLink],
  templateUrl: './trang-cong-ty.html',
  styleUrls: ['./trang-cong-ty.css']
})
export class TrangCongTy {

}
