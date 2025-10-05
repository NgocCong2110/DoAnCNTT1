import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-trang-ket-qua-thanh-toan',
  imports: [CommonModule],
  templateUrl: './trang-ket-qua-thanh-toan.html',
  styleUrl: './trang-ket-qua-thanh-toan.css'
})
export class TrangKetQuaThanhToan implements OnInit{
  success: boolean = false;
  maDonHang: string | null = null;
  errorCode: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.success = params['success'] === 'true';
      this.maDonHang = params['ma_don_hang'] ?? null;
      this.errorCode = params['error'] ?? null;
    });
  }
}
