import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';

interface API_RESPONSE{
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-the-viec-lam-duoc-quan-tam',
  imports: [CommonModule, FormsModule],
  templateUrl: './the-viec-lam-duoc-quan-tam.html',
  styleUrl: './the-viec-lam-duoc-quan-tam.css'
})
export class TheViecLamDuocQuanTam implements OnInit{

  danh_sach_viec_lam_qt: any;

  constructor(private httpclient: HttpClient, private cd: ChangeDetectorRef, private router: Router) { }

  ngOnInit(): void {
    this.layDanhSachViecLamDuocQuanTam();
  }

  layDanhSachViecLamDuocQuanTam(){
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layDanhSachViecLamDuocQuanTam',{})
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_viec_lam_qt = data.danh_sach;
            this.cd.markForCheck();
          }
        },
        error: (err) => {

        }
      })
  }

  xemViecLamNoiBat(ma_bai_dang: number){
    this.router.navigate(['trang-chi-tiet-viec-lam'], {
      queryParams : { ma_bai_dang }
    })
  }  

  taoDuongDanLogo(url : string): string{
    if(!url) return "";
    
    if(!url.startsWith('http')){
      return `http://localhost:65001/${url}`;
    }
    return url;
  }
}
