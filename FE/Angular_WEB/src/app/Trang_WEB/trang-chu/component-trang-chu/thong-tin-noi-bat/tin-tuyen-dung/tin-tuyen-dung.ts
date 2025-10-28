import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../services/auth';
import { BaiDang } from '../../../../../services/bai-dang-service/bai-dang';
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
  thong_tin_bai_xoa = false;

  constructor(
    public baiDangService: BaiDang,
    public auth: Auth,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.sub = this.baiDangService.bai_dang_duoc_chon$.subscribe(bai_dang => {
      this.bai_dang_duoc_chon = bai_dang;
      this.cdr.detectChanges();
    });

    this.baiDangService.layDanhSachBaiDang().then(res => {
      const danh_sach = res.danh_sach;
      if(danh_sach.length > 0 && !this.bai_dang_duoc_chon) {
        this.baiDangService.chonBaiDang(danh_sach[0]);
        this.cdr.detectChanges();
      }
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
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

  baoCaoBaiDang() {
    const ma_Nguoi_Bao_Cao = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;

    if (ma_Nguoi_Bao_Cao == null) {
      alert("Vui lòng đăng nhập để báo cáo bài đăng.");
      return;
    }

    const thong_tin = {
      ma_bai_vi_pham: this.bai_dang_duoc_chon?.ma_bai_dang,
      ten_nguoi_dang: this.bai_dang_duoc_chon?.ten_nguoi_dang,
      tieu_de: this.bai_dang_duoc_chon?.tieu_de,
      noi_dung: this.bai_dang_duoc_chon?.noi_dung,
      ma_nguoi_bao_cao: ma_Nguoi_Bao_Cao,
      noi_dung_bao_cao: this.noi_dung_bao_cao,
      ngay_bao_cao: new Date()
    }

    this.http.post<any>("http://localhost:65001/api/API_WEB/baoCaoBaiDang", thong_tin).subscribe({
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
    const ma_Nguoi_Luu = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;

    if (ma_Nguoi_Luu == null) {
      alert("Vui lòng đăng nhập để lưu bài đăng.");
      return;
    }

    const thong_tin = {
      ma_bai_dang: this.bai_dang_duoc_chon?.ma_bai_dang,
      ma_nguoi_luu: ma_Nguoi_Luu
    }

    this.http.post<API_RESPONSE>("http://localhost:65001/api/API_WEB/luuBaiDang", thong_tin).subscribe({
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

  ungTuyenCongViec() {

    const ma_Nguoi_Ung_Tuyen = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_tim_viec;
    if (ma_Nguoi_Ung_Tuyen == null) {
      alert("Vui lòng đăng nhập để ứng tuyển công việc.");
      return;
    }
    const ktra = {
      ma_viec: this.bai_dang_duoc_chon?.viec_lam?.ma_viec,
      ma_cong_ty: this.bai_dang_duoc_chon?.ma_nguoi_dang,
      ma_nguoi_tim_viec: ma_Nguoi_Ung_Tuyen
    };

    this.http.post<any>("http://localhost:65001/api/API_WEB/kiemTraUngTuyen", ktra)
      .subscribe({
        next: (data) => {
          if (!data.success) {
            alert("Bạn đã ứng tuyển công việc này rồi.");
            return;
          } this.http.post<any>("http://localhost:65001/api/API_WEB/ungTuyenCongViec", {
            ma_viec: this.bai_dang_duoc_chon?.viec_lam?.ma_viec,
            ma_cong_ty: this.bai_dang_duoc_chon?.ma_nguoi_dang,
            ma_nguoi_tim_viec: ma_Nguoi_Ung_Tuyen,
          }).subscribe({
            next: (data) => {
              if (data.success) {
                this.pop_up_ung_tuyen = true;
                this.cdr.detectChanges();
                setTimeout(() => {
                  this.pop_up_ung_tuyen = false;
                  this.cdr.detectChanges();
                }, 2000);
              }
            },
            error: () => {
              alert("Ứng tuyển thất bại.");
            }
          });
        }
      })
  }

  moPopUpXoa() {
    this.pop_up_xoa_bai = true;
    this.cdr.detectChanges();
  }

  huyXoa() {
    this.pop_up_xoa_bai = false;
    this.cdr.detectChanges();
  }

  xacNhanXoa() {
    if (!this.bai_dang_duoc_chon?.ma_bai_dang) return;

    this.baiDangService.xoaBaiDang(this.bai_dang_duoc_chon.ma_bai_dang)
      .then((data) => {
        if (data?.success) {
          this.thong_tin_bai_xoa = true;
          this.cdr.markForCheck();
          setTimeout(() => {
            this.thong_tin_bai_xoa = false;
            this.baiDangService.chonBaiDang(null);
            this.cdr.markForCheck();
            setTimeout(() => {
              window.location.reload();
            }, 500);
          }, 2000);
        } else {
          alert("Xoá bài đăng thất bại.");
        }
      })
      .catch(() => {
        alert("Có lỗi xảy ra khi xoá.");
      });
  }

  xoaBaiDang() {
    alert(this.bai_dang_duoc_chon?.ma_bai_dang);
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
}
