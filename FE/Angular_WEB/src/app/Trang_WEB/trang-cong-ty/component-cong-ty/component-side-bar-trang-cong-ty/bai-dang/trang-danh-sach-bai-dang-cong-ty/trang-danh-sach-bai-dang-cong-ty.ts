import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-danh-sach-bai-dang-cong-ty',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-danh-sach-bai-dang-cong-ty.html',
  styleUrls: ['./trang-danh-sach-bai-dang-cong-ty.css']
})
export class TrangDanhSachBaiDangCongTy implements OnInit {
  danhSachBaiDang: any[] = [];
  thongTin: any = {};
  constructor(private auth: Auth) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit() {
    return this.layDanhSachBaiDangCuaCongTy();
  }
  async layDanhSachBaiDangCuaCongTy() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;
    const response = await fetch("http://localhost:65001/api/API_WEB/layBaiDangTheoIDCongTy", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ ma_cong_ty: ma_Cong_Ty })
    });
    const data = await response.json();
    if(data.success){
      this.danhSachBaiDang = data.danh_sach_bai_dang;
    }
  }
}
