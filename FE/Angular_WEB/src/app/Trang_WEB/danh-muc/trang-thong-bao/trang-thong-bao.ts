import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../services/auth';
import { HeaderWEB } from '../../Component/header-web/header-web';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { FooterWeb } from '../../Component/footer-web/footer-web';
import { Router, RouterModule } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-thong-bao',
  imports: [CommonModule, HeaderWEB, FooterWeb],
  templateUrl: './trang-thong-bao.html',
  styleUrl: './trang-thong-bao.css'
})
export class TrangThongBao implements OnInit {

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef, private router: Router) { }

  danh_sach_thong_bao: any[] = [];

  danh_sach_thong_bao_goc: any[] = [];

  pop_up_lay_thong_tin_that_bai = false;

  loading = true;

  error = false;

  thongTin: any;

  chiTietThongBao: any = null;

  xoa_confirm_id: number | null = null;

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao() {
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const thong_tin = {
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung()?.kieu_nguoi_dung ?? 'nguoi_Tim_Viec',
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_nguoi_dung || 0,
      ma_cong_ty: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_cong_ty || 0,
      ma_nguoi_tim_viec: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_nguoi_tim_viec || 0,
    }

    console.log(thong_tin)

    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachThongBao', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
            this.danh_sach_thong_bao_goc = [...data.danh_sach];
            this.cd.detectChanges();
          }
          else {
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            }, 1500);
          }
          this.cd.markForCheck();
        }
      });
  }

  chonThongBao(event: any) {
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const value = event.target.value.trim();
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet.ma_cong_ty;
    let thong_bao_loc: any[] = [];  
    if (value === 'toan_Bo') {
      this.danhSachThongBao();
      return;
    }

    if (value === 'thong_bao_cua_toi') {
      this.thongBaoCongTyRieng(ma_cong_ty);
      return;
    }
    if (value === 'toan_Server') {
      thong_bao_loc = [...this.danh_sach_thong_bao_goc.filter(tb => tb.loai_thong_bao === 1)];
    }

    else if (value === 'viec_Lam_Moi') {
      thong_bao_loc = [...this.danh_sach_thong_bao_goc.filter(tb => tb.loai_thong_bao === 2)];
    }
    else if (value === 'thu_Moi_Phong_Van') {
      thong_bao_loc = [...this.danh_sach_thong_bao_goc.filter(tb => tb.loai_thong_bao === 3)];
    }
    else if( value === 'thong_bao_da_an'){
      this.layDanhSachThongBaoDaAn();
    }
    this.danh_sach_thong_bao = [...thong_bao_loc];
    this.cd.detectChanges();
    this.loading = false;
  }

  thongBaoCongTyRieng(ma_cong_ty: number) {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/thongBaoCongTyRieng', ma_cong_ty)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
            this.cd.detectChanges();
          }
          else {
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            }, 1500);
          }
          this.cd.markForCheck();
        }
      });
  }

  layDanhSachThongBaoDaAn(){
    const thong_tin = {
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_nguoi_dung,
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung().kieu_nguoi_dung
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachThongBaoDaAn', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
            this.cd.detectChanges();
          }
          else {
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            }, 1500);
          }
          this.cd.markForCheck();
        }
      });
  }

  moPopUpXoaThongBao(ma_thong_bao: number) {
    this.xoa_confirm_id = ma_thong_bao;
  }

  xac_nhan_xoa() {
    if (this.xoa_confirm_id !== null) {
      this.xoaThongBao(this.xoa_confirm_id);
      this.xoa_confirm_id = null;
    }
  }

  xoaThongBao(ma_thong_bao: number) {
    const thong_tin = {
      ma_thong_bao: ma_thong_bao,
      ma_cong_ty: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_cong_ty || 0
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaThongBaoCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao.splice(this.danh_sach_thong_bao.findIndex(tb => tb.ma_thong_bao === ma_thong_bao), 1);
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        }
      });
  }

  toggleDropdown(tb: any, event: MouseEvent) {
    event.stopPropagation();
    tb.showDropdown = !tb.showDropdown;
  }

  anThongBao(ma_thong_bao: number) {
    const thong_tin = {
      ma_thong_bao: ma_thong_bao,
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_nguoi_dung,
      loai_nguoi_nhan: this.auth.layThongTinNguoiDung().kieu_nguoi_dung
    };
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/anThongBao', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao.splice(this.danh_sach_thong_bao.findIndex(tb => tb.ma_thong_bao === ma_thong_bao), 1);
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        }
      });
  }

  boAnThongBao(ma_thong_bao: number) {
    const thong_tin = {
      ma_thong_bao: ma_thong_bao,
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_nguoi_dung || 0,
      loai_nguoi_nhan: this.auth.layThongTinNguoiDung().kieu_nguoi_dung
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/boAnThongBao', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao.splice(this.danh_sach_thong_bao.findIndex(tb => tb.ma_thong_bao === ma_thong_bao), 1);
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        }
      });
  }

  xemChiTiet(tb: any) {
    this.chiTietThongBao = tb;
  }

  dieuHuongTrangChiTietViecLam(ma_bai_dang: number) {
    this.router.navigate(['trang-chi-tiet-viec-lam'], {
      queryParams: { ma_bai_dang }
    });
  }

  hienToastLoi() {
    this.pop_up_lay_thong_tin_that_bai = true;
    setTimeout(() => {
      this.pop_up_lay_thong_tin_that_bai = false;
    }, 1500);
  }
}
