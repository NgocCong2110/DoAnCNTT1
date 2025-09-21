import { Component, OnInit, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { HttpClient, HttpClientModule } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
  so_luong: { thang: number; so_luong_nguoi_dung: number }[];
}

@Component({
  selector: 'app-trang-thong-ke-nguoi-dung',
  standalone: true,
  imports: [BaseChartDirective, HttpClientModule],
  templateUrl: './trang-thong-ke-nguoi-dung.html',
  styleUrls: ['./trang-thong-ke-nguoi-dung.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TrangThongKeNguoiDung implements OnInit {
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  constructor(
    public http: HttpClient,
    public cd: ChangeDetectorRef
  ) {}

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{ data: [], label: 'Số lượng người dùng' }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{ data: [], label: 'Số lượng người dùng', fill: false, tension: 0.3 }]
  };

  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      title: { display: true, text: 'Thống kê số lượng người dùng theo tháng' }
    }
  };

  laySoLieuNguoiDung() {
    this.http.post<API_RESPONSE>(
      'http://localhost:65001/api/API_WEB/laySoLuongNguoiDung',
      {}
    ).subscribe({
      next: (data) => {
        if (data?.success && Array.isArray(data.so_luong)) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong_nguoi_dung);

          this.barChartData = {
            labels,
            datasets: [{ data: values, label: 'Số lượng người dùng' }]
          };

          this.lineChartData = {
            labels,
            datasets: [{ data: values, label: 'Số lượng người dùng', fill: false, tension: 0.3 }]
          };

          this.chart?.update();
          this.cd.markForCheck(); 
        }
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
