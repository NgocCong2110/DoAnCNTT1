import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Auth } from '../../../../../../services/auth';
import { DecimalPipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
  url_Thanh_Toan: any;
}

@Component({
  selector: 'app-trang-danh-sach-goi-dich-vu-cong-ty',
  imports: [DecimalPipe, CommonModule, FormsModule],
  templateUrl: './trang-danh-sach-goi-dich-vu-cong-ty.html',
  styleUrl: './trang-danh-sach-goi-dich-vu-cong-ty.css'
})
export class TrangDanhSachGoiDichVuCongTy implements OnInit {

  danh_sach_dich_vu: any[] = [];
  trangThai = false;
  thongTin: any = null;
  qrCodeUrl: string | null = null;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.layDanhSachDichVu();
  }

  layDanhSachDichVu() {
    const thong_tin = {
      ma_cong_ty: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.ma_cong_ty
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachDichVu', thong_tin) // gui string, int,... khong boc
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_dich_vu = [];
            this.danh_sach_dich_vu = data.danh_sach;
            this.cd.detectChanges();
          }
          else {
            console.log("loi dich vu")
          }
        },
        error: (err) => {
          this.danh_sach_dich_vu = [];
          console.log(err)
          this.cd.detectChanges();
        }
      })
  }

  hienThiTrangThaiThanhToan(n: number): string {
    if (n == 1) {
      return 'Chưa thanh toán';
    }
    else if (n == 2) {
      return 'Đã thanh toán';
    }
    return 'Không xác định';
  }

  chonDichVu(dv: any) {
    const ma_cong_ty = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.ma_cong_ty;
    const so_tien_dich_vu = dv.dich_Vu.so_tien;
    const noi_dung = dv.dich_Vu.ma_dich_vu + "/" + ma_cong_ty;
    this.kiemTraThanhToan(so_tien_dich_vu, noi_dung);
    this.qrCodeUrl = `https://img.vietqr.io/image/MB-0396814806-compact2.png?amount=${so_tien_dich_vu}&addInfo=${noi_dung}`;
  }

  kiemTraThanhToan(so_tien: number, noi_dung: string): boolean {
    const url = 'https://script.google.com/macros/s/AKfycbxWY4nHE1evpeNFvFZSqjmgbcmLdxXtCTSLbX-i_eDyAETp_V_Ra6E6LeRzwH45iyU6/exec';
    this.httpclient.get(url).subscribe({
      next: (res: any) => {
        const thong_tin = res.data;
        const giao_dich = thong_tin.find((gd: any) =>
          Number(gd["Giá trị"]) >= Number(so_tien) &&
          String(gd["Mô tả"]).includes(noi_dung)
        )
        if (giao_dich) {
          alert("Thanh toán thành công!");
          console.log("Giao dịch:", giao_dich);
        } else {
          alert("Chưa thanh toán");
        }
      },
      error: (err) => console.error(err)
    });
    return false;
  }
}
