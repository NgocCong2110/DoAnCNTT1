import { Component } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE{
  success: boolean;
}

@Component({
  selector: 'app-trang-thong-bao-cong-ty',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-thong-bao-cong-ty.html',
  styleUrl: './trang-thong-bao-cong-ty.css'
})

export class TrangThongBaoCongTy {
  constructor(
      public auth: Auth,
      public http: HttpClient
    ) {}
  
    pop_up_them = false;
    pop_up_them_thanh_cong = false;
    pop_up_them_that_bai = false;
  
    dongPopUp() {
      this.pop_up_them = false;
    }
  
    moPopUpThemThongBao() {
      this.pop_up_them = true;
    }
  
    du_lieu_thong_bao_server = {
      tieu_de: '',
      noi_dung: '',
      loai_thong_bao: 'toan_Server',
      ma_nguoi_nhan: null
    };
  
    guiThongBao() {
      this.http.post<API_RESPONSE>(
        'http://localhost:65001/api/API_WEB/guiThongBaoMoi',
        { du_lieu_thong_bao_server: this.du_lieu_thong_bao_server }
      ).subscribe({
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
}
