import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { ChangeDetectorRef } from '@angular/core';

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
export class TrangDanhSachThongBao implements OnInit{

  constructor(public httpclient: HttpClient, public auth: Auth, public cd: ChangeDetectorRef) {}

  danh_sach_thong_bao: any[] = [];
  loading = true;
  error = '';

  danh_sach_thong_bao_full: any[] = [];

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;
  pop_up_lay_thong_tin_that_bai = false;

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao(){
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const thong_tin = {
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung()?.kieu_nguoi_dung || null,
      ma_nguoi_tim_viec: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec || null
    }
    
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachThongBao', thong_tin )
    .subscribe({
      next: (data) =>{
        this.loading = false;
        if(data.success){
          this.danh_sach_thong_bao = data.danh_sach;
        }
        else{
          this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            },1500);
        }
        this.cd.detectChanges();
      }
    });
  }

  layLoaiThongBao: {[key : number]: string} = {
    1: "Toàn server",
    2: "Việc làm mới",
    3: "Thư mời phỏng vấn"
  };

  layLoaiHinh(loaiHinh: number){
    return this.layLoaiThongBao[loaiHinh] || 'Không xác định được loại thông báo'
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danh_sach_thong_bao = this.danh_sach_thong_bao_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }
}
