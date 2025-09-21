import { Component, OnInit } from '@angular/core';
import { Auth } from '../../../../../../services/auth';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-trang-thong-tin-tai-khoan',
  imports: [FormsModule, CommonModule, DatePipe],
  templateUrl: './trang-thong-tin-tai-khoan.html',
  styleUrl: './trang-thong-tin-tai-khoan.css'
})
export class TrangThongTinTaiKhoan implements OnInit {

  constructor(private auth: Auth) { }

  thongTin: any;
  formDangMo = false;
  duLieuSua = "";
  giaTriMoi: any;


  ngOnInit(): void {
    this.thongTin = {
      thongTin_NguoiDung: this.auth.layThongTinNguoiDung()
    };
  }

  moForm(duLieu: string){
    this.duLieuSua = duLieu
    this.giaTriMoi = this.thongTin.thongTin_NguoiDung[duLieu];
    this.formDangMo = true
  }

  dongForm(){
    this.formDangMo = false;
    this.duLieuSua = "";
    this.giaTriMoi = "";
  }

  luuForm(){
    this.thongTin.thongTin_NguoiDung[this.duLieuSua] = this.giaTriMoi;

    this.luuDuLieuMoi( this.giaTriMoi );

    this.dongForm();

  }

  async luuDuLieuMoi(thongTin : any){
    const response = await fetch("http://localhost:65001/api/API_WEB/luuDuLieuMoi",{
      method : "POST",
      headers : {
        "Content-Type" : "application/json"
      },
      body: JSON.stringify(thongTin)
    });
    const data = await response.json();
    if(data.success){

      //test chuc nang

      console.log("Them du lieu thanh cong")
    }
    else{
      console.log("them du lieu khong thanh cong")
    }
  }

}
