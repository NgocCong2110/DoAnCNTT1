import { Component } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { CommonModule, JsonPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-trang-gui-thong-bao',
  imports: [CommonModule, FormsModule],
  templateUrl: './trang-gui-thong-bao.html',
  styleUrl: './trang-gui-thong-bao.css'
})
export class TrangGuiThongBao {
  constructor(public auth: Auth) {}
  pop_up_them = false;
  pop_up_them_thanh_cong = false;
  pop_up_them_that_bai = false;

  dongPopUp() {
    this.pop_up_them = false;
  }

  moPopUpThemThongBao(){
    this.pop_up_them = true;
  }

  du_lieu_thong_bao_server = {
    tieu_de: '',
    noi_dung: '',
    loai_thong_bao: 'toan_Server',
    ma_nguoi_nhan: null
  };

  async guiThongBao(){
    const response = await fetch("http://65001/api/API_WEB/guiThongBaoMoi", {
      method: "POST",
      headers: {
        "Content-Type" : "application/json"
      },
      body: JSON.stringify({du_lieu_thong_bao_server: this.du_lieu_thong_bao_server})
    })
    const data = await response.json();
    if(data.success){
      this.pop_up_them_thanh_cong = true;
      setTimeout(() => {
        this.pop_up_them_thanh_cong = false;
      },2000)
    }
    else{
      this.pop_up_them_that_bai = true;
      setTimeout(() => {
        this.pop_up_them_that_bai = false;
      },2000)
    }
  }
}
