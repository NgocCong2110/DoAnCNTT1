import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
}

@Component({
  selector: 'app-trang-gui-thong-bao',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-gui-thong-bao.html',
  styleUrls: ['./trang-gui-thong-bao.css']
})
export class TrangGuiThongBao implements OnInit {
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

  du_lieu_thong_bao_server: any = {
    tieu_de: '',
    noi_dung: '',
    loai_thong_bao: 1,
    ma_quan_tri: null
  };

  ngOnInit(): void {
    const thong_tin = this.auth.layThongTinNguoiDung();
    if(thong_tin){
      this.du_lieu_thong_bao_server.ma_quan_tri = thong_tin.ma_quan_tri;
    }
  }

  guiThongBao() {
    const thong_tin = this.du_lieu_thong_bao_server;
    this.http.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/guiThongBaoToiServer', thong_tin).subscribe({
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
