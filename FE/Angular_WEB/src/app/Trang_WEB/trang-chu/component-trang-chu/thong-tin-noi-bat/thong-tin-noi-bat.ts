import { Component } from '@angular/core';
import { BangXepHang } from './bang-xep-hang/bang-xep-hang';
import { TinTuyenDung } from './tin-tuyen-dung/tin-tuyen-dung';

@Component({
  selector: 'app-thong-tin-noi-bat',
  imports: [BangXepHang, TinTuyenDung],
  templateUrl: './thong-tin-noi-bat.html',
  styleUrls: ['./thong-tin-noi-bat.css']
})
export class ThongTinNoiBat {

}
