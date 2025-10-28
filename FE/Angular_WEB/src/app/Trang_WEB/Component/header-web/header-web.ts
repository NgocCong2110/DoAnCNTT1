import { Component, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { Auth } from '../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { debounceTime } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-header-web',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './header-web.html',
  styleUrls: ['./header-web.css']
})
export class HeaderWEB implements OnInit {
  nguoi_Dung: any;
  goiY: any;
  item_chon: any;
  tu_khoa = "";
  focus = false;

    du_lieu_mac_dinh = [
    { ten_label: 'Công nghệ thông tin', ten: 'cong_nghe_thong_tin', loai: 'viec_lam', logo: '' },
    { ten_label: 'Marketing', ten: 'marketing', loai: 'viec_lam', logo: '' },
    { ten_label: 'Sales', ten: 'sales', loai: 'viec_lam', logo: '' },
  ];
  
  //subject la mot doi tuong Observable (có thể subscribe) vừa là Observer (có thể next(value) để phát giá trị).
  //subscribe la kenh nhan du lieu 
  //gia tri duoc gui bang next value
  private tu_khoa_sub = new Subject<string>();

  constructor(public auth: Auth, public router: Router, private httpclient: HttpClient, public cdr: ChangeDetectorRef) {
    this.tu_khoa_sub.pipe(debounceTime(300)).subscribe(tu_khoa => {
      if(tu_khoa.trim()){
        this.httpclient.post<any>('http://localhost:65001/api/API_WEB/goiYTuKhoa', `"${this.tu_khoa}"`, { headers: { 'Content-Type': 'application/json' } })
          .subscribe({
            next: (data) => {
              this.goiY = data.danh_sach;
              this.cdr.markForCheck();
            }
          })
      }
      else{
        this.goiY = this.du_lieu_mac_dinh;
        if(this.focus){
          this.goiY = this.du_lieu_mac_dinh;
        }
      }
    })
  }

  layGoiY(){
    this.tu_khoa_sub.next(this.tu_khoa);
  }

  layDuLieuMacDinh(){
    this.goiY = this.du_lieu_mac_dinh;
  }



  chonGoiY(thong_tin_chon: string){

    this.item_chon = thong_tin_chon;
    if(this.item_chon.loai == 'viec_lam'){
      this.router.navigate(['trang-tim-viec-theo-tu-khoa'], {
        queryParams: { nganh: this.item_chon.ten }
      })
    }
    this.tu_khoa = thong_tin_chon;
    this.goiY = [];
  }

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    }
  }

  focusVao(){
    this.focus = true
    if(!this.tu_khoa.trim()){
      this.goiY = this.du_lieu_mac_dinh;
    }
  }

  roiKhoi(){
    this.focus = false;
    this.cdr.markForCheck();
    setTimeout(() =>{
      this.goiY = []
    },1000);
  }

  vaiTro() {
    this.nguoi_Dung = this.auth.layThongTinNguoiDung();
    let kieu_nguoi_dung = this.nguoi_Dung?.kieu_nguoi_dung;

    if (kieu_nguoi_dung == "quan_Tri_Vien") {
      this.router.navigate(['/trang-quan-tri']);
    } else if (kieu_nguoi_dung == "nguoi_Tim_Viec") {
      this.router.navigate(['/trang-nguoi-tim-viec']);
    } else if (kieu_nguoi_dung == "cong_Ty") {
      this.router.navigate(['/trang-cong-ty']);
    }
  }

  dangXuat() {
    this.auth.dangXuat();
    setTimeout(() => {
      this.router.navigate(['/dang-nhap']);
    }, 1500);
  }

  
}
