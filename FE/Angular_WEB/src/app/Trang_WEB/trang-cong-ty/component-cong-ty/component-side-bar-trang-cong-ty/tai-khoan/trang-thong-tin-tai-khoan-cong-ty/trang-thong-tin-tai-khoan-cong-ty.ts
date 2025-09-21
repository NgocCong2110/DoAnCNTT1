import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan-cong-ty',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-tin-tai-khoan-cong-ty.html',
  styleUrls: ['./trang-thong-tin-tai-khoan-cong-ty.css']
})
export class TrangThongTinTaiKhoanCongTy implements OnInit {
  thongTin: any;

  formDangMo = false;
  duLieuSua: string = '';
  giaTriMoi: any = '';
  giaTriCu: any = '';

  constructor(private auth: Auth) { }

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thongTin = duLieu?.thong_tin_chi_tiet || {};
  }

  moForm(field: string) {
    this.duLieuSua = field;
    this.giaTriMoi = this.thongTin?.cong_ty[field] || '';
    this.giaTriCu = this.giaTriMoi;
    this.formDangMo = true;
  }

  dongForm() {
    this.formDangMo = false;
    this.duLieuSua = '';
    this.giaTriMoi = '';
    this.giaTriCu = '';
  }

  async luuForm() {
    if (!this.thongTin || !this.thongTin.cong_ty) return;

    this.thongTin.cong_ty[this.duLieuSua] = this.giaTriMoi;

    const payload = {
      ma_cong_ty: this.thongTin.cong_ty.ma_cong_ty,
      [this.duLieuSua]: this.giaTriMoi
    };

    try {
      const response = await fetch("http://localhost:65001/api/API_WEB/capNhatThongTinCongTy", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
      });

      const data = await response.json();

      if (data.success) {
        alert(`Cập nhật ${this.duLieuSua} thành công!`);
      } else {
        this.thongTin.cong_ty[this.duLieuSua] = this.giaTriCu;
        alert(`Cập nhật ${this.duLieuSua} thất bại. Vui lòng thử lại.`);
      }
    } catch (err) {
      console.error("Lỗi kết nối:", err);
      this.thongTin.cong_ty[this.duLieuSua] = this.giaTriCu;
      alert("Không kết nối được server!");
    }

    this.dongForm();
  }
}
