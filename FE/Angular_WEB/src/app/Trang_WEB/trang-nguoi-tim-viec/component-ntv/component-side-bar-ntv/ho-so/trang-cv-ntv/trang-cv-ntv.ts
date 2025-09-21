import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface CvForm {
  hoTen: string;
  email: string;
  dienThoai: string;
  ngay_sinh: string;
  dia_chi: string;
  truong_hoc: string;
  chuyen_nganh: string;
  kinh_nghiem: string;
  kyNang: string;
  duAn: string;
  mucTieu: string;
}

@Component({
  selector: 'app-trang-cv-ntv',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './trang-cv-ntv.html',
  styleUrls: ['./trang-cv-ntv.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrangCvNtv {
  uploadedFileName: string = '';
  dang_tai_file: File | null = null;

  formCv: CvForm = {
    hoTen: '',
    email: '',
    dienThoai: '',
    ngay_sinh: '',
    dia_chi: '',
    truong_hoc: '',
    chuyen_nganh: '',
    kinh_nghiem: '',
    kyNang: '',
    duAn: '',
    mucTieu: '',
  };

  ma_nguoi_dung = 1;
  cvs: any[] = [];
  hienModal = false;

  allBlocks = ['thongTinCoBan', 'hocVan', 'kinhNghiem', 'kyNang', 'duAn', 'mucTieu'];
  blocks: string[] = [];

  constructor(private http: HttpClient, private cdr: ChangeDetectorRef) {}

  moForm() {
    this.hienModal = true;
    this.blocks = [...this.allBlocks];
    console.log('Modal opened, all blocks loaded:', this.blocks);
    this.cdr.markForCheck(); 
  }

  dongForm() {
    this.hienModal = false;
    this.blocks = [];
    this.formCv = {
      hoTen: '',
      email: '',
      dienThoai: '',
      ngay_sinh: '',
      dia_chi: '',
      truong_hoc: '',
      chuyen_nganh: '',
      kinh_nghiem: '',
      kyNang: '',
      duAn: '',
      mucTieu: '',
    };
    this.cdr.markForCheck();
  }

  onFileSelected(event: any) {
    this.dang_tai_file = event.target.files[0];
    if (this.dang_tai_file) this.uploadedFileName = this.dang_tai_file.name;
    this.cdr.markForCheck();
  }

  dangTaiForm() {
    if (!this.dang_tai_file) {
      alert('Chưa chọn file!');
      return;
    }

    const formData = new FormData();
    formData.append('cvFile', this.dang_tai_file);
    formData.append('userId', this.ma_nguoi_dung.toString());

    this.http.post('https://localhost:65001/api/API_WEB/upload', formData)
      .subscribe({
        next: () => {
          alert('Upload CV thành công!');
          this.loadCvs();
        },
        error: (err) => {
          alert(`Upload lỗi: ${err.message}`);
        }
      });
  }

  luuCV() {
    const body = { ...this.formCv, userId: this.ma_nguoi_dung };

    this.http.post('https://localhost:65001/api/API_WEB/create', body)
      .subscribe({
        next: () => {
          alert('CV online lưu thành công!');
          this.loadCvs();
          this.dongForm();
        },
        error: (err) => {
          alert(`Lưu lỗi: ${err.message}`);
        }
      });
  }

  loadCvs() {
    this.http.get<any[]>(`https://localhost:65001/api/API_WEB/user/${this.ma_nguoi_dung}`)
      .subscribe({
        next: (data) => {
          this.cvs = data;
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi khi tải CV:', err);
        }
      });
  }
}
