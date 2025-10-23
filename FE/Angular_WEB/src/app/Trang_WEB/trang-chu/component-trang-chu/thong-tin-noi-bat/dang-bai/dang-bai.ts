import { Component, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

declare var bootstrap: any;

interface API_RESPONSE {
  success: boolean;
}

@Component({
  selector: 'app-dang-bai',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './dang-bai.html',
  styleUrls: ['./dang-bai.css']
})
export class DangBai {
  constructor(
    public auth: Auth,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) { }

  @ViewChild('modalDangBai') modalDangBai!: ElementRef;

  mucLuongList = [
    { value: 'duoi_7', label: 'Dưới 7 triệu' },
    { value: '7_10', label: '7 – 10 triệu' },
    { value: '10_15', label: '10 – 15 triệu' },
    { value: '15_20', label: '15 – 20 triệu' },
    { value: '20_30', label: '20 – 30 triệu' },
    { value: 'tren_30', label: 'Trên 30 triệu' },
    { value: 'thoathuan', label: 'Thỏa thuận' }
  ];
  muc_luong: string | null = null;
  muc_luong_label: string | null = null;

  nganhNgheList = [
    { value: 'cong_nghe_thong_tin', label: 'Công nghệ thông tin' },
    { value: 'tai_chinh', label: 'Tài chính - Ngân hàng - Kế toán' },
    { value: 'marketing', label: 'Marketing' },
    { value: 'ban_hang', label: 'Bán hàng' },
    { value: 'san_xuat', label: 'Sản xuất - Công nghiệp' },
    { value: 'xay_dung', label: 'Xây dựng - Kiến trúc' },
    { value: 'giao_duc', label: 'Giáo dục - Đào tạo' },
    { value: 'y_te', label: 'Y tế - Dược' },
    { value: 'hanh_chinh', label: 'Hành chính - Văn phòng' },
    { value: 'nhan_su', label: 'Nhân sự' },
    { value: 'luat', label: 'Pháp lý - Luật' },
    { value: 'du_lich', label: 'Du lịch - Nhà hàng - Khách sạn' },
    { value: 'bat_dong_san', label: 'Bất động sản' },
    { value: 'van_tai', label: 'Vận tải - Kho vận - Logistics' },
    { value: 'truyen_thong', label: 'Truyền thông - Quảng cáo' },
    { value: 'thiet_ke', label: 'Thiết kế - Mỹ thuật - Sáng tạo' },
    { value: 'nong_lam_ngu_nghiep', label: 'Nông - Lâm - Ngư nghiệp' },
    { value: 'co_khi_dien_dien_tu', label: 'Cơ khí - Điện - Điện tử' },
    { value: 'cong_tac_xa_hoi', label: 'Công tác xã hội - Phi lợi nhuận' },
    { value: 'khac', label: 'Khác' }
  ];
  nganh_nghe: string | null = null;
  nganh_nghe_label: string | null = null;

  kinhNghiemList = [
    { value: 'none', label: 'Không yêu cầu' },
    { value: '1nam', label: '1 năm' },
    { value: '2_3nam', label: '2 – 3 năm' },
    { value: '3_5nam', label: '3 – 5 năm' },
    { value: 'tren_5nam', label: 'Trên 5 năm' }
  ];
  kinh_nghiem: string | null = null;
  kinh_nghiem_label: string | null = null;

  loaiHinhList = [
    { value: '1', label: 'Toàn thời gian' },
    { value: '2', label: 'Bán thời gian' },
    { value: '3', label: 'Thực tập' }
  ];
  loai_hinh: string | null = null;
  loai_hinh_label: string | null = null;


  vi_tri: string = '';

  tieu_de_moi = '';
  noi_dung_moi = '';
  loai_bai = 1;
  trang_thai = 1;

  yeu_cau_moi = '';
  dia_diem_moi = '';

  pop_up_dang_bai_thanh_cong: boolean = false;
  pop_up_dang_bai_that_bai: boolean = false;

  moPopup() {
    const modal = new bootstrap.Modal(this.modalDangBai.nativeElement);
    modal.show();
  }

  themBaiDang() {
    this.thongTinBaiDang();
  }

  thongTinBaiDang() {
    const nguoiDung = this.auth.layThongTinNguoiDung();
    if (!nguoiDung) {
      alert('Bạn cần đăng nhập để đăng bài!');
      return;
    }

    const thong_Tin = {
      bai_Dang: {
        ma_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ma_nguoi_dung,
        ten_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ten_cong_ty,
        tieu_de: this.tieu_de_moi,
        noi_dung: this.noi_dung_moi,
        loai_bai: this.loai_bai,
        trang_thai: this.trang_thai,
        ngay_tao: new Date().toISOString(),
        ngay_cap_nhat: new Date().toISOString()
      },
      viec_Lam: {
        ma_cong_ty: nguoiDung.thong_tin_chi_tiet.ma_cong_ty ?? 0,
        nganh_nghe: this.nganh_nghe,
        vi_tri: this.vi_tri,
        kinh_nghiem: this.kinh_nghiem,
        tieu_de: this.tieu_de_moi,
        mo_ta: this.noi_dung_moi,
        yeu_cau: this.yeu_cau_moi,
        muc_luong: this.muc_luong,
        dia_diem: this.dia_diem_moi,
        loai_hinh: Number(this.loai_hinh) || 0
      }
    };

    this.http.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/themBaiDangMoi', thong_Tin)
      .subscribe({
        next: (data) => {
          if (data.success) {

            this.pop_up_dang_bai_thanh_cong = true;
            this.cdr.markForCheck();

            const modal = bootstrap.Modal.getInstance(this.modalDangBai.nativeElement);
            modal.hide();

            setTimeout(() => {
              this.pop_up_dang_bai_thanh_cong = false;
              this.cdr.markForCheck();
              setTimeout(() => {
                window.location.reload();
              }, 500);
            }, 2000);

          } else {
            this.pop_up_dang_bai_that_bai = true;
            this.cdr.detectChanges();
            setTimeout(() => {
              this.pop_up_dang_bai_that_bai = false;
            }, 3000);
          }
        },
        error: (err) => {
          console.error('Lỗi khi thêm bài đăng:', err);
          this.pop_up_dang_bai_that_bai = true;
          this.cdr.detectChanges();
          setTimeout(() => {
            this.pop_up_dang_bai_that_bai = false;
            this.cdr.detectChanges();
          }, 1500);
        }
      });
  }
}
