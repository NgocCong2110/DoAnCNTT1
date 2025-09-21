import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class Auth {
  private readonly USER_KEY = 'thongTin_DangNhap';

  constructor(private cookieService: CookieService) { }

  kiemTraDangNhap(): boolean {
    return this.cookieService.check(this.USER_KEY);
  }

  dangNhap(thong_tin: any): void {
    try {
      this.dangXuat();
      const nguoi_dung_json = JSON.stringify(thong_tin);
      this.cookieService.set(this.USER_KEY, nguoi_dung_json, 7, '/');
    } catch (e) {
      console.error('Không thể lưu thông tin người dùng:', e);
    }
  }

  dangXuat(): void {
    const cookies = document.cookie.split(';');

    cookies.forEach(cookie => {
      const eqPos = cookie.indexOf('=');
      const name = eqPos > -1 ? cookie.substr(0, eqPos).trim() : cookie.trim();

      this.cookieService.delete(name, '/');
      this.cookieService.delete(name, '');
    });
  }

  layThongTinNguoiDung(): any | null {
    if (!this.cookieService.check(this.USER_KEY)) return null;
    try {
      return JSON.parse(this.cookieService.get(this.USER_KEY));
    } catch (e) {
      console.error('Không thể parse thông tin người dùng:', e);
      return null;
    }
  }
}
