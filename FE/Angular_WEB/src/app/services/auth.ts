import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Auth {
  constructor(){}
  kiemTraDangNhap(): boolean {
    if (typeof window !== 'undefined') {
      return !!localStorage.getItem('thongTin_NguoiDung'); 
    }
    return false;
  }
  dangNhap(user: any){
    if (typeof window !== 'undefined') {
      localStorage.setItem('thongTin_NguoiDung', JSON.stringify(user));
    }
  }
  dangXuat(){
    if (typeof window !== 'undefined') {
      localStorage.removeItem('thongTin_NguoiDung');
    }
  }
  layThongTinNguoiDung(){
    if (typeof window !== 'undefined') {
      const data = localStorage.getItem('thongTin_NguoiDung');
      return data ? JSON.parse(data) : null;
    }
    return null;
  }
}
