import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
  danh_sach_bai_dang: any[];
}

@Component({
  selector: 'app-trang-danh-sach-bai-dang-cong-ty',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-danh-sach-bai-dang-cong-ty.html',
  styleUrls: ['./trang-danh-sach-bai-dang-cong-ty.css']
})
export class TrangDanhSachBaiDangCongTy implements OnInit {
  danhSachBaiDang: any[] = [];
  thongTin: any = {};

  constructor(
    private auth: Auth,
    private httpclient: HttpClient,
    private cd: ChangeDetectorRef
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit() {
    this.layDanhSachBaiDangCuaCongTy();
  }

  layDanhSachBaiDangCuaCongTy() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;

    this.httpclient
      .post<API_RESPONSE>(
        'http://localhost:65001/api/API_WEB/layBaiDangTheoIDCongTy',
        { ma_cong_ty: ma_Cong_Ty }
      )
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danhSachBaiDang = data.danh_sach_bai_dang;
            this.cd.detectChanges(); 
          }
        },
        error: (err) => {
          console.error('Lỗi khi gọi API:', err);
        }
      });
  }
}
