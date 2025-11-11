import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../services/auth';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-the-danh-gia-tu-nguoi-dung',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './the-danh-gia-tu-nguoi-dung.html',
  styleUrls: ['./the-danh-gia-tu-nguoi-dung.css']
})
export class TheDanhGiaTuNguoiDung implements OnInit {

  so_diem_danh_gia = 0;
  noi_dung_danh_gia = '';
  danh_sach_danh_gia: any[] = [];
  slides: any[][] = [];

  constructor(
    private auth: Auth,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.layDanhSachDanhGia();
  }

  layDanhSachDanhGia() {
    this.http.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachDanhGia', {})
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.danh_sach_danh_gia = res.danh_sach;

            this.slides = [];
            for (let i = 0; i < this.danh_sach_danh_gia.length; i += 2) {
              this.slides.push(this.danh_sach_danh_gia.slice(i, i + 2));
            }

            this.cdr.markForCheck();
          }
        },
        error: (err) => {
          console.log(err);
        }
      });
  }

  guiDanhGia(): void {
    const thong_tin = this.auth.layThongTinNguoiDung();
    if (!thong_tin) {
      alert('Vui lòng đăng nhập để đánh giá');
      return;
    }

    const thong_tin_danh_gia = {
      ma_nguoi_danh_gia: thong_tin.thong_tin_chi_tiet.ma_nguoi_dung,
      ten_nguoi_danh_gia: thong_tin.ten_dang_nhap,
      so_diem_danh_gia: this.so_diem_danh_gia,
      noi_dung_danh_gia: this.noi_dung_danh_gia
    };

    this.http.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/themDanhGia', thong_tin_danh_gia)
      .subscribe({
        next: (res) => {
          if (res.success) {
            alert('Gửi đánh giá thành công');
            this.noi_dung_danh_gia = '';
            this.so_diem_danh_gia = 0;
            this.layDanhSachDanhGia(); 
          }
        },
        error: () => {
          console.log('Lỗi gửi đánh giá');
        }
      });
  }
}
