import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { Router } from '@angular/router';
import { inject } from '@angular/core';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

interface phuc_loi_cong_ty {
  ma_phuc_loi_cty: number;
  ma_cong_ty: number;
  ten_phuc_loi?: string;
  mo_ta?: string;
}

interface gia_tri_cot_loi_cong_ty {
  ma_gia_tri: number;
  ma_cong_ty: number;
  tieu_de?: string;
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
  loai_hinh?: number
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
  ma_bai_dang: number
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
  gia_tri_cot_loi_cong_ty?: gia_tri_cot_loi_cong_ty[];
  jobs?: viec_lam[];
  socialLinks?: { [key: string]: string };
}

@Component({
  selector: 'app-trang-gioi-thieu-cua-cong-ty',
  imports: [DatePipe, CommonModule, FormsModule],
  templateUrl: './trang-gioi-thieu-cua-cong-ty.html',
  styleUrls: ['./trang-gioi-thieu-cua-cong-ty.css']
})
export class TrangGioiThieuCuaCongTy implements OnInit {
  dang_tai = false;
  tab_dang_hoat_dong: 'overview' | 'jobs' | 'culture' = 'overview';
  phan_dang_chinh_sua: string | null = null;
  phan_dang_chinh_sua_van_hoa: string | null = null;
  la_chu_cong_ty = true;
  dang_theo_doi = false;
  mo_ta_van_hoa_cong_ty: string = "";
  ma_cong_ty: number = 0;
  danh_sach_viec_lam_noi_bat_cua_cong_ty: any;

  danh_sach_viec_lam: any;

  cong_ty: cong_ty = {
    ma_cong_ty: 0,
    ten_cong_ty: '',
    mo_ta: '',
    logo: '',
    anh_bia: '',
    phuc_loi_cong_ty: [],
    lien_ket_mang_xa_hoi: [],
    gia_tri_cot_loi_cong_ty: [],
    jobs: [],
    socialLinks: {}
  };

  constructor(private cd: ChangeDetectorRef, private httpclient: HttpClient, private auth: Auth, private route: ActivatedRoute, private sanitizer: DomSanitizer) { }
  private router = inject(Router);

  ngOnInit(): void {
    this.taiDuLieuCongTy();
    if (!this.cong_ty) {
      this.cong_ty = {
        ma_cong_ty: 0,
        phuc_loi_cong_ty: [],
        lien_ket_mang_xa_hoi: [],
        gia_tri_cot_loi_cong_ty: [],
        jobs: [],
        socialLinks: {}
      };
    }

    if (!this.cong_ty.phuc_loi_cong_ty) this.cong_ty.phuc_loi_cong_ty = [];
    if (!this.cong_ty.lien_ket_mang_xa_hoi) this.cong_ty.lien_ket_mang_xa_hoi = [];
    if (!this.cong_ty.gia_tri_cot_loi_cong_ty) this.cong_ty.gia_tri_cot_loi_cong_ty = [];
    if (!this.cong_ty.jobs) this.cong_ty.jobs = [];
    if (!this.cong_ty.socialLinks) this.cong_ty.socialLinks = {};

  }



  layDanhSachViecLamCuaCongTy() {
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_cong_ty;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachViecLamCuaCongTy', `"${ma_cong_ty}"`, { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            const danh_sach = data.danh_sach.filter((x: any) => x.bai_dang.trang_thai === 1);
            console.log(danh_sach)
            this.cong_ty.jobs = danh_sach;
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

  layDanhSachViecLamNoiBatCuaCongTy(){
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_cong_ty;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachViecLamNoiBatCuaCongTy', `"${ma_cong_ty}"`, { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_viec_lam_noi_bat_cua_cong_ty = data.danh_sach;
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

  themGioiThieuCongTY() {
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/themGioiThieuVeCongTy', {})
  }

  taiDuLieuCongTy(): void {
    this.dang_tai = true;
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_cong_ty;
    this.httpclient.post<any>(
      'http://localhost:7000/api/API_WEB/layThongTinCongTy',
      `"${ma_cong_ty}"`,
      { headers: { "Content-Type": "application/json" } }
    ).subscribe({
      next: (data) => {
        if (data.success) {
          this.cong_ty = data.danh_sach[0];
          this.cong_ty.phuc_loi_cong_ty = data.danh_sach[0].phuc_Loi_Cong_Ty || [];
          this.cong_ty.gia_tri_cot_loi_cong_ty = data.danh_sach[0].gia_Tri_Cot_Loi_Cong_Ty || [];
          this.cong_ty.lien_ket_mang_xa_hoi = data.danh_sach[0].lien_Ket_Mang_Xa_Hoi || [];
          this.layDanhSachViecLamCuaCongTy();
          this.layDanhSachViecLamNoiBatCuaCongTy();
        } else {
          console.log('Lỗi lấy thông tin công ty');
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

  layDiaChiHinhAnhBia(url?: string): string {
    if (!url) return 'https://toomva.com/images/posts/2024/11/cac-loai-hinh-cong-ty-trong-tieng-anh.png';
    if (!url.startsWith('http')) return "http://localhost:7000/" + url;
    return "";
  }

  ban_do_loai_hinh_cong_ty: { [key: number]: string } = {
    1: "Công ty trách nhiệm hữu hạn",
    2: "Công ty cổ phần",
    3: "Doanh nghiệp tư nhân",
    4: "Công ty hợp danh"
  }

  layLoaiHinhCongTy(khoa?: number) {
    if (khoa == null) return null;
    return this.ban_do_loai_hinh_cong_ty[khoa] || null;
  }

  ban_do_loai_hinh_viec_lam: { [key: number]: string } = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Thực tập",
    4: "Tự do"
  }

  layLoaiHinhViecLam(khoa?: number) {
    if (khoa == null) return null;
    return this.ban_do_loai_hinh_viec_lam[khoa] || null;
  }

  batCheDoChinhSua(phan: string) {
    this.phan_dang_chinh_sua = phan;
  }

  huyChinhSua() {
    this.phan_dang_chinh_sua = null;
    this.taiDuLieuCongTy();
  }

  luuChinhSuaGioiThieuCongTy(ma_cong_ty: number) {
    const du_lieu = {
      ma_cong_ty: ma_cong_ty,
      mo_ta: this.cong_ty.mo_ta
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/capNhatMoTaCongTy', du_lieu)
      .subscribe({
        next: (res) => {
          if (res.success) {
            alert('Lưu thông tin thành công!');
            this.phan_dang_chinh_sua = null;
          } else {
            alert('Lưu thất bại!');
          }
        },
        error: (err) => {
          console.log(err);
          alert('Có lỗi xảy ra!');
        }
      });
  }

  themPhucLoi() {
    if (!this.cong_ty.phuc_loi_cong_ty) this.cong_ty.phuc_loi_cong_ty = [];
    this.cong_ty.phuc_loi_cong_ty.push({ ma_phuc_loi_cty: 0, ma_cong_ty: this.cong_ty.ma_cong_ty, ten_phuc_loi: '', mo_ta: '' });
  }

  luuChinhSuaPhucLoi(ma_cong_ty: number) {
    const du_lieu = {
      ma_cong_ty: ma_cong_ty,
      phuc_Loi_Cong_Ty: this.cong_ty.phuc_loi_cong_ty
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/capNhatPhucLoiCongTy', du_lieu)
      .subscribe({
        next: (res) => {
          if (res.success) {
            alert('Lưu phúc lợi thành công!');
            this.phan_dang_chinh_sua = null;
            this.cd.detectChanges();
          } else {
            alert('Lưu thất bại!');
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
          alert('Có lỗi xảy ra!');
          this.cd.markForCheck();
        }
      });
  }

  xoaPhucLoi(ma_cong_ty: number, index: number, ma_phuc_loi_cty: number) {
    const du_lieu = {
      ma_cong_ty: ma_cong_ty,
      phuc_Loi_Cong_Ty: [
        { ma_phuc_loi_cty: ma_phuc_loi_cty }
      ]
    };
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/xoaPhucLoiCongTy', du_lieu)
      .subscribe({
        next: (res) => {
          if (res.success) {
            alert('Xóa phúc lợi thành công!');
            this.phan_dang_chinh_sua = null;
            this.cong_ty.phuc_loi_cong_ty?.splice(index, 1);
            this.cd.detectChanges();
          } else {
            alert('Xóa thất bại!');
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
          alert('Có lỗi xảy ra!');
          this.cd.markForCheck();
        }
      });
  }

  ///////////////////////////////////////////
  //////////////////////////////////////////
  /////////////////////////////////////////
  ////////////////////////////////////////
  //////////////////////////////////////////
  /////////////////////////////////////////
  ////////////////////////////////////////

  danh_sach_mang_xa_hoi = [
    { ten: 'facebook', bieu_tuong: 'anh_WEB/anh_icon_WEB/anh_icon_mang_xa_hoi/icon_facebook.jpg', lien_ket: '' },
    { ten: 'instagram', bieu_tuong: 'bi-instagram', lien_ket: '' },
    { ten: 'linkedin', bieu_tuong: 'bi-linkedin', lien_ket: '' },
  ];

  layBieuTuong(ten_mxh: string): SafeHtml {
    let svg = '';
    switch (ten_mxh.toLowerCase()) {
      case 'facebook':
        svg = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="#1877F2" width="24" height="24"> <path d="M22.675 0h-21.35C.594 0 0 .593 0 1.325v21.351C0 23.406.594 24 1.325 24h11.495v-9.294H9.691v-3.622h3.129V8.413c0-3.1 1.894-4.788 4.659-4.788 1.325 0 2.463.099 2.794.143v3.24l-1.918.001c-1.504 0-1.795.715-1.795 1.763v2.31h3.587l-.467 3.622h-3.12V24h6.116C23.406 24 24 23.406 24 22.676V1.325C24 .593 23.406 0 22.675 0z"/> </svg>`;
        break;
      case 'instagram':
        svg = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="#E4405F" width="24" height="24"> <path d="M12 2.163c3.204 0 3.584.012 4.85.07 1.366.062 2.633.35 3.608 1.324.975.975 1.262 2.242 1.324 3.608.058 1.266.069 1.646.069 4.85s-.012 3.584-.07 4.85c-.062 1.366-.35 2.633-1.324 3.608-.975.975-2.242 1.262-3.608 1.324-1.266.058-1.646.069-4.85.069s-3.584-.012-4.85-.07c-1.366-.062-2.633-.35-3.608-1.324-.975-.975-1.262-2.242-1.324-3.608-.058-1.266-.069-1.646-.069-4.85s.012-3.584.07-4.85c.062-1.366.35-2.633 1.324-3.608C4.517 2.513 5.784 2.225 7.15 2.163 8.416 2.105 8.796 2.163 12 2.163zm0-2.163C8.737 0 8.332.013 7.052.072 5.775.13 4.655.395 3.68 1.37c-.975.975-1.24 2.095-1.298 3.372C2.013 6.667 2 7.072 2 12s.013 5.333.072 6.613c.058 1.277.323 2.397 1.298 3.372.975.975 2.095 1.24 3.372 1.298 1.28.058 1.685.072 6.613.072s5.333-.013 6.613-.072c1.277-.058 2.397-.323 3.372-1.298.975-.975 1.24-2.095 1.298-3.372.058-1.28.072-1.685.072-6.613s-.013-5.333-.072-6.613c-.058-1.277-.323-2.397-1.298-3.372-.975-.975-2.095-1.24-3.372-1.298C17.333.013 16.928 0 12 0z"/> <circle cx="12" cy="12" r="3.6"/> </svg>`;
        break;
      case 'linkedin':
        svg = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="#0A66C2" width="24" height="24"> <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.849-3.037-1.849 0-2.132 1.445-2.132 2.939v5.667H9.356V9h3.414v1.561h.049c.476-.899 1.637-1.849 3.37-1.849 3.6 0 4.268 2.368 4.268 5.455v6.285zM5.337 7.433c-1.144 0-2.07-.927-2.07-2.07 0-1.144.926-2.07 2.07-2.07s2.07.926 2.07 2.07c0 1.143-.926 2.07-2.07 2.07zM6.897 20.452H3.777V9h3.12v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.207 24 24 23.227 24 22.271V1.729C24 .774 23.207 0 22.225 0z"/> </svg>`;
        break;
      default:
        svg = `<span>${ten_mxh}</span>`;
    }

    return this.sanitizer.bypassSecurityTrustHtml(svg);
  }

  ban_do_bieu_tuong_mang_xa_hoi: Record<string, string> = {
  facebook: 'bi-facebook',
  instagram: 'bi-instagram',
  twitter: 'bi-twitter',
  linkedin: 'bi-linkedin'
};

  danh_sach_mang_xa_hoi_co_san = [
    { ten: 'facebook', bieu_tuong: 'bi-facebook' },
    { ten: 'instagram', bieu_tuong: 'bi-instagram' },
    { ten: 'linkedin', bieu_tuong: 'bi-linkedin' },
    { ten: 'twitter', bieu_tuong: 'bi-twitter' }
  ];

  themLienKetMangXaHoi() {
    if (!this.cong_ty.lien_ket_mang_xa_hoi) this.cong_ty.lien_ket_mang_xa_hoi = [];
    this.cong_ty.lien_ket_mang_xa_hoi.push({
      ma_lien_ket: 0,
      ma_cong_ty: this.cong_ty.ma_cong_ty,
      ten_mang_xa_hoi: 'facebook',
      duong_dan: ''
    });
  }

  luuLienKetMangXaHoi() {
    this.cong_ty.lien_ket_mang_xa_hoi = this.danh_sach_mang_xa_hoi.map(s => ({
      ma_lien_ket: 0,
      ma_cong_ty: this.cong_ty.ma_cong_ty,
      ten_mang_xa_hoi: s.ten,
      duong_dan: s.lien_ket
    }));

    const du_lieu = {
      ma_cong_ty: this.cong_ty.ma_cong_ty,
      lien_Ket_Mang_Xa_Hoi: this.cong_ty.lien_ket_mang_xa_hoi
    }
    console.log(du_lieu)
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/capNhatLienKetMangXaHoi', du_lieu)
      .subscribe({
        next: (res) => {
          if (res.success) {
            alert('Lưu liên kết mạng xã hội thành công!');
            this.phan_dang_chinh_sua = null;
            this.cd.detectChanges();
          } else {
            alert('Lưu thất bại!');
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
          alert('Có lỗi xảy ra!');
          this.cd.markForCheck();
        }
      });
  }


  xoaLienKetMangXaHoi(chi_so: number) {
    this.cong_ty.lien_ket_mang_xa_hoi?.splice(chi_so, 1);
  }

  /////////////////////////////////
  /////////////////////////////////
  /////////////////////////////////
  /////////////////////////////////
  /////////////////////////////////
  /////////////////////////////////
  /////////////////////////////////

  moViecLam = (viec: viec_lam | undefined) => {
    if (!viec) return;
    const ma_bai_dang = viec.ma_bai_dang;
    this.router.navigate(['trang-chi-tiet-viec-lam'], { queryParams: { ma_bai_dang } });
  }


  khiChonAnhBia(event: any) {
    const file = event.target.files[0];
    if (file) {
      const formData = new FormData();
      formData.append('file', file);
      formData.append('ma_cong_ty', this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.ma_cong_ty);

      this.httpclient.post<any>('http://localhost:7000/api/API_WEB/capNhatAnhBiaCongTy', formData)
        .subscribe({
          next: (data) => {
            if (data.success) {
              alert("Cap nhat anh bia thanh cong");
              this.cong_ty.anh_bia = data.url;
            }
            else {
              alert(data.message);
            }
            this.cd.detectChanges();
          },
          error: (err) => {
            alert("cap nhat anh bia that bai ")
          }
        })
    }
  }

  layDiaChiHinhAnh(url?: string): string {
    if (!url) return '/assets/default-logo.png';
    if (!url.startsWith('http')) return "http://localhost:7000/" + url;
    return url;
  }
}