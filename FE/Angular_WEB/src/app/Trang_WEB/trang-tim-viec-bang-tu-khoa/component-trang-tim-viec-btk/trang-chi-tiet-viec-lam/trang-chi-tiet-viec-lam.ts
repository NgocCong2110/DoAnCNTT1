import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../../Component/header-web/header-web';
import { ChangeDetectorRef } from '@angular/core';
import { Auth } from '../../../../services/auth';

interface API_RESPONSE {
  success: boolean;
  chi_tiet: any; 
}

@Component({
  selector: 'app-trang-chi-tiet-viec-lam',
  imports: [CommonModule, HeaderWEB],
  templateUrl: './trang-chi-tiet-viec-lam.html',
  styleUrls: ['./trang-chi-tiet-viec-lam.css'] 
})
export class TrangChiTietViecLam {
  chi_tiet: any;

  ma_bai_dang = 0;

  thongTin: any;

  constructor(
    private route: ActivatedRoute,
    private httpclient: HttpClient,
    private cd: ChangeDetectorRef,
    public auth: Auth
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit() {
    this.ma_bai_dang = Number(this.route.snapshot.paramMap.get('ma_bai_dang'));
    this.layChiTiet(this.ma_bai_dang);
  }

  layChiTiet(ma_bai_dang: number) {
    this.httpclient
      .post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layChiTietViecLam', {ma_bai_dang})
      .subscribe({
        next: (data) => {
          this.chi_tiet = data.chi_tiet;  
          this.cd.detectChanges();
        },
        error: (err) => {
          console.error(err);
        },
      });
  }

  ungTuyenCongViec() {
    const ma_Nguoi_Ung_Tuyen = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;

    const thong_Tin = {
      ma_viec: this.chi_tiet?.viec_lam?.ma_viec,
      ma_cong_ty: this.chi_tiet?.ma_nguoi_dang,
      ma_nguoi_tim_viec: ma_Nguoi_Ung_Tuyen,
    }

    console.log(thong_Tin);

    this.httpclient.post<any>("http://localhost:65001/api/API_WEB/ungTuyenCongViec", thong_Tin).subscribe({
      next: (data) => {
        if (data.success) {
          alert("Ứng tuyển thành công.");
          this.cd.detectChanges();
          setTimeout(() => {
            this.cd.detectChanges();
          }, 2000);
        }
      },
      error: () => {
        alert("Ứng tuyển thất bại.");
      }
    });
  }

  loaiHinhMap: {[key:number]: string} = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Làm việc tự do"
  }

  layLoaiHinh(key: number){
    return this.loaiHinhMap[key] || 'Không xác định';
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