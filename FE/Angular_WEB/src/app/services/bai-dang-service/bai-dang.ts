import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BaiDangComponent } from '../../Trang_WEB/trang-chu/component-trang-chu/thong-tin-noi-bat/bai-dang.model';
import { HttpClient } from '@angular/common/http';


@Injectable({ providedIn: 'root' })
export class BaiDang {
  private apiUrl = 'http://localhost:65001/api/API_WEB/layDanhSachBaiDang';

  private bai_dang_duoc_chon = new BehaviorSubject<BaiDangComponent | null>(null);
  bai_dang_duoc_chon$ = this.bai_dang_duoc_chon.asObservable();

  private danhSachSubject = new BehaviorSubject<BaiDangComponent[]>([]);
  danhSach$ = this.danhSachSubject.asObservable();

  private daLoad = false;

  async layDanhSachBaiDang() {
    if (this.daLoad && this.danhSachSubject.value.length > 0) {
      return { danh_sach_bai_dang: this.danhSachSubject.value };
    }

    try {
      const response = await fetch(this.apiUrl, {
        method: 'POST', 
        headers: { 'Content-Type': 'application/json' }
      });
      if (!response.ok) throw new Error('Lỗi HTTP ' + response.status);

      const data = await response.json();
      const ds = data.danh_sach_bai_dang ?? [];

      this.danhSachSubject.next(ds);
      this.daLoad = true;

      return { danh_sach_bai_dang: ds };
    } catch (err) {
      console.error('Lỗi fetch:', err);
      throw err;
    }
  }

  chonBaiDang(baiDang: BaiDangComponent | null) {
    this.bai_dang_duoc_chon.next(baiDang);
  }
}
