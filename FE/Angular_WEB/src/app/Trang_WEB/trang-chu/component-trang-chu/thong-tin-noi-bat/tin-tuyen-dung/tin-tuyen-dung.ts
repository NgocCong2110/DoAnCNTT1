import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../../../../services/auth';
import { BaiDang } from '../../../../../services/bai-dang-service/bai-dang';
import { Subscription } from 'rxjs';
import { BaiDangComponent } from '../bai-dang.model';

@Component({
  selector: 'app-tin-tuyen-dung',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tin-tuyen-dung.html',
  styleUrls: ['./tin-tuyen-dung.css']
})
export class TinTuyenDung implements OnInit, OnDestroy {
  bai_dang_duoc_chon: BaiDangComponent | null = null;
  private sub?: Subscription;
  pop_up_bao_cao: boolean = false;
  noi_dung_bao_cao: string = "";
  thongTin: any;
  pop_up_ung_tuyen = false;

  constructor(public baiDangService: BaiDang, public auth: Auth) { 
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.sub = this.baiDangService.bai_dang_duoc_chon$.subscribe(bai_dang => {
      this.bai_dang_duoc_chon = bai_dang;
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
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

  // huyLuaChon() {
  //   this.baiDangService.chonBaiDang(null);
  // }

  themBaiDang(a: string, b: string) {
    console.log("skibidi");
  }

  async baoCaoBaiDang(){
    const ma_Nguoi_Bao_Cao = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;
    const response = await fetch("http://localhost:65001/api/API_WEB/baoCaoBaiDang", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        ma_bai_dang: this.bai_dang_duoc_chon?.ma_bai_dang,
        ten_nguoi_dang: this.bai_dang_duoc_chon?.ten_nguoi_dang,
        tieu_de: this.bai_dang_duoc_chon?.tieu_de,
        noi_dung: this.bai_dang_duoc_chon?.noi_dung,
        ma_nguoi_bao_cao: ma_Nguoi_Bao_Cao,
        noi_dung_bao_cao: this.noi_dung_bao_cao,
        ngay_bao_cao: Date.now,
      })
    });
    const data = await response.json();
    if (data.success) {
      alert("Báo cáo bài đăng thành công.");
      this.pop_up_bao_cao = false;
      this.noi_dung_bao_cao = '';
    } else {
      alert("Báo cáo bài đăng thất bại. Vui lòng thử lại.");
    }
  }

  async luuBaiDang(){
    const ma_Nguoi_Luu = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;
    const response = await fetch("http://localhost:65001/api/API_WEB/luuBaiDang", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        ma_bai_dang: this.bai_dang_duoc_chon?.ma_bai_dang,
        ma_nguoi_luu: ma_Nguoi_Luu,
      })
    });
  }

  async ungTuyenCongViec(){
    const ma_Nguoi_Ung_Tuyen = this.thongTin?.thong_tin_chi_tiet?.ma_nguoi_dung;
    const response = await fetch("http://localhost:65001/api/API_WEB/ungTuyenCongViec", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        ma_viec: this.bai_dang_duoc_chon?.viec_lam?.ma_viec,
        ma_cong_ty: this.bai_dang_duoc_chon?.ma_nguoi_dang,
        ma_nguoi_tim_viec: ma_Nguoi_Ung_Tuyen,
      })
    });
    const data = await response.json();
    if(data.success){
      this.pop_up_ung_tuyen = true;
      setTimeout(() => {
        this.pop_up_ung_tuyen = false;
      },2000)
    }
  }
  moPopUpBaoCao(){
    this.pop_up_bao_cao = true;
    this.noi_dung_bao_cao = "";
  }

  huyBaoCao() {
  this.pop_up_bao_cao = false;
  this.noi_dung_bao_cao = '';
}
}
