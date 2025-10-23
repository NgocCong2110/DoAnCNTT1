import { Component, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { OnInit } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgModel } from '@angular/forms';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

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
  imports: [FormsModule, CommonModule, RouterLink, RouterModule],
  templateUrl: './trang-cv-ntv.html',
  styleUrls: ['./trang-cv-ntv.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TrangCvNtv implements OnInit {
  uploadedFileName: string = '';
  dang_tai_file: File | null = null;
  maucv: any;

  lua_chon_cv: any = null;

  ngOnInit() {
    this.loadCvs();
  }

  dsMauCV = [
    { key: 'modern', ten: 'Hiện đại', preview: 'assets/cv/modern.png' },
    { key: 'mac-dinh', ten: 'Mac dinh', preview: 'assets/cv/classic.png' },
    { key: 'minimal', ten: 'Tối giản', preview: 'assets/cv/minimal.png' }
  ];

  pdfUrl: SafeResourceUrl | null = null;

  thongTin: any;

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
  danh_sach_cv: any[] = [];
  hienModal = false;

  allBlocks = ['thongTinCoBan', 'hocVan', 'kinhNghiem', 'kyNang', 'duAn', 'mucTieu'];
  blocks: string[] = [];


  constructor(public auth: Auth, private httpclient: HttpClient, private cdr: ChangeDetectorRef, private sanitizer: DomSanitizer, private router: Router) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  moForm() {
    this.hienModal = true;
    this.blocks = [...this.allBlocks];
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

    const file: File = event.target.files[0];
    if (file && file.type !== 'application/pdf') {
      alert('Vui lòng chọn file PDF!');
      return;
    }

    if (file && file.size > 5 * 1024 * 1024) {
      alert('Kích thước file vượt quá 5MB!');
      return;
    }

    this.dang_tai_file = file;
    if (this.dang_tai_file) this.uploadedFileName = this.dang_tai_file.name;
    this.cdr.markForCheck();
  }

  dangTaiForm() {
    if (!this.dang_tai_file) {
      alert('Chưa chọn file!');
      return;
    }

    const formData = new FormData();
    formData.append('cv_file', this.dang_tai_file);
    formData.append('ma_Nguoi_Tim_Viec', this.thongTin?.thong_tin_chi_tiet.ma_nguoi_tim_viec);

    this.httpclient.post('http://localhost:65001/api/API_WEB/dangTaiCV', formData)
      .subscribe({
        next: () => {
          alert('Upload CV thành công!');
          this.loadCvs();
          window.location.reload();
        },
        error: (err) => {
          alert(`Upload lỗi: ${err.message}`);
        }
      });
  }



  luuCV() {
    const body = { ...this.formCv, userId: this.ma_nguoi_dung };

    this.httpclient.post('http://localhost:65001/api/API_WEB/luuCV', body)
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
    this.httpclient.post<API_RESPONSE>(`http://localhost:65001/api/API_WEB/layDanhSachCV`, { ma_nguoi_tim_viec: this.thongTin?.thong_tin_chi_tiet.ma_nguoi_tim_viec })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_cv = data.danh_sach;
            this.cdr.markForCheck();
          }
        },
        error: (err) => {
          console.error('Lỗi khi tải CV:', err);
        }
      });
  }

  xemCV(cv: any) {
    this.lua_chon_cv = cv;
    this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(`http://localhost:65001/cv-files/${cv.duong_dan_file}`);
  }

  chonMauCV(mau_cv : any){
    this.maucv = mau_cv;
  }

  xacNhanChonMau(){
    if(this.maucv == null){
      alert('chua chon mau cv');
      return;
    }

     console.log(this.maucv.key);

    if(this.maucv.key == 'mac-dinh'){
      this.router.navigate(['app-mau-cv-mac-dinh']);
    }
  }
}
