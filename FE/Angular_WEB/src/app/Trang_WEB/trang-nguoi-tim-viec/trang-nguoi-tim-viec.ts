import { Component } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { SideBarTrangNguoiTimViec } from './component-ntv/side-bar-trang-nguoi-tim-viec/side-bar-trang-nguoi-tim-viec';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-trang-nguoi-tim-viec',
  imports: [HeaderWEB, SideBarTrangNguoiTimViec, RouterModule],
  templateUrl: './trang-nguoi-tim-viec.html',
  styleUrls: ['./trang-nguoi-tim-viec.css']
})
export class TrangNguoiTimViec {

}
