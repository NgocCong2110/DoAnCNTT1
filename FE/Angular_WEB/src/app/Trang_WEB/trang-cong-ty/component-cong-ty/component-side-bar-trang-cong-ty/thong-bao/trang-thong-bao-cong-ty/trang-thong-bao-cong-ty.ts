import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-trang-thong-bao-cong-ty',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-bao-cong-ty.html',
  styleUrl: './trang-thong-bao-cong-ty.css'
})

export class TrangThongBaoCongTy implements OnInit {
  ma_cong_ty: number | undefined;
  constructor(
    public auth: Auth,
    public http: HttpClient,
    public cd: ChangeDetectorRef
  ) { }

  pop_up_them = false;
  pop_up_them_thanh_cong = false;
  pop_up_them_that_bai = false;

  danh_sach_viec_lam_cong_ty: any[] = [];

  dongPopUp() {
    this.pop_up_them = false;
  }

  moPopUpThemThongBao() {
    this.pop_up_them = true;
  }

  du_lieu_thong_bao_server: any = {
    tieu_de: '',
    noi_dung: '',
    loai_thong_bao: 2,
    ma_bai_dang: 0,
    ma_cong_ty: null
  };

  ngOnInit(): void {
    this.layDanhSachViecLamCongTy();
    this.ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_cong_ty;
    this.du_lieu_thong_bao_server.ma_cong_ty = this.ma_cong_ty;
  }

  guiThongBao() {
    const thong_tin = this.du_lieu_thong_bao_server;
    this.http.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/guiThongBaoViecLamMoi', thong_tin).subscribe({
      next: (data) => {
        if (data.success) {
          this.pop_up_them_thanh_cong = true;
          setTimeout(() => this.pop_up_them_thanh_cong = false, 2000);
        } else {
          this.pop_up_them_that_bai = true;
          setTimeout(() => this.pop_up_them_that_bai = false, 2000);
        }
      },
      error: () => {
        this.pop_up_them_that_bai = true;
        setTimeout(() => this.pop_up_them_that_bai = false, 2000);
      }
    });
  }

  layDanhSachViecLamCongTy() {
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_cong_ty;
    this.http.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/layDanhSachViecLamCuaCongTy', ma_cong_ty).subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_viec_lam_cong_ty = data.danh_sach;
            console.log(this.danh_sach_viec_lam_cong_ty);
            this.cd.detectChanges();
          } else {
          }
          this.cd.markForCheck();
        },
        error: () => {
          this.cd.detectChanges();
        }
      });
  }
  layNganhNghe(nganh_nghe: string): string {
    const mapping: { [key: string]: string } = {
      'bat_dong_san': 'Bất động sản',
      'cham_soc_khach_hang': 'Chăm sóc khách hàng',
      'co_khi_dien_dien_tu': 'Cơ khí - Điện - Điện tử',
      'cong_nghe_thong_tin': 'Công nghệ thông tin',
      'cong_tac_xa_hoi': 'Công tác xã hội - Phi lợi nhuận',
      'du_lich': 'Du lịch - Nhà hàng - Khách sạn',
      'giao_duc': 'Giáo dục - Đào tạo',
      'hanh_chinh': 'Hành chính - Văn phòng',
      'khac': 'Khác',
      'luat': 'Pháp lý - Luật',
      'marketing': 'Marketing',
      'nhan_su': 'Nhân sự',
      'nong_lam_ngu_nghiep': 'Nông - Lâm - Ngư nghiệp',
      'sales': 'Sales',
      'san_xuat': 'Sản xuất - Công nghiệp',
      'tai_chinh': 'Tài chính - Ngân hàng - Kế toán',
      'thiet_ke': 'Thiết kế - Mỹ thuật - Sáng tạo',
      'truyen_thong': 'Truyền thông - Quảng cáo',
      'van_tai': 'Vận tải - Kho vận - Logistics',
      'xay_dung': 'Xây dựng - Kiến trúc',
      'y_te': 'Y tế - Dược'

    };
    return mapping[nganh_nghe] || nganh_nghe;
  }
}