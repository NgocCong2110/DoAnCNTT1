import { Component, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../Component/header-web/header-web';

@Component({
  selector: 'app-mau-cv-mac-dinh',
  standalone: true,
  imports: [FormsModule, CommonModule, HeaderWEB],
  templateUrl: './mau-cv-mac-dinh.html',
  styleUrls: ['./mau-cv-mac-dinh.css']
})
export class MauCvMacDinh {

  formCv = {
    anh_dai_dien: '',
    hoTen: '',
    email: '',
    dienThoai: '',
    truong_hoc: '',
    chuyen_nganh: '',
    mucTieu: '',
    viTriUngTuyen: '',
    ngaySinh: '',
    gioiTinh: '',
    website: '',
    diaChi: '',
    hocVan: [
      {
        thoiGian: '2019 - 2023',
        tenTruong: 'Đại học Công nghệ TP.HCM',
        nganhHoc: 'Công nghệ thông tin',
        moTa: 'Tốt nghiệp loại Giỏi, chuyên ngành Phát triển phần mềm.'
      }
    ],
    kinhNghiem: [
      {
        thoiGian: '2023 - nay',
        tenCongTy: 'Công ty TNHH ABC',
        viTri: 'Lập trình viên Frontend',
        moTa: 'Phát triển giao diện người dùng với Angular và TypeScript.'
      }
    ]
  };

  constructor(private http: HttpClient) { }

  chonAnhDaiDien() {
      const fileInput = document.querySelector('input[type="file"]') as HTMLElement;
      fileInput?.click();
    }

  taiCV() {
    this.http.post('https://localhost:5001/api/CV/tao-cv', this.formCv, {
      responseType: 'blob'
    }).subscribe(fileBlob => {
      const url = window.URL.createObjectURL(fileBlob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${this.formCv.hoTen || 'CV'}.pdf`;
      a.click();
    });
  }

  onImageSelected(event: any) {
    const file: File = event.target.files[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      alert('Vui lòng chọn file ảnh');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      alert('Kích thước của ảnh quá lớn');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      this.formCv.anh_dai_dien = reader.result as string;
    };
    reader.readAsDataURL(file);
  }
}
