import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-trang-danh-muc-nganh-nghe',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-danh-muc-nganh-nghe.html',
  styleUrls: ['./trang-danh-muc-nganh-nghe.css']
})
export class TrangDanhMucNganhNghe implements OnInit {
  danh_sach_nganh_nghe: any[] = [];

  maNganhNghe = '';
  tenNganhNghe = '';
  thuTu: number | null = null;

  constructor(private cd: ChangeDetectorRef, private httpclient: HttpClient) { }

  ngOnInit(): void {
    this.layDanhsachNganhNghe();
  }

  layDanhsachNganhNghe() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachNganhNghe', {})
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.danh_sach_nganh_nghe = [...res.danh_sach];
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi khi lấy danh sách ngành nghề', err);
        }
      });
  }

  themNganhNghe() {
    if (this.maNganhNghe && this.tenNganhNghe && this.thuTu != null) {

      const thong_tin = {
        ma_nganh_nghe: this.maNganhNghe,
        ten_nganh_nghe: this.tenNganhNghe,
        thu_tu: this.thuTu
      };

      this.danh_sach_nganh_nghe.unshift({ ...thong_tin });
      this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/themNganhNgheMoi', thong_tin)
        .subscribe({
          next: (res) => {
            if (res.success) {
              this.cd.detectChanges();
            }
            this.cd.markForCheck();
          },
          error: (err) => {
            console.error('Lỗi khi thêm danh sách ngành nghề', err);
          }
        });

      this.maNganhNghe = '';
      this.tenNganhNghe = '';
      this.thuTu = null;
    }
  }

  xoaNganhNghe(index: number) {
    const nganh = this.danh_sach_nganh_nghe[index];
    const thong_tin = {
      ma_nganh_nghe: nganh.ma_nganh_nghe,
      ten_nganh_nghe: nganh.ten_nganh_nghe,
      thu_tu: nganh.thu_tu
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaNganhNghe', thong_tin)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.danh_sach_nganh_nghe.splice(index, 1);
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi khi xóa ngành nghề', err);
        }
      });
  }

}
