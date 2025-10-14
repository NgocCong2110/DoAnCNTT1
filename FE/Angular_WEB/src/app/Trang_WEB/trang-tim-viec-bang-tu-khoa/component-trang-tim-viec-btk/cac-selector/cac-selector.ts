import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { ThongTinViecLam } from '../../../../services/thong-tin-viec-lam-service/thong-tin-viec-lam';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-cac-selector',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './cac-selector.html',
  styleUrls: ['./cac-selector.css']
})
export class CacSelector implements OnInit{

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef, private router: Router, public vl: ThongTinViecLam) { }

  danh_sach_de_xuat: any[] = [];

  loading = false;

  toan_bo = true;

  tung_phan = false;

  error = false;

  ma_bai_dang: number = 0;

  nganh_nghe = '';
  dia_diem = '';
  muc_luong = '';
  kinh_nghiem = '';
  hinh_thuc: number = 0;
  chuc_vu = '';
  phuc_loi = '';
  quy_mo_cong_ty = '';

  nganh_nghe_label = '';
  dia_diem_label = '';
  muc_luong_label = '';
  kinh_nghiem_label = '';
  hinh_thuc_label = '';
  chuc_vu_label = '';
  phuc_loi_label = '';
  quy_mo_cong_ty_label = '';

  nganhNgheList = [
    { value: 'cong_nghe_thong_tin', label: 'Công nghệ thông tin' },
    { value: 'marketing', label: 'Marketing' },
    { value: 'vien_thong', label: 'Viễn Thông' },
    { value: 'ban_le', label: 'Kinh doanh - Bán hàng' }
  ];

  diaDiemList = [
    { value: 'hanoi', label: 'Hà Nội' },
    { value: 'hcm', label: 'TP. Hồ Chí Minh' },
    { value: 'danang', label: 'Đà Nẵng' },
    { value: 'cantho', label: 'Cần Thơ' }
  ];

  mucLuongList = [
    { value: 'duoi_10', label: 'Dưới 10 triệu' },
    { value: '10_20', label: '10 - 20 triệu' },
    { value: '20_30', label: '20 - 30 triệu' },
    { value: 'tren_30', label: 'Trên 30 triệu' }
  ];

  kinhNghiemList = [
    { value: 'chua_co_kn', label: 'Chưa có kinh nghiệm' },
    { value: '1_2', label: '1 - 2 năm' },
    { value: '3_5', label: '3 - 5 năm' },
    { value: 'tren_5', label: 'Trên 5 năm' }
  ];

  hinhThucList = [
    { value: 1, label: 'Toàn thời gian' },
    { value: 2, label: 'Bán thời gian' },
    { value: 3, label: 'Thực tập' },
    { value: 4, label: 'Freelance' }
  ];

  chucVuList = [
    { value: 'nhan_vien', label: 'Nhân viên' },
    { value: 'truong_nhom', label: 'Trưởng nhóm' },
    { value: 'quan_ly', label: 'Quản lý' },
    { value: 'giam_doc', label: 'Giám đốc' },
    { value: 'chu_tich', label: 'Chủ tịch' }
  ];

  phucLoiList = [
    { value: 'bao_hiem_day_du', label: 'Bảo hiểm đầy đủ' },
    { value: 'luong_thang_13', label: 'Thưởng tháng 13' },
    { value: 'du_lich_cong_ty', label: 'Du lịch công ty' },
    { value: 'phu_cap_an_true', label: 'Phụ cấp ăn trưa' },
    { value: 'remote', label: 'Hỗ trợ remote' }
  ];

  quyMoCongTyList = [
    { value: 'duoi_50', label: 'Dưới 50 người' },
    { value: '50_200', label: '50 - 200 người' },
    { value: '200_1000', label: '200 - 1000 người' },
    { value: 'tren_1000', label: 'Trên 1000 người' }
  ];

  locCongViec() {
    const bo_loc = {
      nganh_nghe: this.nganh_nghe,
      dia_diem: this.dia_diem,
      muc_luong: this.muc_luong,
      kinh_nghiem: this.kinh_nghiem,
      loai_hinh: this.hinh_thuc,
    };

    this.loading = true;
    this.duaRaDeXuat(bo_loc);
  }

  ngOnInit(): void {
    this.vl.ket_qua$.subscribe(data => {
      if(data && data.length > 0){
        this.danh_sach_de_xuat = data;
        this.toan_bo = false;
        this.tung_phan = true;
        this.cd.markForCheck();
      }
    })

    this.layDanhSachViecLam();
  }

  layDanhSachViecLam()  {
    this.toan_bo = true;
    this.tung_phan = false;
    this.cd.markForCheck();
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachViecLam', {})
      .subscribe({
        next: (data) => {
          this.loading = false;

          if (data.success) {
            this.danh_sach_de_xuat = [];
            this.danh_sach_de_xuat = data.danh_sach;
            this.error = false;
          }
          else {
            this.error = true;
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          this.loading = false;
          this.error = true;
          this.danh_sach_de_xuat = [];
          this.cd.markForCheck();
        }
      })
  }

  duaRaDeXuat(viec_Lam: any) {
    this.toan_bo = false;
    this.tung_phan = true;
    this.loading = true;
    this.cd.markForCheck();
    //van de ve warp object
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/deXuatViecLamSelector', viec_Lam)
      .subscribe({
        next: (data) => {
          this.loading = false;

          if (data.success) {
            this.danh_sach_de_xuat = [];
            this.danh_sach_de_xuat = data.danh_sach;
            this.error = false;
          }
          else {
            this.error = true;
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          this.loading = false;
          this.error = true;
          this.danh_sach_de_xuat = [];
          this.cd.markForCheck();
        }
      })
  }

  chiTietBaiDang(ma_bai_dang: number){
    this.router.navigate(['trang-chi-tiet-viec-lam', ma_bai_dang]);
  }
}
