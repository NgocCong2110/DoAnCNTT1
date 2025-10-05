import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan-ntv',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-tin-tai-khoan-ntv.html',
  styleUrls: ['./trang-thong-tin-tai-khoan-ntv.css']
})
export class TrangThongTinTaiKhoanNtv implements OnInit {
  thongTin: any;

  formDangMo = false;
  duLieuSua: string = '';
  giaTriMoi: any = '';
  giaTriCu: any = '';

  constructor(private auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    const duLieu = this.auth.layThongTinNguoiDung();
    this.thongTin = duLieu?.thong_tin_chi_tiet || {};
  }

  moForm(field: string) {
    this.duLieuSua = field;
    this.giaTriMoi = this.thongTin?.[field] || '';
    this.giaTriCu = this.giaTriMoi;
    this.formDangMo = true;
  }

  dongForm() {
    this.formDangMo = false;
    this.duLieuSua = '';
    this.giaTriMoi = '';
    this.giaTriCu = '';
  }

  luuForm() {
    if (!this.thongTin) return;

    // cập nhật tạm thời trên view
    this.thongTin[this.duLieuSua] = this.giaTriMoi;

    const payload = {
      ma_nguoi_tim_viec: this.thongTin.ma_nguoi_tim_viec,
      [this.duLieuSua]: this.giaTriMoi
    };

    this.httpclient.post<any>(
      "http://localhost:65001/api/API_WEB/capNhatThongTinNguoiTimViec",
      payload
    ).subscribe({
      next: (data) => {
        if (data.success) {
          alert(`Cập nhật ${this.duLieuSua} thành công!`);
        } else {
          this.thongTin[this.duLieuSua] = this.giaTriCu;
          alert(`Cập nhật ${this.duLieuSua} thất bại. Vui lòng thử lại.`);
        }
        this.cd.detectChanges(); 
        this.dongForm();
      },
      error: (err) => {
        console.error("Lỗi kết nối:", err);
        this.thongTin[this.duLieuSua] = this.giaTriCu;
        alert("Không kết nối được server!");
        this.cd.detectChanges(); // cập nhật view
        this.dongForm();
      }
    });
  }
}
