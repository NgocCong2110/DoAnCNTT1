import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../services/auth';
import { Subscription } from 'rxjs';
import { BaiDangComponent } from '../bai-dang.model';
import { HttpClient } from '@angular/common/http';
import { error } from 'node:console';

interface API_RESPONSE {
  success: boolean
}

@Component({
  selector: 'app-tin-tuyen-dung',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tin-tuyen-dung.html',
  styleUrls: ['./tin-tuyen-dung.css']
})
export class TinTuyenDung implements OnInit, OnDestroy {
  bai_dang_duoc_chon: BaiDangComponent | null = null;
  private sub?: Subscription;
  pop_up_bao_cao: boolean = false;
  noi_dung_bao_cao: string = "";
  thongTin: any;
  pop_up_ung_tuyen = false;
  pop_up_xoa_bai = false;
  pop_up_chon_cv = false;
  thong_tin_bai_xoa = false;
  danh_sach_cv: any;
  cv_duoc_chon: number | null = null;
  loai_cv: string = '';
  file_cv_upload: File | null = null;
  danh_sach_bai_dang: any;
  danh_sach_bai_dang_full: any;

  loai_vi_pham = '';

  danh_sach_loai = [
    'Thông tin sai sự thật',
    'Tuyển dụng lừa đảo / thu phí',
    'Sai ngành nghề',
    'Vi phạm bản quyền / hình ảnh',
    'Liên kết độc hại / mã độc',
    'Phân biệt đối xử',
    'Yêu cầu không hợp pháp',
    'Thông tin thiếu minh bạch',
    'Khác'
  ];

  error = '';
  loading = false;

  trang_hien_tai = 1;
  so_luong_moi_trang = 8;
  tong_trang = 1;

  constructor(
    public auth: Auth,
    private httpclient: HttpClient,
    private cdr: ChangeDetectorRef
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    const mang = this.layDanhSachBaiDang();
  }

  layDanhSachBaiDang() {
    this.loading = true;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachBaiDang', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_bai_dang_full = data.danh_sach;
            this.tong_trang = Math.ceil(this.danh_sach_bai_dang_full.length / this.so_luong_moi_trang);
            this.loadTrang(1);
            this.bai_dang_duoc_chon = this.danh_sach_bai_dang[0] || null;
          } else {
            this.error = 'Không lấy được danh sách bài đăng.';
          }
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.error = 'Lỗi kết nối máy chủ.';
          this.loading = false;
          console.error(err);
          this.cdr.detectChanges();
        }
      });
  }

  loadTrang(trang: number) {
    this.trang_hien_tai = trang;
    const start = (trang - 1) * this.so_luong_moi_trang;
    const end = start + this.so_luong_moi_trang;
    this.danh_sach_bai_dang = this.danh_sach_bai_dang_full.slice(start, end);
    this.cdr.detectChanges();
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tong_trang) return;
    this.loadTrang(trang);
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.bai_dang_duoc_chon;
  }

  loaiHinhMap: { [key: number]: string } = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Thực tập",
    4: "Tự Do"
  };

  layLoaiHinh(loaiHinh: number) {
    return this.loaiHinhMap[loaiHinh] || 'Không xác định';
  }

  chonFile(event: any) {
    this.file_cv_upload = event.target.files[0];
  }

  baoCaoBaiDang() {
    const ma_Nguoi_Bao_Cao = this.thongTin?.thong_tin_chi_tiet?.nguoi_tim_viec.ma_nguoi_tim_viec;

    if (ma_Nguoi_Bao_Cao == null) {
      alert("Vui lòng đăng nhập để báo cáo bài đăng.");
      return;
    }

    const thong_tin = {
      ma_bai_dang: this.bai_dang_duoc_chon?.ma_bai_dang,
      ten_nguoi_dang: this.bai_dang_duoc_chon?.ten_nguoi_dang,
      tieu_de: this.bai_dang_duoc_chon?.tieu_de,
      noi_dung: this.bai_dang_duoc_chon?.noi_dung,
      loai_vi_pham: this.loai_vi_pham,
      ma_nguoi_bao_cao: ma_Nguoi_Bao_Cao,
      noi_dung_bao_cao: this.noi_dung_bao_cao,
      trang_thai_xu_ly: 1,
      ngay_bao_cao: new Date()
    }
    
    this.httpclient.post<any>("http://localhost:7000/api/API_WEB/baoCaoBaiDang", thong_tin).subscribe({
      next: (data) => {
        if (data.success) {
          alert("Báo cáo bài đăng thành công.");
          this.pop_up_bao_cao = false;
          this.noi_dung_bao_cao = '';
        } else {
          alert("Báo cáo bài đăng thất bại. Vui lòng thử lại.");
        }
        this.cdr.detectChanges();
      },
      error: () => {
        alert("Có lỗi xảy ra khi gửi báo cáo.");
      }
    });
  }

  luuBaiDang() {
    const ma_Nguoi_Luu = this.thongTin?.thong_tin_chi_tiet?.nguoi_tim_viec.ma_nguoi_tim_viec;

    if (ma_Nguoi_Luu == null) {
      alert("Vui lòng đăng nhập để lưu bài đăng.");
      return;
    }

    const thong_tin = {
      ma_bai_dang: this.bai_dang_duoc_chon?.ma_bai_dang,
      ma_nguoi_luu: ma_Nguoi_Luu
    }

    this.httpclient.post<API_RESPONSE>("http://localhost:7000/api/API_WEB/luuBaiDang", thong_tin).subscribe({
      next: (data) => {
        if (data.success) {
          alert("Đã lưu bài đăng.");
          this.cdr.detectChanges();
        }
      },
      error: () => {
        alert("Không thể lưu bài đăng.");
      }
    });
  }

  moPopUpUngTuyen() {
    this.pop_up_chon_cv = true;
    this.cv_duoc_chon = null;

    const maNguoiTimViec = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    if (!maNguoiTimViec) return;

    this.layDanhSachCVOnlineNguoiTimViec();
  }

  canSubmit(): boolean {
    if (this.loai_cv === 'he_thong') {
      return this.cv_duoc_chon !== null;
    } else if (this.loai_cv === 'upload') {
      return !!this.file_cv_upload;
    }
    return false;
  }

  ungTuyenCongViec() {

    const maNguoiTimViec = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    if (!maNguoiTimViec) {
      alert("Vui lòng đăng nhập để ứng tuyển công việc.");
      return;
    }

    const ktra = {
      ma_viec: this.bai_dang_duoc_chon?.viec_lam?.ma_viec,
      ma_cong_ty: this.bai_dang_duoc_chon?.ma_nguoi_dang,
      ma_nguoi_tim_viec: maNguoiTimViec
    };

    this.httpclient.post<any>("http://localhost:7000/api/API_WEB/kiemTraUngTuyen", ktra)
      .subscribe({
        next: (data) => {
          if (!data.success) {
            alert("Bạn đã ứng tuyển công việc này rồi.");
            return;
          }

          const thong_tin = {
            ma_viec: this.bai_dang_duoc_chon?.viec_lam?.ma_viec,
            ma_cong_ty: this.bai_dang_duoc_chon?.ma_nguoi_dang,
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
            formData.append('ma_viec', String(this.bai_dang_duoc_chon?.viec_lam?.ma_viec) || '');
            formData.append('ma_cong_ty', String(this.bai_dang_duoc_chon?.ma_nguoi_dang) || '');
            formData.append('ma_nguoi_tim_viec', maNguoiTimViec || '');
            formData.append('duong_dan_file_cv_upload', this.file_cv_upload);

            console.log('FormData chuẩn bị gửi:', formData);

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
  autoHidePopup() {
    this.cdr.detectChanges();
    setTimeout(() => {
      this.pop_up_ung_tuyen = false;
      this.cdr.detectChanges();
    }, 2000);
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

  layDanhSachCVOnlineNguoiTimViec() {
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachCVOnlineNguoiTimViec', ma_nguoi_tim_viec,
      { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv = data.danh_sach;
            this.cdr.detectChanges();
          }
          else {
            console.log("loi")
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err)
        }
      })
  }

  huyChonCV() {
    this.pop_up_chon_cv = false;
  }

  moPopUpBaoCao() {
    this.pop_up_bao_cao = true;
    this.noi_dung_bao_cao = "";
    this.cdr.detectChanges();
  }

  huyBaoCao() {
    this.pop_up_bao_cao = false;
    this.noi_dung_bao_cao = '';
    this.cdr.detectChanges();
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

  trinhDoHocVanMap: { [key: number]: string } = {
    1: 'Trung học',
    2: 'Cao đẳng',
    3: 'Đại học',
    4: 'Tốt nghiệp',
    5: 'Khác',
    6: 'Không yêu cầu'
  }

  layTrinhDoHocVan(ma: number): string {
    return this.trinhDoHocVanMap[ma] || '';
  }
}
