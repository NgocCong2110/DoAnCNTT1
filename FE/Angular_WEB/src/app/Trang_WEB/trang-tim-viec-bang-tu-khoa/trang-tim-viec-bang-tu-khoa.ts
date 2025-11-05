import { Component } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { ThanhTimKiem } from './component-trang-tim-viec-btk/thanh-tim-kiem/thanh-tim-kiem';
import { CacSelector } from './component-trang-tim-viec-btk/cac-selector/cac-selector';
import { FooterWeb } from '../Component/footer-web/footer-web';

@Component({
  selector: 'app-trang-tim-viec-bang-tu-khoa',
  imports: [HeaderWEB, ThanhTimKiem, CacSelector, FooterWeb],
  templateUrl: './trang-tim-viec-bang-tu-khoa.html',
  styleUrl: './trang-tim-viec-bang-tu-khoa.css'
})
export class TrangTimViecBangTuKhoa {

}
