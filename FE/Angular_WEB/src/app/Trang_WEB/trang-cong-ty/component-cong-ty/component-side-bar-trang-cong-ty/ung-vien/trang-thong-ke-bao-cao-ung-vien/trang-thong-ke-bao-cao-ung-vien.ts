import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Auth } from '../../../../../../services/auth';

@Component({
  selector: 'app-trang-thong-ke-bao-cao-ung-vien',
  imports: [BaseChartDirective],
  templateUrl: './trang-thong-ke-bao-cao-ung-vien.html',
  styleUrl: './trang-thong-ke-bao-cao-ung-vien.css'
})
export class TrangThongKeBaoCaoUngVien implements OnInit {
  thongTin: any;
  constructor(private auth: Auth) { 
    this.thongTin = this.auth.layThongTinNguoiDung();
  }
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{ data: [], label: "Số lượng ứng viên" }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{ data: [], label: "Số lượng ứng viên", fill: false, tension: 0.3 }]
  };
  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      title: { display: true, text: "Thống kê số lượng ứng viên theo tháng" }
    }
  };

  async laySoLieuUngVien() {
    const ma_Cong_Ty = this.thongTin?.thong_tin_chi_tiet?.ma_cong_ty;
    const response = await fetch("http://localhost:65001/api/API_WEB/laySoLuongUngVien", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ ma_cong_ty: ma_Cong_Ty })
    });
    const data = await response.json();
    if(data.success){
      const labels = data.so_luong.map((x: any) => `Tháng ${x.thang}`);
      const values = data.so_luong.map((x: any) => x.so_luong_ung_vien);

      this.barChartData.labels = labels;
      this.barChartData.datasets[0].data = values;

      this.lineChartData.labels = labels;
      this.lineChartData.datasets[0].data = values;
      this.chart?.update();
    }
  }
  ngOnInit(): void {
    this.laySoLieuUngVien();
  }
}
