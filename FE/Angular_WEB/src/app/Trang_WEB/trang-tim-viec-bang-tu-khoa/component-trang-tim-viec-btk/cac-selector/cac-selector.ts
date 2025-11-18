import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { ThongTinViecLam } from '../../../../services/thong-tin-viec-lam-service/thong-tin-viec-lam';
import { ActivatedRoute } from '@angular/router';

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
export class CacSelector implements OnInit {

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef, private router: Router, public vl: ThongTinViecLam, private route: ActivatedRoute) { }

  danh_sach_de_xuat: any[] = [];

  danh_sach_goc: any[] = [];

  loading = false;

  toan_bo = true;

  de_xuat_thanh_tim_kiem = false;

  tung_phan = false;

  error = false;

  ma_bai_dang: number = 0;

  so_luong_viec_lam = 0;

  nganh_nghe = '';
  vi_tri = '';
  dia_diem = '';
  muc_luong: string = "";
  kinh_nghiem = '';
  hinh_thuc: number = 0;
  phuc_loi = '';
  quy_mo_cong_ty = '';

  nganh_nghe_label = '';
  dia_diem_label = '';
  muc_luong_label = '';
  kinh_nghiem_label = '';
  hinh_thuc_label = '';
  vi_tri_label = '';
  phuc_loi_label = '';
  quy_mo_cong_ty_label = '';

  //bo loc
  sap_xep_ngay = '';
  sap_xep_ngay_label = '';
  sap_xep_luong_label = '';

  nganh_nghe_params = '';

  nganhNgheList: {value: string, label: string}[] = [];


  diaDiemList = [
    { value: 'hanoi', label: 'Hà Nội' },
    { value: 'hcm', label: 'TP. Hồ Chí Minh' },
    { value: 'danang', label: 'Đà Nẵng' },
    { value: 'cantho', label: 'Cần Thơ' }
  ];

  mucLuongList = [
    { value: "10", label: 'Dưới 10 triệu' },
    { value: "11", label: '10 - 20 triệu' },
    { value: "21", label: '20 - 30 triệu' },
    { value: "30", label: 'Trên 30 triệu' },
    { value: "thoathuan", label: 'Thỏa thuận'}
  ];

  kinhNghiemList = [
    { value: "Không yêu cầu", label: "Không yêu cầu" },
    { value: "Dưới 1 năm", label: "Dưới 1 năm" },
    { value: "Từ 1 - 2 năm", label: "Từ 1 - 2 năm" },
    { value: "Từ 3 - 5 năm", label: "Từ 3 - 5 năm" },
    { value: "Trên 5 năm", label: "Trên 5 năm" }
  ];

  hinhThucList = [
    { value: 1, label: 'Toàn thời gian' },
    { value: 2, label: 'Bán thời gian' },
    { value: 3, label: 'Thực tập' },
    { value: 4, label: 'Freelance' }
  ];

  viTriList = [
    { value: 'nhan_vien', label: 'Nhân viên' },
    { value: 'truong_nhom', label: 'Trưởng nhóm' },
    { value: 'quan_ly', label: 'Quản lý' },
    { value: 'giam_doc', label: 'Giám đốc' },
    { value: 'chu_tich', label: 'Chủ tịch' }
  ];

  xoaBoLoc() {
    this.nganh_nghe = '';
      this.nganh_nghe_label = '';
      this.dia_diem = '';
      this.dia_diem_label = '';
      this.muc_luong = "";
      this.muc_luong_label = '';
      this.kinh_nghiem = '';
      this.kinh_nghiem_label = '';
      this.hinh_thuc = 0;
      this.hinh_thuc_label = '';
      this.vi_tri = '';
      this.vi_tri_label = '';
    const conBoLocNao =
      this.nganh_nghe || this.dia_diem || this.muc_luong ||
      this.kinh_nghiem || this.hinh_thuc || this.vi_tri;

    if (!conBoLocNao) {
      this.layDanhSachViecLam();
    } else {
      this.locCongViec();
    }
  }

  locCongViec() {
    const bo_loc = {
      nganh_nghe: this.nganh_nghe,
      vi_tri: this.vi_tri,
      dia_diem: this.dia_diem,
      kinh_nghiem: this.kinh_nghiem,
      muc_luong: this.muc_luong,
      loai_hinh: this.hinh_thuc,
    };
    this.loading = true;
    this.duaRaDeXuat(bo_loc);
  }

  boLocNgay(ngay: string) {
    let ds = [...this.danh_sach_de_xuat];
    this.sap_xep_ngay = ngay;
    if (this.sap_xep_ngay == 'moi_nhat') {
      // a- b > 0 dua b truoc a con a - b < 0 giu nguyen 
      ds.sort((a, b) => new Date(b.viec_Lam.ngay_cap_nhat).getTime() - new Date(a.viec_Lam.ngay_cap_nhat).getTime());
    }
    if (this.sap_xep_ngay == 'cu_nhat') {
      ds.sort((a, b) => new Date(a.viec_Lam.ngay_cap_nhat).getTime() - new Date(b.viec_Lam.ngay_cap_nhat).getTime())
    }
    this.danh_sach_de_xuat = ds;
  }

  layDanhSachNganhNghe(){
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachNganhNghe', {})
    .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
             this.nganhNgheList = data.danh_sach.map(n => ({ value: n.ma_nganh_nghe, label: n.ten_nganh_nghe }));
          }
          else {
            console.log("loi");
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          this.loading = false;
          this.error = true;
        }
      })
  }

  boLocLuong(luong: string) {
    let ds = [...this.danh_sach_de_xuat];
    if (luong == 'cao_Nhat') {
      ds.sort((a, b) => b.viec_Lam?.muc_luong_cao_nhat - a.viec_Lam?.muc_luong_cao_nhat);
    }
    if (luong == 'thap_Nhat') {
      ds.sort((a, b) => a.viec_Lam?.muc_luong_cao_nhat - b.viec_Lam?.muc_luong_cao_nhat);
    }
    this.danh_sach_de_xuat = ds;
  }

  ngOnInit(): void {

    this.layDanhSachNganhNghe();

    this.route.queryParams.subscribe(params => {
      this.xuLyThayDoiParam(params);
    });

    this.vl.ket_qua$.subscribe(data => {
      if (data && data.length > 0) {
        this.danh_sach_de_xuat = data;
        this.so_luong_viec_lam = this.danh_sach_de_xuat.length;
        this.de_xuat_thanh_tim_kiem = true;
        this.toan_bo = false;
        this.tung_phan = false;
        this.cd.markForCheck();
      }
    });

  }

  xuLyThayDoiParam(params: any) {
    const nganh = params['nganh'];

    if (nganh) {
      this.nganh_nghe_params = nganh;
      this.nganh_nghe = nganh;

      const found = this.nganhNgheList.find(item => item.value === nganh);
      if (found) {
        this.nganh_nghe_label = found.label;
      } else {
        this.nganh_nghe_label = '';
      }

      const thong_tin = {
        nganh_nghe: nganh,
        vi_tri: this.vi_tri,
        dia_diem: this.dia_diem,
        kinh_nghiem: this.kinh_nghiem,
        muc_luong: this.muc_luong,
        loai_hinh: this.hinh_thuc,
      }

      this.toan_bo = false;
      this.tung_phan = false;
      this.de_xuat_thanh_tim_kiem = true;
      this.duaRaDeXuat(thong_tin);
    }
    else {
      this.de_xuat_thanh_tim_kiem = false;
      this.toan_bo = true;
      this.tung_phan = false;
      this.layDanhSachViecLam();
    }
  }

  layDanhSachViecLam() {
    this.toan_bo = true;
    this.tung_phan = false;
    this.de_xuat_thanh_tim_kiem = false;
    this.cd.markForCheck();
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachViecLam', {})
      .subscribe({
        next: (data) => {
          this.loading = false;

          if (data.success) {
            this.danh_sach_de_xuat = [];
            this.danh_sach_de_xuat = data.danh_sach;
            this.danh_sach_goc = [...this.danh_sach_de_xuat];
            this.so_luong_viec_lam = this.danh_sach_de_xuat.length;
            this.error = false;
          }
          else {
            this.error = true;
            this.so_luong_viec_lam = 0;
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          this.loading = false;
          this.error = true;
          this.danh_sach_de_xuat = [];
          this.so_luong_viec_lam = 0
          this.cd.markForCheck();
        }
      })
  }

  duaRaDeXuat(viec_Lam: any) {
    console.log(viec_Lam)
    if (Object.values(viec_Lam).every(v => [null, undefined, '', 0].includes(v as any))) {
      this.layDanhSachViecLam();
      return;
    }
    this.toan_bo = false;
    this.de_xuat_thanh_tim_kiem = false;
    this.tung_phan = true;
    this.loading = true;
    this.cd.markForCheck();
    //van de ve warp object
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/deXuatViecLamSelector', viec_Lam)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_de_xuat = data.danh_sach || [];
            this.so_luong_viec_lam = this.danh_sach_de_xuat.length;
            this.error = false;
          }
          else {
            this.error = true;
            this.so_luong_viec_lam = 0;
            this.danh_sach_de_xuat = [];
          }
          this.cd.markForCheck();
        },
        error: (err) => {
          this.loading = false;
          this.error = true;
          this.danh_sach_de_xuat = [];
          this.so_luong_viec_lam = 0;
          this.cd.markForCheck();
        }
      })
  }

  chiTietBaiDang(ma_bai_dang: number) {
    this.router.navigate(['trang-chi-tiet-viec-lam'], {
      queryParams: { ma_bai_dang }
    });
  }

  layDuongDanLogo(url: string): string {
    if (!url) return "";
    if (!url.startsWith('http')) return `http://localhost:7000/${url}`;
    return url;
  }

}
