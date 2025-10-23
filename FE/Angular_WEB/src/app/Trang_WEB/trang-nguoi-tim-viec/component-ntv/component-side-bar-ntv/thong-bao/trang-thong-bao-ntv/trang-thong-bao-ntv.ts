import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean,
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-thong-bao-ntv',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trang-thong-bao-ntv.html',
  styleUrls: ['./trang-thong-bao-ntv.css']
})
export class TrangThongBaoNtv implements OnInit {

  thongTin: any;
  danh_sach_thong_bao: any[] = [];
  pop_up_lay_thong_tin_that_bai = false;
  loading = true;

  chiTietThongBao: any = null;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.danhSachThongBao();
  }

  danhSachThongBao() {
    this.danh_sach_thong_bao = [];
    this.loading = true;

    const thong_tin = {
      kieu_nguoi_dung: this.auth.layThongTinNguoiDung()?.kieu_nguoi_dung || '',
      ma_nguoi_tim_viec: this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec || 0
    };

    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachThongBao', thong_tin)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
          } else {
            this.hienToastLoi();
          }
          this.cd.detectChanges();
        },
        error: () => {
          this.loading = false;
          this.hienToastLoi();
        }
      });
  }

  chonThongBao(event: any) {
    this.danh_sach_thong_bao = [];
    this.loading = true;
    const value = event.target.value;
    if (value === 'toan_Bo') {
      this.danhSachThongBao();
      return;
    }
    const value_num = Number(value);
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/chonThongBaoCoDinh', { loai_thong_bao: value_num })
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_thong_bao = data.danh_sach;
          } else {
            this.hienToastLoi();
          }
          this.cd.detectChanges();
        },
        error: () => {
          this.loading = false;
          this.hienToastLoi();
        }
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
}
