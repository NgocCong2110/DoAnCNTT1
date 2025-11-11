import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Auth } from '../../../../../../services/auth';
import { DecimalPipe } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface API_RESPONSE{
  success: boolean;
  danh_sach: any;
}

@Component({
  selector: 'app-trang-danh-sach-goi-dich-vu-quan-tri',
  imports: [CommonModule, DecimalPipe],
  templateUrl: './trang-danh-sach-goi-dich-vu-quan-tri.html',
  styleUrl: './trang-danh-sach-goi-dich-vu-quan-tri.css'
})
export class TrangDanhSachGoiDichVuQuanTri implements OnInit{

  danh_sach_dich_vu: any[] = [];
  trangThai = false;
  thongTin: any = null;

  constructor(public auth: Auth, public httpclient: HttpClient, public cd: ChangeDetectorRef ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.layDanhSachDichVu();
  }

  layDanhSachDichVu(){
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/layDanhSachDichVu',  {}) // gui string, int,... khong boc
      .subscribe({
        next: (data) => {
          if(data.success){
            this.danh_sach_dich_vu = [];
            this.danh_sach_dich_vu = data.danh_sach;
            this.cd.detectChanges();
          }
          else{
            console.log("loi dich vu")
          }
        },
        error: (err) => {
          this.danh_sach_dich_vu = [];
          console.log(err)
          this.cd.detectChanges();
        }
      })
  }
}
