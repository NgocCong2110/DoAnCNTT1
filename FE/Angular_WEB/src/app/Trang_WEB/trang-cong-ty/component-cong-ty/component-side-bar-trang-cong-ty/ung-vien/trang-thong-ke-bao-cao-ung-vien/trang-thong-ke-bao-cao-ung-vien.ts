import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface API_RESPONSE {
  success: boolean;
  so_luong: { thang: number; so_luong_ung_vien: number }[];
}

@Component({
  selector: 'app-trang-thong-ke-bao-cao-ung-vien',
  standalone: true,
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './trang-thong-ke-bao-cao-ung-vien.html',
  styleUrls: ['./trang-thong-ke-bao-cao-ung-vien.css']
})
export class TrangThongKeBaoCaoUngVien implements OnInit {
  thongTin: any;
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  // Stats cards data
  tongUngVien: number = 0;
  ungVienThangNay: number = 0;
  tangTruong: number = 0;
  trungBinh: number = 0;

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Số lượng ứng viên',
      backgroundColor: 'rgba(99, 102, 241, 0.8)',
      borderColor: 'rgba(99, 102, 241, 1)',
      borderWidth: 2,
      borderRadius: 8,
      barThickness: 40
    }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Xu hướng ứng viên',
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
        text: 'Biểu đồ cột - Số lượng ứng viên',
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

  constructor(
    public auth: Auth,
    public http: HttpClient,
    public cd: ChangeDetectorRef
  ) {
    this.thongTin = this.auth.layThongTinNguoiDung();
  }

  ngOnInit(): void {
    this.laySoLieuUngVien();
  }

  laySoLieuUngVien() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;

    this.http.post<API_RESPONSE>(
      'http://localhost:65001/api/API_WEB/laySoLuongUngVien',
      { ma_cong_ty: ma_Cong_Ty }
    ).subscribe({
      next: (data) => {
        if (data.success && data.so_luong.length > 0) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong_ung_vien);

          // Update charts
          this.barChartData.labels = labels;
          this.barChartData.datasets[0].data = values;

          this.lineChartData.labels = labels;
          this.lineChartData.datasets[0].data = values;

          // Calculate stats
          this.tongUngVien = values.reduce((a, b) => a + b, 0);
          this.ungVienThangNay = values[values.length - 1] || 0;
          this.trungBinh = Math.round(this.tongUngVien / values.length);
          
          if (values.length >= 2) {
            const thangTruoc = values[values.length - 2];
            this.tangTruong = thangTruoc > 0 
              ? Math.round(((this.ungVienThangNay - thangTruoc) / thangTruoc) * 100)
              : 0;
          }

          this.chart?.update();
          this.cd.detectChanges();
        }
      },
      error: (err) => {
        console.error('Lỗi khi lấy số liệu ứng viên:', err);
      }
    });
  }
}