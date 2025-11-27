import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../../Component/header-web/header-web';
import { ChangeDetectorRef } from '@angular/core';
import { Auth } from '../../../../services/auth';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE {
  success: boolean;
  chi_tiet: any; 
  danh_sach: any;
}

@Component({
  selector: 'app-trang-chi-tiet-viec-lam',
  imports: [CommonModule, HeaderWEB, FormsModule],
  templateUrl: './trang-chi-tiet-viec-lam.html',
  styleUrls: ['./trang-chi-tiet-viec-lam.css'] 
})
export class TrangChiTietViecLam {
  chi_tiet: any;

  ma_bai_dang_chi_tiet = 0;

  danh_sach_cv: any;

  danh_sach_viec_lam_cung_cong_ty: any;

  pop_up_ung_tuyen = false;

  cv_duoc_chon: number | null = null;

  loai_cv: string = '';

  file_cv_upload: File | null = null;

  pop_up_chon_cv = false;

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
      .post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layChiTietViecLam', {ma_bai_dang})
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

  chonFile(event: any) {
    this.file_cv_upload = event.target.files[0];
  }

  layViecLamLienQuan(){
    const thong_tin = {
      ma_bai_dang: this.ma_bai_dang_chi_tiet,
      ma_nguoi_dang: this.chi_tiet?.ma_nguoi_dang
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachViecLamCungCongTy', thong_tin)
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
      return `http://localhost:7000/${url}`;
    }
    return url;
  }

  moPopUpUngTuyen() {
    this.pop_up_chon_cv = true;
    this.cv_duoc_chon = null;

    const maNguoiTimViec = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    if (!maNguoiTimViec) return;

    this.layDanhSachCVOnlineNguoiTimViec();
  }

  ungTuyenCongViec() {

    const maNguoiTimViec = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    const ma_nguoi_nhan = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;
    if (!maNguoiTimViec) {
      alert("Vui lòng đăng nhập để ứng tuyển công việc.");
      return;
    }

    const ktra = {
      ma_viec: this.chi_tiet?.viec_lam?.ma_viec,
      ma_cong_ty: this.chi_tiet?.ma_nguoi_dang,
      ma_nguoi_tim_viec: maNguoiTimViec
    };

    this.httpclient.post<any>("http://localhost:7000/api/API_WEB/kiemTraUngTuyen", ktra)
      .subscribe({
        next: (data) => {
          if (data.success) {
            alert("Bạn đã ứng tuyển công việc này rồi.");
            return;
          }

          const thong_tin = {
            ma_viec: this.chi_tiet?.viec_lam?.ma_viec,
            ma_cong_ty: this.chi_tiet?.ma_nguoi_dang,
            ma_nguoi_nhan: ma_nguoi_nhan,
            ma_nguoi_tim_viec: maNguoiTimViec,
            ma_cv: this.cv_duoc_chon
          };

          if (this.loai_cv === 'he_thong' && this.cv_duoc_chon) {
            this.httpclient.post<any>("http://localhost:7000/api/API_WEB/ungTuyenCongViec", thong_tin).subscribe({
              next: (res) => {
                if (res.success) {
                  alert("Ứng tuyển thành công bằng CV hệ thống!");
                  this.pop_up_chon_cv = false;
                  this.pop_up_ung_tuyen = true;
                  this.autoHidePopup();
                }
              },
              error: () => alert("Ứng tuyển thất bại.")
            });
          }

          else if (this.loai_cv === 'upload' && this.file_cv_upload) {
            const formData = new FormData();
            formData.append('ma_viec', String(this.chi_tiet?.viec_lam?.ma_viec) || '');
            formData.append('ma_cong_ty', String(this.chi_tiet?.ma_nguoi_dang) || '');
            formData.append('ma_nguoi_tim_viec', maNguoiTimViec || '');
            formData.append('ma_nguoi_nhan', ma_nguoi_nhan || '');
            formData.append('duong_dan_file_cv_upload', this.file_cv_upload);

            this.httpclient.post<any>(
              'http://localhost:7000/api/API_WEB/ungTuyenCongViecUploadCV',
              formData
            ).subscribe({
              next: (res) => {
                if (res.success) {
                  alert("Ứng tuyển thành công với CV tải lên!");
                  this.pop_up_chon_cv = false;
                  this.pop_up_ung_tuyen = true;
                  this.autoHidePopup();
                }
              },
              error: () => alert("Ứng tuyển thất bại khi tải lên CV.")
            });
          } else {
            alert("Vui lòng chọn loại CV hoặc tải lên file.");
          }
        },
        error: () => {
          alert("Lỗi khi kiểm tra ứng tuyển.");
        }
      });
  }

  xoaFileDangChon() {
    this.file_cv_upload = null;
    const input = document.querySelector('input[type="file"]') as HTMLInputElement | null;
    if (input) input.value = '';
  }

  xemFileTamThoi() {
    if (!this.file_cv_upload) return;
    const url = "http://localhost:7000/" + this.file_cv_upload;
    window.open(url);
  }

  huyChonCV() {
    this.pop_up_chon_cv = false;
  }

  autoHidePopup() {
    this.cd.detectChanges();
    setTimeout(() => {
      this.pop_up_ung_tuyen = false;
      this.cd.detectChanges();
    }, 2000);
  }

  canSubmit(): boolean {
    if (this.loai_cv === 'he_thong') {
      return this.cv_duoc_chon !== null;
    } else if (this.loai_cv === 'upload') {
      return !!this.file_cv_upload;
    }
    return false;
  }

  layDanhSachCVOnlineNguoiTimViec() {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachCVOnlineNguoiTimViec', ma_nguoi_tim_viec,
      { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv = data.danh_sach;
            this.cd.detectChanges();
          }
          else {
            console.log("loi")
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
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
    
        this.httpclient.post<API_RESPONSE>("http://localhost:7000/api/API_WEB/luuBaiDang", thong_tin).subscribe({
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
}