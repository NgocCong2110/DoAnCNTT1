import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

interface API_RESPONSE {
  success: boolean;
  message: string;
  danh_sach: any[];
}

@Component({
  selector: 'app-trang-danh-sach-ung-vien',
  standalone: true,
  imports: [CommonModule, DatePipe, FormsModule],
  templateUrl: './trang-danh-sach-ung-vien.html',
  styleUrls: ['./trang-danh-sach-ung-vien.css']
})
export class TrangDanhSachUngVien implements OnInit {

  thongTin: any = null;


  danh_sach_ung_vien_full: any[] = [];
  danh_sach_ung_vien: any[] = [];


  loading = true;
  error = '';

  pop_up_xoa_thanh_cong = false;
  pop_up_xoa_that_bai = false;
  pop_up_lay_thong_tin_that_bai = false;
  pop_up_moi_phong_van_thanh_cong = false;
  pop_up_moi_phong_van_that_bai = false;
  pop_up_tu_choi_that_bai = false;

  uvMuonXoa: any = null;
  indexMuonXoa: number | null = null;
  showXacNhanXoa = false;

  trangHienTai = 1;
  soLuongMoiTrang = 10;
  tongTrang = 1;

  constructor(
    public auth: Auth,
    public httpclient: HttpClient,
    public cd: ChangeDetectorRef,
    private sanitizer: DomSanitizer
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.layDanhSachUngVien();
  }

  layDanhSachUngVien() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;

    if (!ma_Cong_Ty) {
      this.error = "Không tìm thấy thông tin công ty (vui lòng đăng nhập lại)";
      this.loading = false;
      return;
    }

    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachUngVien', { ma_cong_ty: ma_Cong_Ty })
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.danh_sach_ung_vien_full = data.danh_sach;
            this.tongTrang = Math.ceil(this.danh_sach_ung_vien_full.length / this.soLuongMoiTrang);
            this.loadTrang(this.trangHienTai);
          } else {
            this.pop_up_lay_thong_tin_that_bai = true;
            setTimeout(() => this.pop_up_lay_thong_tin_that_bai = false, 1500);
          }
          this.loading = false;
          this.cd.detectChanges();
        },
        error: (err) => {
          console.error('Lỗi khi gọi API:', err);
          this.error = "Lỗi khi tải dữ liệu";
          this.loading = false;
          this.cd.detectChanges();
        }
      });
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danh_sach_ung_vien = this.danh_sach_ung_vien_full.slice(start, end);
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }

  layTrinhDoHocVan: { [key: number]: string } = {
    1: "Trung học",
    2: "Cao đẳng",
    3: "Đại học",
    4: "Tốt nghiệp",
  }

  layTrinhDo(key: number) {
    return this.layTrinhDoHocVan[key] || "Không có thông tin";
  }

  uvDangChon: any = null;
  themThongTinPhongVan = false;

  thoiGianPhongVan: string = '';
  diaDiemPhongVan: string = '';
  noiDungPhongVan: string = '';
  trang_thai: string = '';

  moFormUV(uv: any) {
    this.uvDangChon = {
      ...uv,
      safeCvUrl: this.sanitizer.bypassSecurityTrustResourceUrl(uv.cvUrl)
    };
    this.themThongTinPhongVan = false;
  }

  dongForm() {
    this.uvDangChon = null;
    this.themThongTinPhongVan = false;
  }

  b1ChapNhan() {
    this.themThongTinPhongVan = true;
  }

  huyMoiPhongVan() {
    this.themThongTinPhongVan = false
  }

  guiMoiPhongVan() {
    const thong_tin_phong_van = {
      ma_viec: this.uvDangChon.ma_viec,
      ma_cong_ty: this.thongTin.thong_tin_chi_tiet.ma_cong_ty,
      ma_nguoi_tim_viec: this.uvDangChon.ma_nguoi_tim_viec,
      thoi_gian: new Date(this.thoiGianPhongVan).toISOString(),
      dia_diem: this.diaDiemPhongVan,
      noi_dung: this.noiDungPhongVan,
      trang_thai: "chap_Nhan",
      trang_thai_duyet: "da_Duyet"
    };

    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/guiThuMoiPhongVan',  thong_tin_phong_van )
      .subscribe(data => {
        if (data.success) {
          this.pop_up_moi_phong_van_thanh_cong = true;
          setTimeout(() => {
            this.pop_up_moi_phong_van_thanh_cong = false;
          }, 2000);
        }
        else {
          this.pop_up_moi_phong_van_that_bai = true;
          setTimeout(() => {
            this.pop_up_moi_phong_van_that_bai = false;
          }, 2000)
        }
        this.cd.detectChanges();
      });
  }

  tuChoiUV() {
    const thong_tin = {
      ma_viec: this.uvDangChon.ma_viec,
      ma_cong_ty: this.thongTin.thong_tin_chi_tiet.ma_cong_ty,
      ma_nguoi_tim_viec: this.uvDangChon.ma_nguoi_tim_viec
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/tuChoiUngVien', thong_tin)
      .subscribe(data => {
        console.log(data.message);
        if (data.success) {
          alert("tu choi ung vien thanh cong");
        }
        else{
          this.pop_up_tu_choi_that_bai = true;
          this.cd.detectChanges();
          setTimeout(() => {
            this.pop_up_tu_choi_that_bai = false;
            this.cd.detectChanges();
          },2000)
          this.dongForm();
        }
        this.cd.markForCheck();
      });
    this.dongForm();
  }

  moXacNhanXoa(uv: any, index: number) {
    this.uvMuonXoa = uv;
    this.indexMuonXoa = index;
    this.showXacNhanXoa = true;
  }

  dongXacNhanXoa() {
    this.showXacNhanXoa = false;
    this.uvMuonXoa = null;
    this.indexMuonXoa = null;
  }

  moCV(url: string){
    url = "http://localhost:7000/" + url;
    window.open(url);
  }

  xoaUngVien() {
    if (!this.uvMuonXoa || this.indexMuonXoa === null) return;

    const thong_tin_xoa = {
      ma_cong_ty: this.thongTin.thong_tin_chi_tiet.ma_cong_ty,
      ma_nguoi_tim_viec: this.uvMuonXoa.ma_nguoi_tim_viec,
      ma_viec: this.uvMuonXoa.ma_viec
    }

    this.httpclient.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/xoaUngVien',
       thong_tin_xoa 
    ).subscribe({
      next: (data) => {
        if (data.success) {
          this.danh_sach_ung_vien.splice(this.indexMuonXoa!, 1);
          this.pop_up_xoa_thanh_cong = true;
          setTimeout(() => { this.pop_up_xoa_thanh_cong = false; this.cd.detectChanges(); }, 1500);
        } else {
          this.pop_up_xoa_that_bai = true;
          setTimeout(() => { this.pop_up_xoa_that_bai = false; this.cd.detectChanges(); }, 1500);
        }
        this.cd.detectChanges();
        this.dongXacNhanXoa();
      },
      error: () => {
        this.pop_up_xoa_that_bai = true;
        setTimeout(() => { this.pop_up_xoa_that_bai = false; this.cd.detectChanges(); }, 1500);
        this.cd.detectChanges();
        this.dongXacNhanXoa();
      }
    });
  }
}
