import { Component, OnInit, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface API_RESPONSE {
  success: boolean;
  so_luong: { thang: number; so_luong_nguoi_dung: number }[];
}

@Component({
  selector: 'app-component-thong-ke-nguoi-dung',
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './component-thong-ke-nguoi-dung.html',
  styleUrl: './component-thong-ke-nguoi-dung.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ComponentThongKeNguoiDung implements OnInit{
  @ViewChild('barChart') barChart?: BaseChartDirective;
  @ViewChild('lineChart') lineChart?: BaseChartDirective;

  tongNguoiDung: number = 0;
  nguoiDungThangNay: number = 0;
  tangTruong: number = 0;
  trungBinh: number = 0;

  constructor(
    public http: HttpClient,
    public cd: ChangeDetectorRef
  ) {}

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Số lượng người dùng',
      backgroundColor: 'rgba(59, 130, 246, 0.8)',
      borderColor: 'rgba(59, 130, 246, 1)',
      borderWidth: 2,
      borderRadius: 8,
      barThickness: 40
    }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Xu hướng người dùng',
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

  barChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false
      },
      title: {
        display: true,
        text: 'Biểu đồ cột - Số lượng người dùng',
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
        grid: {
          display: false
        },
        ticks: {
          font: { size: 12 },
          color: '#6b7280'
        }
      }
    }
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
        grid: {
          display: false
        },
        ticks: {
          font: { size: 12 },
          color: '#6b7280'
        }
      }
    }
  };

  laySoLieuNguoiDung() {
    this.http.post<API_RESPONSE>(
      'http://localhost:7000/api/API_WEB/laySoLuongNguoiDung',
      {}
    ).subscribe({
      next: (data) => {
        if (data?.success && Array.isArray(data.so_luong) && data.so_luong.length > 0) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong_nguoi_dung);

          this.barChartData = {
            labels: [...labels],
            datasets: [{
              ...this.barChartData.datasets[0],
              data: [...values]
            }]
          };

          this.lineChartData = {
            labels: [...labels],
            datasets: [{
              ...this.lineChartData.datasets[0],
              data: [...values]
            }]
          };

          this.tongNguoiDung = values.reduce((a, b) => a + b, 0);
          this.nguoiDungThangNay = values[values.length - 1] || 0;
          this.trungBinh = Math.round(this.tongNguoiDung / values.length);
          
          if (values.length >= 2) {
            const thangTruoc = values[values.length - 2];
            this.tangTruong = thangTruoc > 0 
              ? Math.round(((this.nguoiDungThangNay - thangTruoc) / thangTruoc) * 100)
              : 0;
          }

          this.cd.detectChanges();

          setTimeout(() => {
            this.barChart?.update();
            this.lineChart?.update();
            this.cd.detectChanges();
          },0);
        }
        this.cd.markForCheck();
      },
      error: (err) => {
        console.error('Lỗi khi lấy dữ liệu người dùng:', err);
      }
    });
  }

  ngOnInit(): void {
    this.laySoLieuNguoiDung();
  }
}
