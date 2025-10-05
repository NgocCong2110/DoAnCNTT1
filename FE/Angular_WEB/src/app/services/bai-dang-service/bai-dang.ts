import { Injectable } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
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

  constructor(private http: HttpClient) { }

  async layDanhSachBaiDang() {
    if (this.daLoad && this.danhSachSubject.value.length > 0) {
      return { danh_sach: this.danhSachSubject.value };
    }

    try {
      const data: any = await firstValueFrom(
        this.http.post(this.apiUrl, {}, { headers: { 'Content-Type': 'application/json' } })
      );

      const ds = data?.danh_sach ?? [];

      this.danhSachSubject.next(ds);
      this.daLoad = true;

      return { danh_sach: ds };
    } catch (err: any) {
      console.error('Lỗi API:', err);
      throw err;
    }
  }

  async xoaBaiDang(ma_bai_dang: number) {
    try {
      const result: any = await firstValueFrom(
        this.http.post(
          'http://localhost:65001/api/API_WEB/xoaBaiDang',
          { ma_bai_dang },
          { headers: { 'Content-Type': 'application/json' } }
        )
      );

      if (result?.success) {
        this.danhSachSubject.next([]);
        this.daLoad = false;

        await this.layDanhSachBaiDang();
      }

      return result;
    } catch (err) {
      console.error('Lỗi xoá bài đăng:', err);
      throw err;
    }
  }


  chonBaiDang(baiDang: BaiDangComponent | null) {
    this.bai_dang_duoc_chon.next(baiDang);
  }
}
