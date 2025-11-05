import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';

interface DanhGia {
  ma_danh_gia: number;
  ten_nguoi_danh_gia: string;
  so_diem_danh_gia: number;
  noi_dung_danh_gia: string;
  trang_thai_danh_gia: number;
}

interface API_RESPONSE {
  success: boolean;
  danh_sach: DanhGia[];
  message: string;
}

@Component({
  selector: 'app-trang-duyet-danh-gia-web',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-duyet-danh-gia-web.html',
  styleUrls: ['./trang-duyet-danh-gia-web.css']
})
export class TrangDuyetDanhGiaWeb implements OnInit {
  danh_sach_danh_gia: DanhGia[] = [];
  danh_sach_danh_gia_goc: DanhGia[] = [];
  loading: boolean = true;
  error: string | null = null;

  selectedSao: number = 0;

  constructor(private httpclient: HttpClient, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.layDanhSachDanhGia();
  }

  layDanhSachDanhGia() {
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layToanBoDanhSachDanhGia', {})
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.danh_sach_danh_gia_goc = [...res.danh_sach];
            this.danh_sach_danh_gia = [...res.danh_sach];
            this.loading = false;
          } else {
            this.error = res.message || 'Lỗi khi tải dữ liệu';
            this.loading = false;
          }
        },
        error: (err) => {
          this.error = 'Lỗi server';
          this.loading = false;
        }
      });
  }

  locSoSao() {
    if (this.selectedSao === 0) {
      this.danh_sach_danh_gia = [...this.danh_sach_danh_gia_goc];
    } else {
      this.danh_sach_danh_gia = this.danh_sach_danh_gia_goc.filter(
        dg => dg.so_diem_danh_gia === this.selectedSao
      );
    }
  }

  toggleHienThi(dg: DanhGia) {
    dg.trang_thai_danh_gia = dg.trang_thai_danh_gia === 1 ? 2 : 1;
  }

  capNhatTrangThaiDanhGia(dg: DanhGia){
    dg.trang_thai_danh_gia = dg.trang_thai_danh_gia === 1 ? 2 : 1;
    const trang_thai = dg.trang_thai_danh_gia;
    const thong_tin = {
      ma_danh_gia: dg.ma_danh_gia,
      trang_thai_danh_gia: trang_thai
    }
    console.log(thong_tin)
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/capNhatTrangThaiDanhGia', thong_tin)
      .subscribe({
        next: (data) =>{
          if(data.success){
            dg.trang_thai_danh_gia = dg.trang_thai_danh_gia === 1 ? 2 : 1;
            alert('Cap nhat trang thai thanh cong');
          }
          else{
            console.log("loi");
          }
          this.cd.markForCheck();
        },
        error: (err) =>{
          console.log(err);
        }
      })
  }
}
