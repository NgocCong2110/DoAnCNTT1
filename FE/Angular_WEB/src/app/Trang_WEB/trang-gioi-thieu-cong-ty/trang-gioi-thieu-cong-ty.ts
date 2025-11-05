import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { HeaderWEB } from '../Component/header-web/header-web';

interface phuc_loi_cong_ty {
  ma_phuc_loi_cty: number;
  ma_cong_ty: number;
  ten_phuc_loi?: string;
  mo_ta?: string;
}

interface lien_ket_mang_xa_hoi {
  ma_lien_ket: number;
  ma_cong_ty: number;
  ten_mang_xa_hoi?: string;
  duong_dan?: string;
}

interface viec_lam {
  ma_viec: number;
  ma_cong_ty: number;
  nganh_nghe?: string;
  vi_tri?: string;
  kinh_nghiem?: string;
  tieu_de?: string;
  mo_ta?: string;
  yeu_cau?: string;
  loai_hinh?: number;
  muc_luong?: string;
  muc_luong_thap_nhat?: number;
  muc_luong_cao_nhat?: number;
  quyen_loi_cong_viec?: string;
  trinh_do_hoc_van_yeu_cau?: string;
  thoi_gian_lam_viec?: string;
  dia_diem?: string;
  thoi_han_nop_cv?: string;
  ngay_tao?: string;
  ngay_cap_nhat?: string;
  ma_bai_dang: number;
}

interface cong_ty {
  ma_cong_ty: number;
  ten_cong_ty?: string;
  mo_ta?: string;
  logo?: string;
  anh_bia?: string;
  loai_hinh_cong_ty?: number;
  dia_chi?: string;
  quy_mo?: string;
  nam_thanh_lap?: number;
  email?: string;
  dien_thoai?: string;
  website?: string;
  ma_so_thue?: string;
  phuc_loi_cong_ty?: phuc_loi_cong_ty[];
  lien_ket_mang_xa_hoi?: lien_ket_mang_xa_hoi[];
  jobs?: viec_lam[];
}

@Component({
  selector: 'app-trang-gioi-thieu-cong-ty',
  imports: [DatePipe, CommonModule, HeaderWEB],
  templateUrl: './trang-gioi-thieu-cong-ty.html',
  styleUrl: './trang-gioi-thieu-cong-ty.css'
})
export class TrangGioiThieuCongTy implements OnInit {
  dang_tai = false;
  tab_dang_hoat_dong: string = 'overview';
  ma_cong_ty: number = 0;

  cong_ty: cong_ty = {
    ma_cong_ty: 0,
    ten_cong_ty: '',
    mo_ta: '',
    logo: '',
    anh_bia: '',
    phuc_loi_cong_ty: [],
    lien_ket_mang_xa_hoi: [],
    jobs: []
  };

  danh_sach_mang_xa_hoi: any[] = [];

  constructor(
    private cd: ChangeDetectorRef,
    private httpclient: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.ma_cong_ty = params['ma_cong_ty'];
      if (this.ma_cong_ty) {
        this.taiDuLieuCongTy();
      }
    });
  }

  taiDuLieuCongTy(): void {
    this.dang_tai = true;
    this.httpclient.post<any>(
      'http://localhost:65001/api/API_WEB/layThongTinCongTy',
      `"${this.ma_cong_ty}"`,
      { headers: { "Content-Type": "application/json" } }
    ).subscribe({
      next: (data) => {
        if (data.success) {
          this.cong_ty = data.danh_sach[0];
          this.cong_ty.phuc_loi_cong_ty = data.danh_sach[0].phuc_Loi_Cong_Ty || [];
          this.cong_ty.lien_ket_mang_xa_hoi = data.danh_sach[0].lien_Ket_Mang_Xa_Hoi || [];
          this.cong_ty.jobs = data.danh_sach[0].jobs || [];
          this.layDanhSachViecLamCuaCongTy();
          this.mapSocialLinks();
        }
        this.dang_tai = false;
        this.cd.markForCheck();
      },
      error: (err) => {
        console.log('Lỗi API', err);
        this.dang_tai = false;
        this.cd.markForCheck();
      }
    });
  }

  mapSocialLinks(): void {
    const ban_do_bieu_tuong: any = {
      facebook: 'bi-facebook',
      instagram: 'bi-instagram',
      twitter: 'bi-twitter',
      linkedin: 'bi-linkedin',
      zalo: 'bi-chat-dots'
    };

    this.danh_sach_mang_xa_hoi = this.cong_ty.lien_ket_mang_xa_hoi?.map(link => ({
      ten: link.ten_mang_xa_hoi,
      bieu_tuong: ban_do_bieu_tuong[link.ten_mang_xa_hoi || ''] || 'bi-link',
      lien_ket: link.duong_dan
    })) || [];

    if (this.cong_ty.dien_thoai) {
      this.danh_sach_mang_xa_hoi.push({
        ten: 'phone',
        bieu_tuong: 'bi-telephone',
        lien_ket: this.cong_ty.dien_thoai
      });
    }
  }

  layDiaChiHinhAnhBia(url?: string): string {
    if (!url) return 'https://toomva.com/images/posts/2024/11/cac-loai-hinh-cong-ty-trong-tieng-anh.png';
    if (!url.startsWith('http')) return "http://localhost:65001/" + url;
    return url;
  }

  layDiaChiHinhAnh(url?: string): string {
    if (!url) return '/assets/default-logo.png';
    if (!url.startsWith('http')) return "http://localhost:65001/" + url;
    return url;
  }

  ban_do_loai_hinh_cong_ty: { [key: number]: string } = {
    1: "Công ty trách nhiệm hữu hạn",
    2: "Công ty cổ phần",
    3: "Doanh nghiệp tư nhân",
    4: "Công ty hợp danh"
  };

  layLoaiHinhCongTy(khoa?: number) {
    if (khoa == null) return null;
    return this.ban_do_loai_hinh_cong_ty[khoa] || null;
  }

  ban_do_loai_hinh_viec_lam: { [key: number]: string } = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Thực tập",
    4: "Tự do"
  };

  layLoaiHinhViecLam(khoa?: number) {
    if (khoa == null) return null;
    return this.ban_do_loai_hinh_viec_lam[khoa] || null;
  }

  moViecLam = (viec: viec_lam | undefined) => {
    if (!viec) return;
    const ma_bai_dang = viec.ma_bai_dang;
    this.router.navigate(['trang-chi-tiet-viec-lam'], { queryParams: { ma_bai_dang } });
  }
  layDanhSachViecLamCuaCongTy() {
    this.httpclient.post<any>('http://localhost:65001/api/API_WEB/layDanhSachViecLamCuaCongTy', `"${this.ma_cong_ty}"`, { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.cong_ty.jobs = data.danh_sach;
            this.cd.detectChanges();
          }
          else {
            console.log("loi");
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log("loi");
          this.cd.markForCheck();
        }
      })
  }
}