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

  loading = false;

  toan_bo = true;

  de_xuat_thanh_tim_kiem = false;

  tung_phan = false;

  error = false;

  ma_bai_dang: number = 0;

  nganh_nghe = '';
  vi_tri = '';
  dia_diem = '';
  muc_luong = '';
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

  nganhNgheList = [
    { value: 'cong_nghe_thong_tin', label: 'Công nghệ thông tin' },
    { value: 'tai_chinh', label: 'Tài chính - Ngân hàng - Kế toán' },
    { value: 'marketing', label: 'Marketing' },
    { value: 'sales', label: 'Sales' },
    { value: 'cham_soc_khach_hang', label: 'Chăm sóc khách hàng' },
    { value: 'san_xuat', label: 'Sản xuất - Công nghiệp' },
    { value: 'xay_dung', label: 'Xây dựng - Kiến trúc' },
    { value: 'giao_duc', label: 'Giáo dục - Đào tạo' },
    { value: 'y_te', label: 'Y tế - Dược' },
    { value: 'hanh_chinh', label: 'Hành chính - Văn phòng' },
    { value: 'nhan_su', label: 'Nhân sự' },
    { value: 'luat', label: 'Pháp lý - Luật' },
    { value: 'du_lich', label: 'Du lịch - Nhà hàng - Khách sạn' },
    { value: 'bat_dong_san', label: 'Bất động sản' },
    { value: 'van_tai', label: 'Vận tải - Kho vận - Logistics' },
    { value: 'truyen_thong', label: 'Truyền thông - Quảng cáo' },
    { value: 'thiet_ke', label: 'Thiết kế - Mỹ thuật - Sáng tạo' },
    { value: 'nong_lam_ngu_nghiep', label: 'Nông - Lâm - Ngư nghiệp' },
    { value: 'co_khi_dien_dien_tu', label: 'Cơ khí - Điện - Điện tử' },
    { value: 'cong_tac_xa_hoi', label: 'Công tác xã hội - Phi lợi nhuận' },
    { value: 'khac', label: 'Khác' }
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

  viTriList = [
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
      vi_tri: this.vi_tri,
      dia_diem: this.dia_diem,
      kinh_nghiem: this.kinh_nghiem,
      muc_luong: this.muc_luong,
      loai_hinh: this.hinh_thuc,
    };
    console.log(bo_loc)
    this.loading = true;
    this.duaRaDeXuat(bo_loc);
  }

  //tutu
  boLocNgay(ngay: string){
    let ds = [...this.danh_sach_de_xuat];
    this.sap_xep_ngay = ngay;
    if(this.sap_xep_ngay == 'moi_nhat'){
      // a- b > 0 dua b truoc a con a - b < 0 giu nguyen 
      ds.sort((a,b) => new Date(b.ngay_cap_nhat).getTime() - new Date(a.ngay_cap_nhat).getTime());
    }
    if(this.sap_xep_ngay == 'cu_nhat'){
      ds.sort((a,b) => new Date(a.ngay_cap_nhat).getTime() - new Date(b.ngay_cap_nhat).getTime())
    }
    this.danh_sach_de_xuat = ds;
  }

  boLocLuong(luong: string){
    let ds = [...this.danh_sach_de_xuat];
    if(luong == 'cao_Nhat'){
      ds.sort((a,b) => b.viec_lam?.muc_luong_cao_nhat - a.viec_lam?.muc_luong_cao_nhat);
    }
    if(luong == 'thap_Nhat'){
      ds.sort((a,b) => a.viec_lam?.muc_luong_cao_nhat - b.viec_lam?.muc_luong_cao_nhat);
    }
    this.danh_sach_de_xuat = ds;
  }

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      this.xuLyThayDoiParam(params);
    });

    this.vl.ket_qua$.subscribe(data => {
      if (data && data.length > 0) {
        this.danh_sach_de_xuat = data;
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

      this.toan_bo = false;
      this.tung_phan = false;
      this.de_xuat_thanh_tim_kiem = true;
      this.duaRaDeXuat({ nganh_nghe: nganh });
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
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/deXuatViecLamSelector', viec_Lam)
      .subscribe({
        next: (data) => {
          this.loading = false;
          if (data.success) {
            this.danh_sach_de_xuat = data.danh_sach || [];
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

  chiTietBaiDang(ma_bai_dang: number) {
    this.router.navigate(['trang-chi-tiet-viec-lam'], {
      queryParams: { ma_bai_dang }
    });
  }

  layDuongDanLogo(url : string) : string { 
    if(!url) return "";
    if(!url.startsWith('http')) return `http://localhost:65001/${url}`;
    return url;
  }

}
