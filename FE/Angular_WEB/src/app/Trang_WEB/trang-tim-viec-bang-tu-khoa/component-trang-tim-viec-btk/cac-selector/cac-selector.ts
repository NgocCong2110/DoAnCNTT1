import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cac-selector',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './cac-selector.html', 
  styleUrls: ['./cac-selector.css']
})
export class CacSelector {

  nganh_nghe = '';
  dia_diem = '';
  muc_luong = '';
  kinh_nghiem = '';
  hinh_thuc = '';
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
    { value: 'tai_chinh', label: 'Tài chính - Kế toán' },
    { value: 'kinh_doanh', label: 'Kinh doanh - Bán hàng' }
  ];

  diaDiemList = [
    { value: 'hanoi', label: 'Hà Nội' },
    { value: 'hcm', label: 'TP. Hồ Chí Minh' },
    { value: 'danang', label: 'Đà Nẵng' },
    { value: 'lam_viec_tu_xa', label: 'Remote' }
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
    { value: 'fulltime', label: 'Toàn thời gian' },
    { value: 'parttime', label: 'Bán thời gian' },
    { value: 'freelance', label: 'Freelance' },
    { value: 'lam_viec_tu_xa', label: 'Remote' }
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
      hinh_thuc: this.hinh_thuc,
      chuc_vu: this.chuc_vu,
      phuc_loi: this.phuc_loi,
      quy_mo_cong_ty: this.quy_mo_cong_ty
    };

    this.duaRaDeXuat(bo_loc);
  }

  async duaRaDeXuat(bo_loc: any) { 
    try {
      const response = await fetch("http://localhost:65001/api/API_WEB/", { 
        method: "POST", 
        headers: { "Content-Type": "application/json" }, 
        body: JSON.stringify(bo_loc) 
      }); 
      const data = await response.json(); 
      if (data.success) { 
        console.log("Dữ liệu lọc thành công:", data); 
      } else {
        console.error("Lỗi:", data.message); 
      } 
    } catch (error) {
      console.error("Lỗi kết nối API:", error);
    }
  }
}
