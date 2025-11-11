import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Auth } from '../../../../../../services/auth';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE{
  success: boolean;
  url: string;
  message: string;
  danh_sach: any;
}

@Component({
  selector: 'app-trang-danh-sach-chung-chi',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-danh-sach-chung-chi.html',
  styleUrls: ['./trang-danh-sach-chung-chi.css']
})
export class TrangDanhSachChungChi implements OnInit{
  buoc: number = 1;
  danh_sach_chung_chi: any[] = [];
  tepPDF: File | null = null;
  hienPopup = false;
  error = '';
  loading = false;

  chungChi = {
    ma_nguoi_tim_viec: 0,
    ten_chung_chi: '',
    don_vi_cap: '',
    ngay_cap: '',
    ngay_het_han: '',
    ten_tep: ''
  };

  ngOnInit(): void {
    this.layDanhSachChungChi();
  }

  constructor(private httpclient: HttpClient, private auth: Auth, private cd: ChangeDetectorRef) { }

  chuyenSangBuoc2(form: any) {
    if (form.valid) {
      this.buoc = 2;
    } else {
      alert('Vui lòng nhập đầy đủ thông tin bắt buộc!');
    }
  }

  moPopup() {
    this.hienPopup = true;
    this.buoc = 1;
  }

  dongPopup() {
    this.hienPopup = false;
    this.resetForm();
  }


  quayLai() {
    this.buoc = 1;
  }

  chonFile(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    if (file.type !== 'application/pdf') {
      alert('Chỉ được tải lên tệp PDF!');
      return;
    }

    const gioi_han = 5 * 1024 * 1024;
    if (file.size > gioi_han) {
      alert('Tệp quá lớn (tối đa 5MB)!');
      return;
    }

    this.tepPDF = file;
    this.cd.markForCheck();
  }

  

  taiLen() {
    if (!this.tepPDF) {
      alert('Vui lòng chọn file PDF trước khi tải lên!');
      return;
    }

    const ngay_cap = new Date(this.chungChi.ngay_cap);
    const ngay_het_han = new Date(this.chungChi.ngay_het_han);

    const formData = new FormData();
    formData.append('ma_nguoi_tim_viec', this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.nguoi_tim_viec.ma_nguoi_tim_viec);
    formData.append('ten_chung_chi', this.chungChi.ten_chung_chi);
    formData.append('don_vi_cap', ngay_cap.toISOString());
    formData.append('ngay_cap', ngay_het_han.toISOString());
    formData.append('ngay_het_han', this.chungChi.ngay_het_han);
    formData.append('ten_tep', this.tepPDF);

    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/dangTaiChungChi', formData)
      .subscribe({
        next: (data) => {
          if(data.success){
            alert("da tang tai chung chi thanh cong");
            this.cd.detectChanges();
            this.resetForm();
          }
          else{
            console.log("loi");
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          console.log(err);
        }
      })
    
  }

  layDanhSachChungChi(){
    const ma_nguoi_tim_viec = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.nguoi_tim_viec.ma_nguoi_tim_viec;
    if(ma_nguoi_tim_viec == 0){
      alert("Khong tim thay ma nguoi tim viec");
      return;
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachChungChi', ma_nguoi_tim_viec, {headers: {"Content-Type" : "application/json"}})
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_chung_chi = data.danh_sach;
            this.cd.detectChanges();
          }
          else{
            console.log(data.message);
          }
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  resetForm() {
    this.chungChi = {
      ma_nguoi_tim_viec: 0,
      ten_chung_chi: '',
      don_vi_cap: '',
      ngay_cap: '',
      ngay_het_han: '',
      ten_tep: ''
    };
    this.tepPDF = null;
    this.buoc = 1;
  }

  xoaChungChi(chung_chi: any, index: number) {
    const thong_tin = {
      ma_chung_chi: chung_chi.ma_chung_chi,
      ma_nguoi_tim_viec: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet.nguoi_tim_viec.ma_nguoi_tim_viec
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/xoaChungChi', thong_tin)
      .subscribe({
        next: (data) =>{
          if(data.success){
            alert("xoa chung chi hoan tat");
            this.danh_sach_chung_chi.splice(index, 1);
            this.cd.detectChanges();
          }
          else{
            console.log(data.message);
          }
        },
        error: (err) => {
          console.log(err);
        }
      })
  }

  xemFile(url: string){
    if(!url) return '';
    if(!url.startsWith('http')) return 'http://localhost:7000/' + url;
    return url
  }
}
