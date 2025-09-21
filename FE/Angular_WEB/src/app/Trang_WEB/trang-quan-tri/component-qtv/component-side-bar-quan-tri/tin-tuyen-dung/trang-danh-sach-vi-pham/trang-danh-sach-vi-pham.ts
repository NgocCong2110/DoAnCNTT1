import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-trang-danh-sach-vi-pham',
  imports: [CommonModule],
  templateUrl: './trang-danh-sach-vi-pham.html',
  styleUrl: './trang-danh-sach-vi-pham.css'
})
export class TrangDanhSachViPham implements OnInit {
  danh_sach_vi_pham: any[] = [];
  ngOnInit() {
    return this.layDanhSachViPham();
  }
  async layDanhSachViPham() {
    const response = await fetch('http://localhost:65001/api/API_WEB/layDanhSachViPham', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    });
    const data = await response.json();
    this.danh_sach_vi_pham = data.danh_sach;
  }
}
