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

  tieu_de_moi = '';
  noi_dung_moi = '';
  loai_bai = 'viec_Lam_Moi';
  trang_thai = 'hien_Thi';

  yeu_cau_moi = '';
  dia_diem_moi = '';
  muc_luong = '';
  nganh_nghe = '';
  vi_tri = '';
  kinh_nghiem = '';
  loai_hinh = 'full_Time';

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

    const duLieu = {
      bai_Dang: {
        ma_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ma_nguoi_dung,
        ten_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ten_cong,
        tieu_de: this.tieu_de_moi,
        noi_dung: this.noi_dung_moi,
        loai_bai: this.loai_bai,
        trang_thai: this.trang_thai,
        ngay_tao: new Date().toISOString(),
        ngay_cap_nhat: new Date().toISOString()
      },
      viec_Lam: {
        ma_cong_ty: nguoiDung.ma_cong_ty ?? 0,
        nganh_nghe: this.nganh_nghe,
        vi_tri: this.vi_tri,
        kinh_nghiem: this.kinh_nghiem,
        tieu_de: this.tieu_de_moi,
        mo_ta: this.noi_dung_moi,
        yeu_cau: this.yeu_cau_moi,
        muc_luong: this.muc_luong,
        dia_diem: this.dia_diem_moi,
        loai_hinh: this.loai_hinh
      }
    };

    this.http.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/themBaiDangMoi', duLieu)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.pop_up_dang_bai_thanh_cong = true;
            this.cdr.detectChanges();

            const modal = bootstrap.Modal.getInstance(this.modalDangBai.nativeElement);
            modal?.hide();

            setTimeout(() => {
              this.pop_up_dang_bai_thanh_cong = false;
              this.cdr.detectChanges();
            }, 1500);
          } else {
            this.pop_up_dang_bai_that_bai = true;
            this.cdr.detectChanges();
            setTimeout(() => {
              this.pop_up_dang_bai_that_bai = false;
              this.cdr.detectChanges();
            }, 1500);
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
