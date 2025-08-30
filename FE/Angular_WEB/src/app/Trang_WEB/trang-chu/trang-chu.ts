import { Component } from '@angular/core';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { HeaderWEB } from '../Component/header-web/header-web';
import { FooterWeb } from '../Component/footer-web/footer-web';
import { TheTimKiemViecLam } from './component-trang-chu/the-tim-kiem-viec-lam/the-tim-kiem-viec-lam';
import { ThongTinNoiBat } from './component-trang-chu/thong-tin-noi-bat/thong-tin-noi-bat';

@Component({
  selector: 'app-trang-chu',
  imports: [RouterModule, RouterLink, FormsModule, HeaderWEB, FooterWeb, TheTimKiemViecLam, ThongTinNoiBat],
  templateUrl: './trang-chu.html',
  styleUrls: ['./trang-chu.css']
})
export class TrangChu {
  
}
