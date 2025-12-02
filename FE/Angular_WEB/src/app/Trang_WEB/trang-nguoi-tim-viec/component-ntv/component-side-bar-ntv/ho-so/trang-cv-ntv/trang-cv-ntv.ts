import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { OnInit } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgModel } from '@angular/forms';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

interface CvForm {
  hoTen: string;
  email: string;
  dienThoai: string;
  ngay_sinh: string;
  dia_chi: string;
  truong_hoc: string;
  chuyen_nganh: string;
  kinh_nghiem: string;
  kyNang: string;
  duAn: string;
  mucTieu: string;
}

@Component({
  selector: 'app-trang-cv-ntv',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './trang-cv-ntv.html',
  styleUrls: ['./trang-cv-ntv.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrangCvNtv implements OnInit {
  uploadedFileName: string = '';
  dang_tai_file: File | null = null;
  maucv: any;

  lua_chon_cv: any = null;

  ngOnInit() {
    this.layDanhSachCVOnlineNguoiTimViec();
  }

  thongTin: any;

  ma_nguoi_dung = 1;
  danh_sach_cv: any[] = [];

  blocks: string[] = [];


  constructor(public auth: Auth, private httpclient: HttpClient, private cdr: ChangeDetectorRef, private sanitizer: DomSanitizer, private router: Router) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  layDanhSachCVOnlineNguoiTimViec() {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachCVOnlineNguoiTimViec', ma_nguoi_tim_viec,
      { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv = data.danh_sach;
            this.cdr.detectChanges();
          }
          else {
            console.log("loi")
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
  }

  xemCV(cv: any) {
    this.lua_chon_cv = "http://localhost:7000/" + cv.duong_dan_file_pdf;
    window.open(this.lua_chon_cv);
  }
  xoaCvPdf(cv: any, index: number) {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.ma_nguoi_tim_viec;
    const thong_tin = {
      ma_cv: cv.ma_cv,
      ma_nguoi_tim_viec: ma_nguoi_tim_viec,
      duong_dan_file_pdf: cv.duong_dan_file_pdf
    };
    const check = {
      ma_cv: cv.ma_cv,
      ma_nguoi_tim_viec: ma_nguoi_tim_viec,
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/kiemTraCVUngTuyen', check)
      .subscribe({
        next: (data) => {
          if (data.success) {
            alert("CV này của bạn đang được sử dụng để ứng tuyển");
            return;
          }
          this.cdr.markForCheck();
        }
      });
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaCVNguoiTimViec', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv.splice(index, 1);
            this.cdr.detectChanges();
          }
          else {
            alert("loi xoa cv");
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
  }
}
