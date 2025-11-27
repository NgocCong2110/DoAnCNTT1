import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-danh-sach-thong-bao',
  imports: [CommonModule],
  templateUrl: './trang-danh-sach-thong-bao.html',
  styleUrl: './trang-danh-sach-thong-bao.css'
})
export class TrangDanhSachThongBao implements OnInit {

  constructor(public httpclient: HttpClient, public auth: Auth, public cd: ChangeDetectorRef, private router: Router) { }

  danh_sach_thong_bao: any[] = [];
  danh_sach_thong_bao_goc: any[] = [];
  loading = true;
  error = '';

  danh_sach_thong_bao_full: any[] = [];

  thongTin: any;

  chiTietThongBao: any = null;

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;
  pop_up_lay_thong_tin_that_bai = false;
  xoa_confirm_id: number | null = null;

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao() {
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const thong_tin = {
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung()?.kieu_nguoi_dung || null,
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_dung || this.auth.layThongTinNguoiDung()?.ma_quan_tri 
    }

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

  thongBaoQuanTriRieng(ma_quan_tri: number) {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/thongBaoQuanTriRieng', ma_quan_tri)
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
      ma_quan_tri: this.auth.layThongTinNguoiDung().ma_quan_tri || 0
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaThongBaoQuanTri', thong_tin)
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

  layLoaiThongBao: { [key: number]: string } = {
    1: "Toàn server",
    2: "Việc làm mới",
    3: "Thư mời phỏng vấn"
  };

  layLoaiHinh(loaiHinh: number) {
    return this.layLoaiThongBao[loaiHinh] || 'Không xác định được loại thông báo'
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danh_sach_thong_bao = this.danh_sach_thong_bao_full.slice(start, end);
  }

  chonThongBao(event: any) {
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const value = event.target.value.trim();
    const ma_quan_tri = this.auth.layThongTinNguoiDung()?.ma_quan_tri;
    let thong_bao_loc: any[] = [];  
    if (value === 'toan_Bo') {
      this.danhSachThongBao();
      return;
    }

    if (value === 'thong_bao_cua_toi') {
      this.thongBaoQuanTriRieng(ma_quan_tri);
      return;
    }
    if (value === 'toan_Server') {
      thong_bao_loc = [...this.danh_sach_thong_bao_goc.filter(tb => tb.loai_thong_bao === 1)];
    }

    else if (value === 'viec_Lam_Moi') {
      thong_bao_loc = [...this.danh_sach_thong_bao_goc.filter(tb => tb.loai_thong_bao === 2)];
    }

    else if( value === 'thong_bao_da_an'){
      this.layDanhSachThongBaoDaAn();
    }
    this.danh_sach_thong_bao = [...thong_bao_loc];
    this.cd.detectChanges();
    this.loading = false;
  }
  layDanhSachThongBaoDaAn(){
    const thong_tin = {
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().ma_quan_tri,
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung().kieu_nguoi_dung
    }
    console.log(thong_tin)
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachThongBaoDaAn', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
            console.log(this.danh_sach_thong_bao)
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

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }

  dieuHuongTrangChiTietViecLam(ma_bai_dang: number) {
    this.router.navigate(['trang-chi-tiet-viec-lam'], {
      queryParams: { ma_bai_dang }
    });
  }

  xemChiTiet(tb: any) {
    this.chiTietThongBao = tb;
  }

  hienToastLoi() {
    this.pop_up_lay_thong_tin_that_bai = true;
    setTimeout(() => {
      this.pop_up_lay_thong_tin_that_bai = false;
    }, 1500);
  }
  toggleDropdown(tb: any, event: MouseEvent) {
    event.stopPropagation();
    tb.showDropdown = !tb.showDropdown;
  }

  anThongBao(ma_thong_bao: number) {
    const thong_tin = {
      ma_thong_bao: ma_thong_bao,
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().ma_quan_tri,
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
      ma_nguoi_nhan: this.auth.layThongTinNguoiDung().ma_quan_tri,
      loai_nguoi_nhan: this.auth.layThongTinNguoiDung().kieu_nguoi_dung
    };
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

}
