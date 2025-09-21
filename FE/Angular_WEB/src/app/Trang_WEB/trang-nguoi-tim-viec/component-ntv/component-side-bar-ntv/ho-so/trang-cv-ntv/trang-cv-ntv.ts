import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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

  moForm() {
    this.hienModal = true;
    this.blocks = [...this.allBlocks]; 
    console.log('Modal opened, all blocks loaded:', this.blocks);
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
  }

  onFileSelected(event: any) {
    this.dang_tai_file = event.target.files[0];
    if (this.dang_tai_file) this.uploadedFileName = this.dang_tai_file.name;
  }

  async dangTaiForm() {
    if (!this.dang_tai_file) return alert('Chưa chọn file!');
    const formData = new FormData();
    formData.append('cvFile', this.dang_tai_file);
    formData.append('userId', this.ma_nguoi_dung.toString());

    try {
      const response = await fetch('https://localhost:7290/api/API_WEB/upload', {
        method: 'POST',
        body: formData,
      });
      if (response.ok) {
        alert('Upload CV thành công!');
        this.loadCvs();
      } else {
        alert(`Upload lỗi: ${response.statusText}`);
      }
    } catch (error) {
      alert(`Upload lỗi: ${error}`);
    }
  }

  async luuCV() {
    const body = { ...this.formCv, userId: this.ma_nguoi_dung };
    try {
      const response = await fetch('https://localhost:7290/api/API_WEB/create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
      });
      if (response.ok) {
        alert('CV online lưu thành công!');
        this.loadCvs();
        this.dongForm();
      } else {
        alert(`Lưu lỗi: ${response.statusText}`);
      }
    } catch (error) {
      alert(`Lưu lỗi: ${error}`);
    }
  }

  async loadCvs() {
    try {
      const response = await fetch(`https://localhost:7290/api/API_WEB/user/${this.ma_nguoi_dung}`);
      if (response.ok) {
        this.cvs = await response.json();
      } else {
        console.error('Lỗi khi tải CV:', response.statusText);
      }
    } catch (error) {
      console.error('Lỗi khi tải CV:', error);
    }
  }
}
