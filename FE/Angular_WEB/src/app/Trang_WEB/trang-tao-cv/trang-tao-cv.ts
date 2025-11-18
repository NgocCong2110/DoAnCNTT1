import { Component, OnInit } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../services/auth';
import { ChangeDetectorRef } from '@angular/core';

interface mau_cv {
  name: string;
  image: string;
  route: string
}

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-trang-tao-cv',
  imports: [HeaderWEB, RouterLink, CommonModule],
  templateUrl: './trang-tao-cv.html',
  styleUrl: './trang-tao-cv.css'
})
export class TrangTaoCv implements OnInit {
  danh_sach_cv_nguoi_tim_viec: any;
  so_luong_cv = 0;
  constructor(private auth: Auth, private httpclient: HttpClient, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.layDanhSachCVOnlineNguoiTimViec();
  }
  mau_cv_co_san: mau_cv[] = [
    { name: 'Mẫu CV 1', image: 'anh_WEB/anh-mau-cv/anh-mau-cv-mac-dinh.png', route: '/app-mau-cv-mac-dinh' },
    { name: 'Mẫu CV 2', image: 'assets/cv2.png', route: '/app-mau-cv-cong-nghe' },
    { name: 'Mẫu CV 3', image: 'assets/cv3.png', route: '/cv-detail/3' }
  ]
  layDanhSachCVOnlineNguoiTimViec() {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachCVOnlineNguoiTimViec', ma_nguoi_tim_viec,
      { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv_nguoi_tim_viec = data.danh_sach;
            this.so_luong_cv = this.danh_sach_cv_nguoi_tim_viec.length;
            this.cd.detectChanges();
          }
          else {
            console.log("loi")
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
  }
  taoCV(route: string) {
    if (this.so_luong_cv >= 10) {
      alert("Bạn đã tạo tối đa 10 CV. Không thể tạo thêm!");
      return;
    }
    if (!this.auth.layThongTinNguoiDung()) {
      alert("Vui lòng đăng nhập để tạo CV!");
      return;
    }
    window.location.href = route;
  }
}
