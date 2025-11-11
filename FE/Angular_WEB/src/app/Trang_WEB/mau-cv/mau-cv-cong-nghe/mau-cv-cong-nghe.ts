import { Component, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../Component/header-web/header-web';
import { Auth } from '../../../services/auth';

@Component({
  selector: 'app-mau-cv-cong-nghe',
  imports: [HeaderWEB, CommonModule, FormsModule],
  templateUrl: './mau-cv-cong-nghe.html',
  styleUrl: './mau-cv-cong-nghe.css'
})
export class MauCvCongNghe {
  pop_up_nhap_ten_file = false;

  formCv = {
    ten_cv: '',
    anh_dai_dien: '',
    ho_ten: '',
    email: '',
    dien_thoai: '',
    ngay_sinh: '',
    dia_chi: '',
    chuyen_nganh: '',
    ky_nang: '',
    du_an: '',
    muc_tieu: '',
    vi_tri_ung_tuyen: '',
    gioi_tinh: '',
    hoc_van: [
      {
        thoi_gian_hoc_tap: '2019 - 2023',
        ten_truong: 'Đại học kỹ thuật công nghệ cần thơ',
        nganh_hoc: 'Công nghệ thông tin',
        mo_ta: 'Tốt nghiệp loại Giỏi, chuyên ngành Công nghệ thông tin.'
      }
    ],
    kinh_nghiem: [
      {
        thoi_gian_lam_viec: '2023 - nay',
        ten_cong_ty: 'Công ty TNHH ABC',
        vi_tri: 'Lập trình viên Frontend',
        mo_ta: 'Phát triển giao diện người dùng với Angular và TypeScript.'
      }
    ]
  };

  constructor(private http: HttpClient, private auth: Auth) { }

  chonAnhDaiDien() {
    const fileInput = document.querySelector('input[type="file"]') as HTMLElement;
    fileInput?.click();
  }

  onImageSelected(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      alert('Vui lòng chọn file ảnh hợp lệ');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      alert('Ảnh quá lớn (tối đa 5MB)');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      this.formCv.anh_dai_dien = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  taiCV() {
    this.http.post('https://localhost:5001/api/CV/tai-cv', this.formCv, {
      responseType: 'blob'
    }).subscribe(fileBlob => {
      const url = window.URL.createObjectURL(fileBlob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${this.formCv.ho_ten || 'CV'}.pdf`;
      a.click();
    });
  }

  moPopupNhapTen() {
    this.pop_up_nhap_ten_file = true;
  }

  dongPopup() {
    this.pop_up_nhap_ten_file = false;
  }

  xacNhanLuu() {
    if (!this.formCv.ten_cv || this.formCv.ten_cv.trim() === '') {
      alert('Vui lòng nhập tên CV!');
      return;
    }
    this.pop_up_nhap_ten_file = false;
    this.luuCV();
  }

  luuCV() {
    const nguoi_dung = this.auth.layThongTinNguoiDung();
    if (!nguoi_dung || !nguoi_dung.thong_tin_chi_tiet?.ma_nguoi_tim_viec) {
      alert('Vui lòng đăng nhập để lưu CV');
      return;
    }

    const thong_tin_cv = {
      ma_nguoi_tim_viec: nguoi_dung.thong_tin_chi_tiet.ma_nguoi_tim_viec,
      ten_cv: this.formCv.ten_cv,
      anh_dai_dien: this.formCv.anh_dai_dien,
      ho_ten: this.formCv.ho_ten,
      email: this.formCv.email,
      dien_thoai: this.formCv.dien_thoai,
      ngay_sinh: new Date(this.formCv.ngay_sinh).toISOString(),
      dia_chi: this.formCv.dia_chi,
      gioi_tinh: Number(this.formCv.gioi_tinh),
      chuyen_nganh: this.formCv.chuyen_nganh,
      ky_nang: this.formCv.ky_nang,
      du_an: this.formCv.du_an,
      muc_tieu: this.formCv.muc_tieu,
      vi_tri_ung_tuyen: this.formCv.vi_tri_ung_tuyen,
      hoc_Van: this.formCv.hoc_van,
      kinh_Nghiem: this.formCv.kinh_nghiem
    };

    console.log('Dữ liệu lưu CV:', thong_tin_cv);

    this.http.post('http://localhost:7000/api/API_WEB/luuCV', thong_tin_cv)
      .subscribe({
        next: (res: any) => {
          alert(' Lưu CV thành công!');
          console.log('Kết quả lưu CV:', res);
        },
        error: (err) => {
          console.error(' Lỗi khi lưu CV:', err);
          alert('Không thể lưu CV. Vui lòng thử lại sau.');
        }
      });
  }
}
