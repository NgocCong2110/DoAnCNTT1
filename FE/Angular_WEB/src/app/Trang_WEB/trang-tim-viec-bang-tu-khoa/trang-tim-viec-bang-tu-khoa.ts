import { Component } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { ThanhTimKiem } from './component-trang-tim-viec-btk/thanh-tim-kiem/thanh-tim-kiem';
import { CacSelector } from './component-trang-tim-viec-btk/cac-selector/cac-selector';

@Component({
  selector: 'app-trang-tim-viec-bang-tu-khoa',
  imports: [HeaderWEB, ThanhTimKiem, CacSelector],
  templateUrl: './trang-tim-viec-bang-tu-khoa.html',
  styleUrl: './trang-tim-viec-bang-tu-khoa.css'
})
export class TrangTimViecBangTuKhoa {

}
