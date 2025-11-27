import { Component } from '@angular/core';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
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
import { TheChuyenTrangTaoCv } from './component-trang-chu/the-chuyen-trang-tao-cv/the-chuyen-trang-tao-cv';
import { ComponentThongKeNguoiDung } from '../Component_trang_chu_quan_tri/thong-ke/thong-ke-nguoi-dung/component-thong-ke-nguoi-dung/component-thong-ke-nguoi-dung';
import { ComponentThongKeTuyenDungCuaCongTy } from '../Component_trang_chu_cong_ty/tuyen-dung/thong-ke-tuyen-dung/component-thong-ke-tuyen-dung-cua-cong-ty/component-thong-ke-tuyen-dung-cua-cong-ty';
import { ComponentThongKePhanLoaiNguoiDung } from '../Component_trang_chu_quan_tri/thong-ke/thong-ke-nguoi-dung/component-thong-ke-phan-loai-nguoi-dung/component-thong-ke-phan-loai-nguoi-dung';
import { ComponentThongKeTinTuyenDung } from '../Component_trang_chu_quan_tri/thong-ke/thong-ke-tin-tuyen-dung/component-thong-ke-tin-tuyen-dung/component-thong-ke-tin-tuyen-dung';
import { ComponentThongKeUngVienTungBai } from '../Component_trang_chu_cong_ty/tuyen-dung/thong-ke-tuyen-dung/component-thong-ke-ung-vien-tung-bai/component-thong-ke-ung-vien-tung-bai';

@Component({
  selector: 'app-trang-chu',
  imports: [RouterModule, CommonModule, RouterLink, FormsModule, HeaderWEB, FooterWeb, TheTimKiemViecLam, ThongTinNoiBat, 
    TheNganhNgheNoiBat, TheBanner, TheDanhGiaTuNguoiDung, TheViecLamDuocQuanTam, TheChuyenTrangTaoCv, 
    ComponentThongKeNguoiDung, ComponentThongKePhanLoaiNguoiDung, ComponentThongKeTinTuyenDung,
    ComponentThongKeUngVienTungBai, ComponentThongKeTuyenDungCuaCongTy],
  templateUrl: './trang-chu.html',
  styleUrls: ['./trang-chu.css']
})
export class TrangChu {
  constructor(public auth: Auth) {}
}
