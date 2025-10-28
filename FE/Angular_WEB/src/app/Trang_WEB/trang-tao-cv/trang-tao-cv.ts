import { Component } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

interface mau_cv{
  name: string;
  image: string;
  route: string
}

@Component({
  selector: 'app-trang-tao-cv',
  imports: [HeaderWEB, RouterLink, CommonModule],
  templateUrl: './trang-tao-cv.html',
  styleUrl: './trang-tao-cv.css'
})
export class TrangTaoCv {
  mau_cv_co_san: mau_cv[] = [
    { name: 'Mẫu CV 1', image: 'anh_WEB/anh-mau-cv/anh-mau-cv-mac-dinh.png', route: '/app-mau-cv-mac-dinh' },
    { name: 'Mẫu CV 2', image: 'assets/cv2.png', route: '/cv-detail/2' },
    { name: 'Mẫu CV 3', image: 'assets/cv3.png', route: '/cv-detail/3' }
  ]
}
