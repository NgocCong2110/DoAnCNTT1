import { Component, OnInit } from '@angular/core';
import { HeaderWEB } from '../Component/header-web/header-web';
import { Auth } from '../../services/auth';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { FooterWeb } from '../Component/footer-web/footer-web';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
  message: string;
}

@Component({
  selector: 'app-trang-cv-cua-nguoi-tim-viec',
  imports: [HeaderWEB, CommonModule, RouterModule, FooterWeb],
  templateUrl: './trang-cv-cua-nguoi-tim-viec.html',
  styleUrl: './trang-cv-cua-nguoi-tim-viec.css'
})
export class TrangCvCuaNguoiTimViec implements OnInit {
  danh_sach_cv_nguoi_tim_viec: any[] = [];
  ngOnInit(): void {
    this.layDanhSachCVOnlineNguoiTimViec();
  }
  constructor(private auth: Auth, private httpclient: HttpClient, private cd: ChangeDetectorRef) { }
  layDanhSachCVOnlineNguoiTimViec() {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachCVOnlineNguoiTimViec', ma_nguoi_tim_viec,
      { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv_nguoi_tim_viec = data.danh_sach;
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
  xemCvPdf(cv: any) {
    if (cv.duong_dan_file_pdf) {
      const url = cv.duong_dan_file_pdf.startsWith('http')
        ? cv.duong_dan_file_pdf
        : `http://localhost:7000/${cv.duong_dan_file_pdf.replace(/^\/+/, '')}`;
      window.open(url, '_blank');
    }
  }

  taiCvPdf(cv: any) {
    if (cv.duong_dan_file_pdf) {
      const url = cv.duong_dan_file_pdf.startsWith('http')
        ? cv.duong_dan_file_pdf
        : `http://localhost:7000/${cv.duong_dan_file_pdf.replace(/^\/+/, '')}`;
      this.httpclient.get(url, { responseType: 'blob' }).subscribe({
        next: (data) => {
          const blob = new Blob([data], { type: 'application/pdf' });
          const link = document.createElement('a');
          link.href = window.URL.createObjectURL(blob);
          link.download = url;
          link.click();
        },
        error: (err) => {
          console.error('Lỗi tải CV:', err);
        }
      });
    }

  }

  xoaCvPdf(cv: any, index: number) {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.ma_nguoi_tim_viec;
    const thong_tin = {
      ma_cv: cv.ma_cv,
      ma_nguoi_tim_viec: ma_nguoi_tim_viec,
      duong_dan_file_pdf: cv.duong_dan_file_pdf
    };
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaCVNguoiTimViec', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv_nguoi_tim_viec.splice(index, 1);
            this.cd.detectChanges();
          }
          else {
            alert("loi xoa cv");
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
  }

}
