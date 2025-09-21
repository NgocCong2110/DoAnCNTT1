import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Auth } from '../../../../../../services/auth';
import { HttpClient } from '@angular/common/http';

interface API_RESPONSE {
  success: boolean;
  so_luong: { thang: number; so_luong_ung_vien: number }[];
}

@Component({
  selector: 'app-trang-thong-ke-bao-cao-ung-vien',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './trang-thong-ke-bao-cao-ung-vien.html',
  styleUrls: ['./trang-thong-ke-bao-cao-ung-vien.css']
})
export class TrangThongKeBaoCaoUngVien implements OnInit {
  thongTin: any;
  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{ data: [], label: 'Số lượng ứng viên' }]
  };

  lineChartData: ChartData<'line'> = {
    labels: [],
    datasets: [{ data: [], label: 'Số lượng ứng viên', fill: false, tension: 0.3 }]
  };

  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      title: { display: true, text: 'Thống kê số lượng ứng viên theo tháng' }
    }
  };

  constructor(public auth: Auth, public http: HttpClient, public cd: ChangeDetectorRef){
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
        if (data.success) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong_ung_vien);

          this.barChartData.labels = labels;
          this.barChartData.datasets[0].data = values;

          this.lineChartData.labels = labels;
          this.lineChartData.datasets[0].data = values;

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
