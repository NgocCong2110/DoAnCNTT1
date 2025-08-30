import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../../../services/auth';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan',
  imports: [],
  templateUrl: './trang-thong-tin-tai-khoan.html',
  styleUrl: './trang-thong-tin-tai-khoan.css'
})
export class TrangThongTinTaiKhoan implements OnInit {

  constructor(private auth: Auth) { }

  thongTin: any;

  ngOnInit(): void {
    const thongtin_NguoiDung = this.thongTinNguoiDung();
    this.thongTin = {
      thongTin_NguoiDung: {
        ma_nguoi_tim_viec: null,
        ho_ten: thongtin_NguoiDung.ho_ten,
        ten_dang_nhap: thongtin_NguoiDung.ten_dang_nhap,
        email: thongtin_NguoiDung.email,
        dien_thoai: thongtin_NguoiDung.dien_thoai,
        ngay_sinh: thongtin_NguoiDung.ngay_sinh,
        gioi_tinh: thongtin_NguoiDung.gioi_tinh,
        trinh_do_hoc_van: thongtin_NguoiDung.trinh_do_hoc_van,
        dia_chi: thongtin_NguoiDung.dia_chi,
        anh_dai_dien: thongtin_NguoiDung.anh_dai_dien,
        quoc_tich: thongtin_NguoiDung.quoc_tich,
        mo_ta: thongtin_NguoiDung.mo_ta
      }
    }
  }

  thongTinNguoiDung(){
    return this.auth.layThongTinNguoiDung();
  }
}
