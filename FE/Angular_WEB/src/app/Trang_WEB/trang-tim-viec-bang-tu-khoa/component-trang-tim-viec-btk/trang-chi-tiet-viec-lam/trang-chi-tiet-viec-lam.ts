import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../../Component/header-web/header-web';
import { ChangeDetectorRef } from '@angular/core';
import { Auth } from '../../../../services/auth';
import { Router } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  chi_tiet: any; 
  danh_sach: any;
}

@Component({
  selector: 'app-trang-chi-tiet-viec-lam',
  imports: [CommonModule, HeaderWEB],
  templateUrl: './trang-chi-tiet-viec-lam.html',
  styleUrls: ['./trang-chi-tiet-viec-lam.css'] 
})
export class TrangChiTietViecLam {
  chi_tiet: any;

  ma_bai_dang_chi_tiet = 0;

  danh_sach_viec_lam_cung_cong_ty: any;

  thongTin: any;

  constructor(
    private route: ActivatedRoute,
    private httpclient: HttpClient,
    private cd: ChangeDetectorRef,
    private router: Router,
    public auth: Auth
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      const ma_bai_dang = params["ma_bai_dang"]
      this.ma_bai_dang_chi_tiet = ma_bai_dang;
      this.layChiTiet(ma_bai_dang);
    })
  }

  layChiTiet(ma_bai_dang: any) {
    this.httpclient
      .post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layChiTietViecLam', {ma_bai_dang})
      .subscribe({
        next: (data) => {
          this.chi_tiet = data.chi_tiet;  
          this.cd.detectChanges();
          this.layViecLamLienQuan();
        },
        error: (err) => {
          console.error(err);
        },
      });
  }

  layViecLamLienQuan(){
    const thong_tin = {
      ma_bai_dang: this.ma_bai_dang_chi_tiet,
      ma_nguoi_dang: this.chi_tiet?.ma_nguoi_dang
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachViecLamCungCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_viec_lam_cung_cong_ty = data.danh_sach;
            this.cd.detectChanges();
          }
          else{
            console.log("loi");
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  dieuHuongToiViecLam(viec: any){
    this.router.navigate(['trang-chi-tiet-viec-lam'], {queryParams: { ma_bai_dang: viec.bai_dang.ma_bai_dang }});
  }

  taoDuongDanLogo(url : string) : string {
    if(!url)
    {
      return "";
    }
    if(!url.startsWith('http')) 
    {
      return `http://localhost:65001/${url}`;
    }
    return url;
  }

  ungTuyenCongViec() {
    const ma_Nguoi_Ung_Tuyen = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;

    const thong_Tin = {
      ma_viec: this.chi_tiet?.viec_lam?.ma_viec,
      ma_cong_ty: this.chi_tiet?.ma_nguoi_dang,
      ma_nguoi_tim_viec: ma_Nguoi_Ung_Tuyen,
    }

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

  luuViecLam(){
     const ma_Nguoi_Luu = this.thongTin?.thong_tin_chi_tiet?.nguoi_tim_viec.ma_nguoi_tim_viec;
    
        if (ma_Nguoi_Luu == null) {
          alert("Vui lòng đăng nhập để lưu bài đăng.");
          return;
        }
    
        const thong_tin = {
          ma_bai_dang: this.chi_tiet?.ma_bai_dang,
          ma_nguoi_luu: ma_Nguoi_Luu
        }
    
        this.httpclient.post<API_RESPONSE>("http://localhost:65001/api/API_WEB/luuBaiDang", thong_tin).subscribe({
          next: (data) => {
            if (data.success) {
              alert("Đã lưu bài đăng.");
              this.cd.detectChanges();
            }
          },
          error: () => {
            alert("Không thể lưu bài đăng.");
          }
        });
  }
  trinhDoHocVanMap:{ [key: number]: string} = {
    1 : 'Trung học',
    2 : 'Cao đẳng',
    3 : 'Đại học',
    4 : 'Tốt nghiệp',
    5 : 'Khác',
    6 : 'Không yêu cầu'
  }

  layTrinhDoHocVan(ma: number) : string {
    return this.trinhDoHocVanMap[ma] || '';
  }

  dieuHuongToiTrangGioiThieu(ma_cong_ty: number){
    this.router.navigate(['trang-gioi-thieu-cong-ty'], {queryParams: {ma_cong_ty}})
  }

  viecLamLienQuan(){

  }
}