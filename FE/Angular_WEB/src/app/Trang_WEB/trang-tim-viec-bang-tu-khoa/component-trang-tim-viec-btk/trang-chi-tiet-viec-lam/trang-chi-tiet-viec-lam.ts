import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { HeaderWEB } from '../../../Component/header-web/header-web';
import { ChangeDetectorRef } from '@angular/core';

interface API_RESPONSE {
  success: boolean;
  chi_tiet: any; 
}

@Component({
  selector: 'app-trang-chi-tiet-viec-lam',
  imports: [CommonModule, HeaderWEB],
  templateUrl: './trang-chi-tiet-viec-lam.html',
  styleUrls: ['./trang-chi-tiet-viec-lam.css'] 
})
export class TrangChiTietViecLam {
  chi_tiet: any;

  constructor(
    private route: ActivatedRoute,
    private httpclient: HttpClient,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const ma_bai_dang = Number(this.route.snapshot.paramMap.get('ma_bai_dang'));
    this.layChiTiet(ma_bai_dang);
  }

  layChiTiet(ma_bai_dang: number) {
    this.httpclient
      .post<API_RESPONSE>('http://localhost:65001/api/API_WEB/layChiTietViecLam', {ma_bai_dang})
      .subscribe({
        next: (data) => {
          this.chi_tiet = data.chi_tiet;  
          this.cd.detectChanges();
        },
        error: (err) => {
          console.error(err);
        },
      });
  }

  loaiHinhMap: {[key:number]: string} = {
    1: "Toàn thời gian",
    2: "Bán thời gian",
    3: "Làm việc tự do"
  }

  layLoaiHinh(key: number){
    return this.loaiHinhMap[key] || 'Không xác định';
  }
}