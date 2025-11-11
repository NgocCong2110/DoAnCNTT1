import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { HttpClient } from '@angular/common/http';
import { BaseChartDirective } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  so_luong: { thang: number; so_luong: number }[];
}

interface tin_tuyen_dung_moi{
  ma_bai_dang: number;
  tieu_de: string;
  muc_luong: string;
  ngay_tao: string;
  ten_cong_ty: string;
}

@Component({
  selector: 'app-component-thong-ke-tin-tuyen-dung',
  standalone: true,
  imports: [BaseChartDirective, CommonModule, RouterModule],
  templateUrl: './component-thong-ke-tin-tuyen-dung.html',
  styleUrls: ['./component-thong-ke-tin-tuyen-dung.css']
})
export class ComponentThongKeTinTuyenDung implements OnInit {
  @ViewChild('lineChart') lineChart?: BaseChartDirective;

  tang_truong: number = 0;
  tin_tuyen_dung_hang_ngay: number = 0;
  danh_sach_tin_moi: any;

  constructor(
    private httpclient: HttpClient,
    private cd: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.laySoLuongTinTuyenDung();
    this.layDanhSachTinTuyenDungMoi();
  }

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Xu hướng tin tuyển dụng',
      fill: true,
      tension: 0.4,
      backgroundColor: 'rgba(16, 185, 129, 0.1)',
      borderColor: 'rgba(16, 185, 129, 1)',
      borderWidth: 3,
      pointRadius: 5,
      pointHoverRadius: 7,
      pointBackgroundColor: 'rgba(16, 185, 129, 1)',
      pointBorderColor: '#fff',
      pointBorderWidth: 2
    }]
  };

  lineChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false
      },
      title: {
        display: true,
        text: 'Biểu đồ đường - Xu hướng tăng trưởng',
        font: { size: 16, weight: 'bold' },
        color: '#1f2937',
        padding: { bottom: 20 }
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        padding: 12,
        cornerRadius: 8,
        titleFont: { size: 14 },
        bodyFont: { size: 13 }
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        grid: {
          color: 'rgba(0, 0, 0, 0.05)'
        },
        ticks: {
          font: { size: 12 },
          color: '#6b7280'
        }
      },
      x: {
        grid: { display: false },
        ticks: {
          font: { size: 12 },
          color: '#6b7280'
        }
      }
    }
  };

  laySoLuongTinTuyenDung() {
    this.httpclient.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/laySoLuongTinTuyenDung',
      {}
    ).subscribe({
      next: (data) => {
        if (data?.success && Array.isArray(data.so_luong) && data.so_luong.length > 0) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong);

          this.lineChartData = {
            labels,
            datasets: [{
              ...this.lineChartData.datasets[0],
              data: values
            }]
          };

          if (values.length >= 2) {
            const thangTruoc = values[values.length - 2];
            const thangNay = values[values.length - 1];
            this.tang_truong = thangTruoc > 0 
              ? Math.round(((thangNay - thangTruoc) / thangTruoc) * 100)
              : 0;
          }

          this.cd.detectChanges();
          setTimeout(() => this.lineChart?.update(), 0);
        }
        this.cd.markForCheck();
      },
      error: (err) => {
        console.error('Lỗi khi lấy dữ liệu tin tuyển dụng:', err);
      }
    });
  }
  layDanhSachTinTuyenDungMoi() {
    this.httpclient.post<any>(
      'http://localhost:7000/api/API_WEB/layDanhSachTinTuyenDungMoi',
      {}
    ).subscribe({
      next: (data) => {
        if (data?.success && Array.isArray(data.danh_sach)) {
          this.danh_sach_tin_moi = data.danh_sach.map((item: any) => ({
            ma_bai_dang: item.viec_Lam?.ma_bai_dang,
            tieu_de: item.viec_Lam?.tieu_de || '(Không có tiêu đề)',
            muc_luong: item.viec_Lam?.muc_luong || 'Thỏa thuận',
            ngay_tao: new Date(item.viec_Lam?.ngay_dang).toLocaleDateString('vi-VN'),
            ten_cong_ty: item.cong_Ty?.ten_cong_ty || '(Không có tên công ty)'
          }));
          this.cd.detectChanges();
        }
        this.cd.markForCheck();
      },
      error: (err) => console.error('Lỗi khi lấy danh sách tin mới:', err)
    });
  }

  xemTinTuyenDung(tin_moi: any){
    const ma_bai_dang = tin_moi.ma_bai_dang;
    this.router.navigate(['trang-chi-tiet-viec-lam'], {queryParams : {ma_bai_dang}});
  }
}
