import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Auth } from '../../../../../services/auth';

declare var bootstrap: any;

@Component({
  selector: 'app-dang-bai',
  imports: [FormsModule, CommonModule],
  templateUrl: './dang-bai.html',
  styleUrl: './dang-bai.css'
})
export class DangBai {
  constructor(public auth: Auth) {}

  @ViewChild('modalDangBai') modalDangBai!: ElementRef;

  tieu_de_moi = '';
  noi_dung_moi = '';
  luot_thich = 0;
  loai_bai = 'viec_Lam_Moi';
  trang_thai = 'hien_Thi';

  yeu_cau_moi = '';
  dia_diem_moi = '';
  muc_luong = '';
  nganh_nghe = '';
  vi_tri = '';
  kinh_nghiem = '';
  loai_hinh = 'full_Time';

  moPopup() {
    const modal = new bootstrap.Modal(this.modalDangBai.nativeElement);
    modal.show();
  }

  themBaiDang() {
    this.thongTinBaiDang();
  }

  async thongTinBaiDang() {
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
        luot_thich: this.luot_thich,
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

    try {
      const response = await fetch('http://localhost:65001/api/API_WEB/themBaiDangMoi', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(duLieu)
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Thêm thành công:', data);
        alert('Đăng bài thành công!');
      } else {
        console.error('Lỗi khi thêm bài đăng:', response.statusText);
        alert('Có lỗi xảy ra khi đăng bài');
      }
    } catch (error) {
      console.error('Exception khi gọi API:', error);
      alert('Không thể kết nối đến server');
    }
  }
}
