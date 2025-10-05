import { Component, ViewChild } from '@angular/core';
import { TinTuyenDung } from './tin-tuyen-dung/tin-tuyen-dung';
import { DanhSachBaiDang } from './danh-sach-bai-dang/danh-sach-bai-dang';
import { DangBai } from './dang-bai/dang-bai';
import { Auth } from '../../../../services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-thong-tin-noi-bat',
  imports: [DanhSachBaiDang, TinTuyenDung, DangBai, CommonModule, FormsModule],
  templateUrl: './thong-tin-noi-bat.html',
  styleUrls: ['./thong-tin-noi-bat.css']
})
export class ThongTinNoiBat {
  constructor(public auth: Auth) {}
  
}
