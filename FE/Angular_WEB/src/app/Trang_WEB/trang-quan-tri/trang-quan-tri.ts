import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SideBarTrangQuanTri } from './component-qtv/side-bar-trang-quan-tri/side-bar-trang-quan-tri';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-quan-tri',
  imports: [RouterModule, SideBarTrangQuanTri, HeaderWEB],
  standalone: true,
  templateUrl: './trang-quan-tri.html',
  styleUrls: ['./trang-quan-tri.css']
})
export class TrangQuanTri {

}
