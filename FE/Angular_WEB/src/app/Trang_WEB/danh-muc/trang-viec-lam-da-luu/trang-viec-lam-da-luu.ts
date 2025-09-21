import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderWEB } from '../../Component/header-web/header-web';

@Component({
  selector: 'app-trang-viec-lam-da-luu',
  imports: [CommonModule, FormsModule, HeaderWEB],
  templateUrl: './trang-viec-lam-da-luu.html',
  styleUrl: './trang-viec-lam-da-luu.css'
})
export class TrangViecLamDaLuu implements OnInit {
  danh_sach_bai_dang_da_luu: any[] = [];
  thongTin: any;
  loading: boolean = false;
  error : string = '';
  
  constructor(private auth: Auth) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }
  
  ngOnInit() {
    return this.layDanhSachBaiDangDaLuu();
  }

  async layDanhSachBaiDangDaLuu() {
    const response = await fetch('http://localhost:65001/api/API_WEB/layDanhSachBaiDangDaLuu', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ ma_nguoi_luu: this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung })
    });
    const data = await response.json();
    this.danh_sach_bai_dang_da_luu = data.danh_sach;
  }
}
