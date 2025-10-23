import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Auth } from '../../../../../../services/auth';
import { DecimalPipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE{
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
export class TrangDanhSachGoiDichVuCongTy implements OnInit{

  danh_sach_dich_vu: any[] = [];
  trangThai = false;
  thongTin: any = null;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.layDanhSachDichVu();
  }

  layDanhSachDichVu(){
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachDichVu',  {}) // gui string, int,... khong boc
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_dich_vu = [];
            this.danh_sach_dich_vu = data.danh_sach;
            this.cd.detectChanges();
          }
          else{
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

  chonDichVu(dv: any){
    const thong_tin_don_hang = {
      ma_cong_ty: this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty,
      ma_dich_vu: dv.ma_dich_vu
    }
     this.httpclient.post<any>('http://localhost:65001/api/API_Ngan_Hang/thanhToanVNPAY', thong_tin_don_hang)
    .subscribe({
      next: (res) => {
        if (res.success && res.urlThanhToan) {
          window.location.href = res.urlThanhToan;
        } else {
          alert("Không tạo được đơn hàng để thanh toán.");
        }
      },
      error: (err) => {
        console.error("Lỗi thanh toán:", err);
        alert("Có lỗi xảy ra khi gọi API thanh toán.");
      }
    });
  }
}
