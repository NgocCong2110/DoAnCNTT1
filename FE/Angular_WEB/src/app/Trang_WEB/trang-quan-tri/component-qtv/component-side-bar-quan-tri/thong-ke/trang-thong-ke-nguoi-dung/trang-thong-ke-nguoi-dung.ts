import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-trang-thong-ke-nguoi-dung',
  standalone: true, 
  imports: [BaseChartDirective],
  templateUrl: './trang-thong-ke-nguoi-dung.html',
  styleUrls: ['./trang-thong-ke-nguoi-dung.css']
})
export class TrangThongKeNguoiDung implements OnInit {

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{ data: [], label: "Số lượng người dùng" }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{ data: [], label: "Số lượng người dùng", fill: false, tension: 0.3 }]
  };

  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      title: { display: true, text: "Thống kê số lượng người dùng theo tháng" }
    }
  };

  async laySoLieuNguoiDung() {
    try {
      const response = await fetch("http://localhost:65001/api/API_WEB/laySoLuongNguoiDung", {
        method: "POST",
        headers: { "Content-Type": "application/json" }
      });
      const data = await response.json();

      if (data?.success && Array.isArray(data.so_luong)) {
        const labels = data.so_luong.map((x: any) => `Tháng ${x.thang}`);
        const values = data.so_luong.map((x: any) => x.so_luong_nguoi_dung);

        this.barChartData.labels = labels;
        this.barChartData.datasets[0].data = values;

        this.lineChartData.labels = labels;
        this.lineChartData.datasets[0].data = values;

        this.chart?.update();
      }
    } catch (err) {
      console.error("Lỗi khi lấy dữ liệu người dùng:", err);
    }
  }

  ngOnInit(): void {
    this.laySoLieuNguoiDung();
  }
}
