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

  dangNhap(user: any): void {
    try {
      this.cookieService.delete(this.USER_KEY);

      this.cookieService.delete(this.USER_KEY, '/');

      this.cookieService.deleteAll('/');

      const userJson = JSON.stringify(user);
      this.cookieService.set(this.USER_KEY, userJson, 7, '/');
    } catch (e) {
      console.error('Không thể lưu thông tin người dùng:', e);
    }
  }

  dangXuat(): void {
    this.cookieService.delete(this.USER_KEY);

    this.cookieService.delete(this.USER_KEY, '/');

    this.cookieService.deleteAll('/');
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
