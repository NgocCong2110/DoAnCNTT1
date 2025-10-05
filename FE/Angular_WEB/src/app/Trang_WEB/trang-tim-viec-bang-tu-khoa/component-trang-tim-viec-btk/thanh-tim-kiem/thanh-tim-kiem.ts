import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThongTinViecLam } from '../../../../services/thong-tin-viec-lam-service/thong-tin-viec-lam';

interface API_RESPONSE{
  success: boolean;
  thong_tin: any;
  message: string;
}

@Component({
  selector: 'app-thanh-tim-kiem',
  imports: [FormsModule, CommonModule],
  templateUrl: './thanh-tim-kiem.html',
  styleUrl: './thanh-tim-kiem.css'
})

export class ThanhTimKiem {

  du_lieu = false;

  pop_up_lay_thong_tin_that_bai = false;

  constructor(public httpclient: HttpClient, public cd: ChangeDetectorRef, public vl: ThongTinViecLam) {}

  tu_khoa_tim_kiem = "";
  thongTinTim(){
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/duaRaDeXuat', JSON.stringify(this.tu_khoa_tim_kiem),
      {headers: {"Content-Type" : "application/json"}})
      .subscribe({
        next: (data) => {
          if(data.success){
            this.du_lieu = true;
            this.vl.capNhatDuLieu(data.thong_tin);
          }
          else{
            this.pop_up_lay_thong_tin_that_bai = true;
          }
          this.cd.detectChanges();
        },
        error: (err) => {
          this.pop_up_lay_thong_tin_that_bai = true;
          this.cd.detectChanges();
        }
      })
  }
}
