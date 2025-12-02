import { Component, OnInit, ViewChild, ElementRef, } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { BaiDang } from '../../services/bai-dang-service/bai-dang';
import { HeaderWEB } from '../Component/header-web/header-web';

declare var bootstrap: any;

interface API_RESPONSE {
  success: boolean;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-bai-dang-cua-cong-ty',
  imports: [CommonModule, FormsModule, HeaderWEB],
  templateUrl: './trang-bai-dang-cua-cong-ty.html',
  styleUrl: './trang-bai-dang-cua-cong-ty.css'
})
export class TrangBaiDangCuaCongTy implements OnInit {
  pop_up_xoa_bai = false;
  danh_sach_bai_lien_quan: any[] = [];
  danh_sach_tinh_thanh: any[] = [];
  thong_tin_bai_xoa = false;
  bai_dang_duoc_chon: any;
  pop_up_bao_cao: boolean = false;
  noi_dung_bao_cao: string = "";
  thongTin: any;
  so_luong_bai_dang: number = 0;

  editingField: string | null = null;
  editingValues: Record<string, any> = {};

  danh_sach_loai_hinh: any[] = [];

  ngOnInit(): void {
    this.layDanhSachNganhNghe();
    this.layDanhSachBaiDang();
    this.layDanhSachTinhThanh();
    this.khoi_tao_danh_sach_loai_hinh();
  }

  constructor(private httpclient: HttpClient, private auth: Auth, private cdr: ChangeDetectorRef, private baiDangService: BaiDang) { }

  khoi_tao_danh_sach_loai_hinh() {
    this.danh_sach_loai_hinh = [
      { id: 1, ten: 'Toàn thời gian' },
      { id: 2, ten: 'Bán thời gian' },
      { id: 3, ten: 'Thực tập' },
      { id: 4, ten: 'Tự Do' }
    ];
  }

  moPopUpXoa() {
    this.pop_up_xoa_bai = true;
    this.cdr.detectChanges();
  }

  huyXoa() {
    this.pop_up_xoa_bai = false;
    this.cdr.detectChanges();
  }

  xacNhanXoa() {
    if (!this.bai_dang_duoc_chon?.ma_bai_dang) return;

    this.baiDangService.xoaBaiDang(this.bai_dang_duoc_chon.ma_bai_dang)
      .then((data) => {
        if (data?.success) {
          this.thong_tin_bai_xoa = true;
          this.cdr.markForCheck();
          setTimeout(() => {
            this.thong_tin_bai_xoa = false;
            this.baiDangService.chonBaiDang(null);
            this.cdr.markForCheck();
            setTimeout(() => {
              window.location.reload();
            }, 500);
          }, 2000);
        } else {
          alert("Xoá bài đăng thất bại.");
        }
      })
      .catch(() => {
        alert("Có lỗi xảy ra khi xoá.");
      });
  }

  layDanhSachBaiDang() {
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_cong_ty;
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layToanBoDanhSachViecLamCuaCongTy', ma_cong_ty, { headers: { "Content-Type": "application/json" } })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_bai_lien_quan = data.danh_sach;
            this.so_luong_bai_dang = this.danh_sach_bai_lien_quan.length;
            if (this.bai_dang_duoc_chon == null) {
              this.bai_dang_duoc_chon = this.danh_sach_bai_lien_quan[0];
            }
            this.cdr.detectChanges();
          } else {
            console.log("loi")
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err);
          this.cdr.markForCheck();
        }
      });
  }

  anBaiDangCongTy(ma_bai_dang: number) {
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_cong_ty;
    const thong_tin = {
      ma_nguoi_dang: ma_cong_ty,
      ma_bai_dang: ma_bai_dang
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/anBaiDangCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.layDanhSachBaiDang();
            this.cdr.detectChanges();
          } else {
            console.log("loi")
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err);
          this.cdr.markForCheck();
        }
      });
  }

  boAnBaiDangCongTy(ma_bai_dang: number) {
    const ma_cong_ty = this.auth.layThongTinNguoiDung()?.thong_tin_chi_tiet?.ma_cong_ty;
    const thong_tin = {
      ma_nguoi_dang: ma_cong_ty,
      ma_bai_dang: ma_bai_dang
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/boAnBaiDangCongTy', thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.layDanhSachBaiDang();
            this.cdr.detectChanges();
          } else {
            console.log("loi")
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.log(err);
          this.cdr.markForCheck();
        }
      });
  }

  nganhNgheMapping: { [key: string]: string } = {
    cong_nghe_thong_tin: 'Công nghệ thông tin',
    cham_soc_khach_hang: 'Chăm sóc khách hàng',
    sales: 'Sales',
    tai_chinh: 'Tài chính',
    marketing: 'Marketing',
    ban_hang: 'Bán hàng',
    san_xuat: 'Sản xuất',
    giao_duc: 'Giáo dục',
    y_te: 'Y tế',
    hanh_chinh: 'Hành chính',
    xay_dung: 'Xây dựng',
    luat: 'Luật - Pháp lý',
    bat_dong_san: 'Bất động sản',
    du_lich: 'Du lịch',
    nong_nghiep: 'Nông nghiệp',
    nghe_thuat: 'Nghệ thuật',
    van_tai: 'Vận tải'
  };

  laynganhnghe(ma: string): string {
    return this.nganhNgheMapping[ma] || '';
  }

  trinhDoHocVanMap: { [key: number]: string } = {
    1: 'Trung học',
    2: 'Cao đẳng',
    3: 'Đại học',
    4: 'Tốt nghiệp',
    5: 'Khác',
    6: 'Không yêu cầu'
  }

  layTrinhDoHocVan(ma: number): string {
    return this.trinhDoHocVanMap[ma] || '';
  }

  loaiHinhMap: { [key: number]: string } = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Thực tập",
    4: "Tự Do"
  };

  layLoaiHinh(loaiHinh: number) {
    return this.loaiHinhMap[loaiHinh] || 'Không xác định';
  }

  viTriMap: { [key: string]: string } = {
    nhan_vien: "Nhân viên",
    truong_nhom: "Trưởng nhóm",
    quan_ly: "Quản lý",
    giam_doc: "Giám đốc",
    chu_tich: "Chủ tịch"
  };

  layViTri(viTri: string): string {
    return this.viTriMap[viTri] || 'Không xác định';
  }

  layTrangThaiBaiDang(trang_thai: number) {
    if (trang_thai == 1) {
      return 'Công khai';
    }
    else if (trang_thai == 2) {
      return 'Riêng tư';
    }
    return 'Đã đóng'
  }

  @ViewChild('modalDangBai') modalDangBai!: ElementRef;

  phucLoiList: { ma_phuc_loi: number, ten_phuc_loi: string }[] = [
    { ma_phuc_loi: 1, ten_phuc_loi: 'Thưởng Tết hấp dẫn' },
    { ma_phuc_loi: 2, ten_phuc_loi: 'Thưởng hiệu suất làm việc' },
    { ma_phuc_loi: 3, ten_phuc_loi: 'Du lịch, team building hàng năm' },
    { ma_phuc_loi: 4, ten_phuc_loi: 'Bảo hiểm xã hội, y tế, thất nghiệp đầy đủ' },
    { ma_phuc_loi: 5, ten_phuc_loi: 'Khám sức khỏe định kỳ hằng năm' },
    { ma_phuc_loi: 6, ten_phuc_loi: 'Nghỉ phép năm và nghỉ lễ theo quy định' },
    { ma_phuc_loi: 7, ten_phuc_loi: 'Phụ cấp ăn trưa, xăng xe, điện thoại' },
    { ma_phuc_loi: 8, ten_phuc_loi: 'Làm việc trong môi trường chuyên nghiệp, năng động' },
    { ma_phuc_loi: 9, ten_phuc_loi: 'Cơ hội thăng tiến rõ ràng' },
    { ma_phuc_loi: 10, ten_phuc_loi: 'Được đào tạo và phát triển kỹ năng thường xuyên' },
    { ma_phuc_loi: 11, ten_phuc_loi: 'Chính sách thưởng theo dự án hoặc doanh số' },
    { ma_phuc_loi: 12, ten_phuc_loi: 'Làm việc từ xa linh hoạt (hybrid/remote)' },
    { ma_phuc_loi: 13, ten_phuc_loi: 'Được cấp laptop và trang thiết bị làm việc hiện đại' },
    { ma_phuc_loi: 14, ten_phuc_loi: 'Tham gia các hoạt động văn hóa, thể thao của công ty' },
    { ma_phuc_loi: 15, ten_phuc_loi: 'Chính sách hỗ trợ nhân viên ngoại tỉnh (nhà ở hoặc đi lại)' },
    { ma_phuc_loi: 16, ten_phuc_loi: 'Chế độ thai sản và nghỉ ốm đầy đủ cho nhân viên nữ' },
    { ma_phuc_loi: 17, ten_phuc_loi: 'Thưởng sinh nhật, cưới hỏi, lễ tết, và các dịp đặc biệt' },
    { ma_phuc_loi: 18, ten_phuc_loi: 'Tăng lương định kỳ hàng năm theo năng lực' },
    { ma_phuc_loi: 19, ten_phuc_loi: 'Được đóng góp ý kiến và tham gia vào các dự án quan trọng' },
    { ma_phuc_loi: 20, ten_phuc_loi: 'Giờ làm việc linh hoạt, cân bằng giữa công việc và cuộc sống' }
  ];

  mucLuongList = [
    { min: 10, max: 15, value: 1, label: '10 – 15 triệu' },
    { min: 15, max: 20, value: 2, label: '15 – 20 triệu' },
    { min: 20, max: 30, value: 3, label: '20 – 30 triệu' },
    { min: 30, max: 40, value: 4, label: '30 – 40 triệu' },
    { min: null, max: null, value: 5, label: 'Thỏa thuận' },
    { min: null, max: null, value: 6, label: 'Khác' }
  ];

  viTriList = [
    { value: 'nhan_vien', label: 'Nhân viên' },
    { value: 'truong_nhom', label: 'Trưởng nhóm' },
    { value: 'quan_ly', label: 'Quản lý' },
    { value: 'giam_doc', label: 'Giám đốc' },
    { value: 'chu_tich', label: 'Chủ tịch' }
  ];
  vi_tri_label = '';

  muc_luong_label: string | null = null;
  muc_luong_min: number | null = null;
  muc_luong_max: number | null = null;
  showCustomLuong: boolean = false;
  loai_hinh: string | null = null;
  loai_hinh_label: string | null = null;

  vi_tri: string = '';

  tieu_de_moi = '';
  noi_dung_moi = '';
  loai_bai = 1;
  trang_thai = 1;

  yeu_cau_moi = '';
  dia_diem_moi = '';

  trinh_do_hoc_van = 0;

  gio_lam_viec = '';

  thoi_han_nop_cv: Date = new Date();

  pop_up_dang_bai_thanh_cong: boolean = false;
  pop_up_dang_bai_that_bai: boolean = false;

  phucLoiDaChon: any[] = [];
  tinhThanhDaChon: any[] = [];
  quyen_loi_khac: string = "";

  chonMucLuong(muc: any) {
    this.muc_luong_label = muc.label;

    if (muc.label === 'Khác') {
      this.showCustomLuong = true;
      this.muc_luong_min = null;
      this.muc_luong_max = null;
    }
    else if (muc.label === 'Thỏa thuận') {
      this.showCustomLuong = false;
      this.muc_luong_min = null;
      this.muc_luong_max = null;
    }
    else {
      this.showCustomLuong = false;
      this.muc_luong_min = muc.min;
      this.muc_luong_max = muc.max;
    }
  }

  chuanHoaMucLuong() {
    if (this.showCustomLuong) {
      if (this.muc_luong_min == null || this.muc_luong_max == null) {
        alert('Vui lòng nhập mức lương hợp lệ');
        return false;
      }
      if (this.muc_luong_min > this.muc_luong_max) {
        [this.muc_luong_min, this.muc_luong_max] = [this.muc_luong_max, this.muc_luong_min];
      }
      this.muc_luong_label = `${this.muc_luong_min} – ${this.muc_luong_max} triệu`;
    }
    return true;
  }

  nganhNgheList: { value: string, label: string }[] = [];

  trinhDoHocVanList = [
    { value: 1, label: 'Trung Học' },
    { value: 2, label: 'Cao Đẳng' },
    { value: 3, label: 'Đại Học' },
    { value: 4, label: 'Tốt nghiệp' },
    { value: 5, label: 'Khác' },
    { value: 6, label: 'Không yêu cầu' }
  ]

  trinhDoHocVanCapNhatList = [
    { value: 'trung_Hoc', label: 'Trung Học' },
    { value: 'cao_Dang', label: 'Cao Đẳng' },
    { value: 'dai_Hoc', label: 'Đại Học' },
    { value: 'tot_Nghiep', label: 'Tốt nghiệp' },
    { value: 'khac', label: 'Khác' },
    { value: 'khong_Yeu_Cau', label: 'Không yêu cầu' }
  ]

  nganh_nghe: string | null = null;
  nganh_nghe_label: string | null = null;

  kinhNghiemList = [
    { value: 'none', label: 'Không yêu cầu' },
    { value: '1 năm', label: '1 năm' },
    { value: '2 tới 3 năm', label: '2 – 3 năm' },
    { value: '3 tới 5 năm', label: '3 – 5 năm' },
    { value: 'trên 5 năm', label: 'Trên 5 năm' }
  ];
  kinh_nghiem: string | null = null;
  kinh_nghiem_label: string | null = null;

  danh_sach_kinh_nghiem = [
    { value: "Không yêu cầu", label: "Không yêu cầu" },
    { value: "Dưới 1 năm", label: "Dưới 1 năm" },
    { value: "Từ 1 - 2 năm", label: "Từ 1 - 2 năm" },
    { value: "Từ 3 - 5 năm", label: "Từ 3 - 5 năm" },
    { value: "Trên 5 năm", label: "Trên 5 năm" }
  ];

  chonKinhNghiem(event: any) {
    const selected = this.danh_sach_kinh_nghiem.find(x => x.value === event.target.value);
    this.kinh_nghiem = selected?.value ?? null;
    this.kinh_nghiem_label = selected?.label ?? null;
  }

  loaiHinhList = [
    { value: '1', label: 'Toàn thời gian' },
    { value: '2', label: 'Bán thời gian' },
    { value: '3', label: 'Thực tập' }
  ];

  chonPhucLoi(event: any, phuc: { ma_phuc_loi: number, ten_phuc_loi: string }) {
    if (event.target.checked) {
      this.phucLoiDaChon.push(phuc.ma_phuc_loi);
    } else {
      this.phucLoiDaChon = this.phucLoiDaChon.filter(x => x !== phuc.ma_phuc_loi);
    }
  }

  chonTinhThanh(event: any, tinh: { ma_tinh: number, ten_tinh: string }) {
    if (event.target.checked) {
      this.tinhThanhDaChon.push(tinh.ma_tinh);
    } else {
      this.tinhThanhDaChon = this.tinhThanhDaChon.filter(x => x !== tinh.ma_tinh);
    }
  }

  layDanhSachNganhNghe() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachNganhNghe', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.nganhNgheList = data.danh_sach.map(n => ({ value: n.ma_nganh_nghe, label: n.ten_nganh_nghe }));
          }
          else {
            console.log("loi");
          }
          this.cdr.markForCheck();
        },
        error: (err) => {

        }
      })
  }

  layDanhSachTinhThanh() {
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachTinhThanh', {})
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_tinh_thanh = data.danh_sach.map(n => ({ ma_tinh: n.ma_tinh, ten_tinh: n.ten_tinh }));
          }
          else {
            console.log("loi");
          }
          this.cdr.markForCheck();
        },
        error: (err) => {

        }
      })
  }

  moPopup() {
    const modal = new bootstrap.Modal(this.modalDangBai.nativeElement);
    modal.show();
  }

  themBaiDang() {
    this.thongTinBaiDang();
  }

  thongTinBaiDang() {
    const nguoiDung = this.auth.layThongTinNguoiDung();

    if (this.so_luong_bai_dang > 10) {
      alert('Bạn đã vượt giới hạn đăng bài là 10 bài!');
      return;
    }

    const thong_Tin = {
      bai_Dang: {
        ma_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ma_cong_ty,
        ten_nguoi_dang: nguoiDung.thong_tin_chi_tiet.ten_cong_ty,
        tieu_de: this.tieu_de_moi,
        noi_dung: this.noi_dung_moi,
        loai_bai: this.loai_bai,
        trang_thai: this.trang_thai,
        ngay_tao: new Date().toISOString(),
        ngay_cap_nhat: new Date().toISOString()
      },
      viec_Lam: {
        ma_cong_ty: nguoiDung.thong_tin_chi_tiet.ma_cong_ty ?? 0,
        nganh_nghe: this.nganh_nghe,
        vi_tri: this.vi_tri,
        kinh_nghiem: this.kinh_nghiem,
        tieu_de: this.tieu_de_moi,
        mo_ta: this.noi_dung_moi,
        yeu_cau: this.yeu_cau_moi,
        muc_luong: this.showCustomLuong ? `${this.muc_luong_min} – ${this.muc_luong_max} triệu` : this.muc_luong_label,
        muc_luong_thap_nhat: this.muc_luong_min,
        muc_luong_cao_nhat: this.muc_luong_max,
        quyen_loi_cong_viec: this.quyen_loi_khac,
        trinh_do_hoc_van_yeu_cau: Number(this.trinh_do_hoc_van) || 6,
        thoi_gian_lam_viec: this.gio_lam_viec,
        dia_diem: this.dia_diem_moi,
        thoi_han_nop_cv: new Date(this.thoi_han_nop_cv).toISOString(),
        loai_hinh: Number(this.loai_hinh) || 1
      },
      phuc_Loi: this.phucLoiDaChon,
      tinh_Thanh: this.tinhThanhDaChon
    };

    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/themBaiDangMoi', thong_Tin)
      .subscribe({
        next: (data) => {
          if (data.success) {

            this.pop_up_dang_bai_thanh_cong = true;
            this.cdr.markForCheck();

            const modal = bootstrap.Modal.getInstance(this.modalDangBai.nativeElement);
            modal.hide();

            setTimeout(() => {
              this.pop_up_dang_bai_thanh_cong = false;
              this.cdr.markForCheck();
              setTimeout(() => {
                window.location.reload();
              }, 500);
            }, 2000);

          } else {
            this.pop_up_dang_bai_that_bai = true;
            this.cdr.detectChanges();
            setTimeout(() => {
              this.pop_up_dang_bai_that_bai = false;
            }, 3000);
          }
        },
        error: (err) => {
          console.error('Lỗi khi thêm bài đăng:', err);
          this.pop_up_dang_bai_that_bai = true;
          this.cdr.detectChanges();
          setTimeout(() => {
            this.pop_up_dang_bai_that_bai = false;
            this.cdr.detectChanges();
          }, 1500);
        }
      });
  }

  isLuongTooLong(): boolean {
    const minStr = this.muc_luong_min?.toString() || '';
    const maxStr = this.muc_luong_max?.toString() || '';
    return minStr.length > 5 || maxStr.length > 5;
  }

  enableEdit(field: string) {
    if (!this.bai_dang_duoc_chon) return;
    this.editingField = field;
    if (field === 'muc_luong' && this.bai_dang_duoc_chon.muc_luong) {
      const mlObj = this.mucLuongList.find(ml => ml.label === this.bai_dang_duoc_chon.muc_luong);
      this.editingValues[field] = mlObj ? mlObj.value : null;
    } else {
      this.editingValues[field] = this.bai_dang_duoc_chon[field];
    }
  }

  cancelEdit() {
    this.editingField = null;
    this.editingValues = {};
  }

  saveEdit() {
    if (!this.bai_dang_duoc_chon || !this.editingField) return;

    const key = this.editingField;
    const selectedValue = this.editingValues[key];

    let gia_tri_moi: any = selectedValue;
    let muc_luong_thap_nhat: number | null = null;
    let muc_luong_cao_nhat: number | null = null;

    if (key === 'muc_luong') {
      const mlObj = this.mucLuongList.find(ml => ml.value === selectedValue);
      if (mlObj) {
        gia_tri_moi = mlObj.label;
        muc_luong_thap_nhat = mlObj.min;
        muc_luong_cao_nhat = mlObj.max;
      }
    }

    if (key === 'thoi_han_nop_cv' && gia_tri_moi) {
      gia_tri_moi = new Date(gia_tri_moi).toISOString();
    }

    const thong_tin_cap_nhat: any = {
      ma_cong_ty: this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_cong_ty,
      ma_bai_dang: this.bai_dang_duoc_chon.ma_bai_dang,
      truong: key,
      gia_tri: gia_tri_moi
    };

    if (key === 'muc_luong') {
      thong_tin_cap_nhat.muc_luong_thap_nhat = muc_luong_thap_nhat;
      thong_tin_cap_nhat.muc_luong_cao_nhat = muc_luong_cao_nhat;
    }

    this.httpclient.patch<any>('http://localhost:7000/api/API_WEB/capNhatBaiDang', thong_tin_cap_nhat)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.bai_dang_duoc_chon[key] = gia_tri_moi;
            if (key === 'muc_luong') {
              this.bai_dang_duoc_chon.muc_luong_thap_nhat = muc_luong_thap_nhat;
              this.bai_dang_duoc_chon.muc_luong_cao_nhat = muc_luong_cao_nhat;
            }
            alert('Cập nhật thành công!');
            this.cancelEdit();
            this.cdr.detectChanges();
          } else {
            alert('Cập nhật thất bại!');
            this.cancelEdit();
          }
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error('Lỗi khi cập nhật:', err);
          alert('Có lỗi xảy ra khi cập nhật bài đăng!');
          this.cancelEdit();
        }
      });
  }
}