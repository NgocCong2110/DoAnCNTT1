import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-trang-danh-sach-vi-pham',
  imports: [CommonModule],
  templateUrl: './trang-danh-sach-vi-pham.html',
  styleUrl: './trang-danh-sach-vi-pham.css'
})
export class TrangDanhSachViPham implements OnInit {
  danh_sach_vi_pham: any[] = [];
  constructor(private http: HttpClient, private cd: ChangeDetectorRef) {}
  ngOnInit() {
    return this.layDanhSachViPham();
  }
  layDanhSachViPham() {
    this.http.post('http://localhost:65001/api/API_WEB/layDanhSachViPham', {}).subscribe({
      next: (data: any) => {
        this.danh_sach_vi_pham = data.danh_sach;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
