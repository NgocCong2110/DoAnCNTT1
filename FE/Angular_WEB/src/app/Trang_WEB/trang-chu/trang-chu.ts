import { Component } from '@angular/core';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { HeaderWEB } from '../Component/header-web/header-web';
import { FooterWeb } from '../Component/footer-web/footer-web';
import { TheTimKiemViecLam } from './component-trang-chu/the-tim-kiem-viec-lam/the-tim-kiem-viec-lam';
import { ThongTinNoiBat } from './component-trang-chu/thong-tin-noi-bat/thong-tin-noi-bat';
import { TheBanner } from './component-trang-chu/the-banner/the-banner';
import { TheDanhGiaTuNguoiDung } from './component-trang-chu/the-danh-gia-tu-nguoi-dung/the-danh-gia-tu-nguoi-dung';
import { TheViecLamDuocQuanTam } from './component-trang-chu/the-viec-lam-duoc-quan-tam/the-viec-lam-duoc-quan-tam';
import { TheNganhNgheNoiBat } from './component-trang-chu/the-nganh-nghe-noi-bat/the-nganh-nghe-noi-bat';

@Component({
  selector: 'app-trang-chu',
  imports: [RouterModule, RouterLink, FormsModule, HeaderWEB, FooterWeb, TheTimKiemViecLam, ThongTinNoiBat, TheNganhNgheNoiBat, TheBanner, TheDanhGiaTuNguoiDung, TheViecLamDuocQuanTam],
  templateUrl: './trang-chu.html',
  styleUrls: ['./trang-chu.css']
})
export class TrangChu {
  
}
