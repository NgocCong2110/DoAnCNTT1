import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE
{
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-lich-su-ung-tuyen',
  imports: [CommonModule, DatePipe],
  templateUrl: './trang-lich-su-ung-tuyen.html',
  styleUrl: './trang-lich-su-ung-tuyen.css'
})
export class TrangLichSuUngTuyen implements OnInit{

  thongTin: any;

  loading = true;

  error = true;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  danh_sach_lich_su_ung_tuyen: any[] = [];

  pop_up_lay_thong_tin_that_bai = false;
  
  ngOnInit(): void {
    this.layDanhSachLichSuUngTuyen();
  }

  async layDanhSachLichSuUngTuyen(){
    this.danh_sach_lich_su_ung_tuyen = [];
    this.loading = true;
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachLichSuUngTuyen', { ma_nguoi_tim_viec: this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec })
      .subscribe({
        next: (data) => {
          this.loading = false;
          this.error = false;
          if(data.success){
            this.danh_sach_lich_su_ung_tuyen = data.danh_sach;
          }
          else{
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => {
              this.pop_up_lay_thong_tin_that_bai = false;
            },1500);
          }
          this.cd.detectChanges();
        }
      })
  }

  trangThaiUngTuyen: {[key: number]: string} = {
    1: "Đang chờ",
    2: "Chấp nhận",
    3: "Từ chối"
  }

  trangThaiMap(key: number){
    return this.trangThaiUngTuyen[key] || "Không có thông tin";
  }

  nganhNgheMapping: { [key: string]: string } = {
      cong_nghe_thong_tin: 'Công nghệ thông tin',
      cham_soc_khach_hang: 'Chăm sóc khách hàng',
      sales: 'Sales',
      tai_chinh: 'Tài chính',
      marketing: 'Marketing',
      ban_hang: 'Bán hàng',
      san_xuat: 'Sản xuất',
      giao_duc: 'Giáo dục',
      y_te: 'Y tế',
      hanh_chinh: 'Hành chính',
      xay_dung: 'Xây dựng',
      luat: 'Luật - Pháp lý',
      bat_dong_san: 'Bất động sản',
      du_lich: 'Du lịch',
      nong_nghiep: 'Nông nghiệp',
      nghe_thuat: 'Nghệ thuật',
      van_tai: 'Vận tải'
    };
    laynganhnghe(ma: string): string {
      return this.nganhNgheMapping[ma] || '';
    }
}
