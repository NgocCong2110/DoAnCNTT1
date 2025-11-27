import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { Auth } from '../../../../../services/auth';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  danh_sach: any;
  so_luong: { thang: number; so_luong_bai_dang: number }[];
}

interface so_luong_ung_vien{
  tieu_de: string;
  so_luong_ung_vien: Number;
  ngay_tao: string
}

@Component({
  selector: 'app-component-thong-ke-ung-vien-tung-bai',
  imports: [CommonModule, BaseChartDirective, RouterModule],
  templateUrl: './component-thong-ke-ung-vien-tung-bai.html',
  styleUrl: './component-thong-ke-ung-vien-tung-bai.css'
})
export class ComponentThongKeUngVienTungBai implements OnInit {
  @ViewChild('lineChart') lineChart?: BaseChartDirective;
  constructor(public auth: Auth, private httpclient: HttpClient, private cd: ChangeDetectorRef) { }
  so_luong_ung_vien_ung_bai: any;

  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [{
      data: [],
      label: 'Số lượng bài đăng',
      backgroundColor: 'rgba(74, 144, 226, 0.8)',
      borderColor: 'rgba(74, 144, 226, 1)',
      borderWidth: 2,
      borderRadius: 8,
      barThickness: 40
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
        text: 'Biểu đồ cột - Số lượng bài đăng',
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

  laySoLuongBaiDangCuaCongTy(){
    const ma_cong_ty = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_cong_ty;
    const thong_tin = {
      ma_cong_ty: ma_cong_ty
    }
    this.httpclient.post<API_RESPONSE>('http://localhost:7000/api/API_WEB/laySoLuongBaiDangCuaCongTy', thong_tin)
      .subscribe({
      next: (data) => {
        if (data.success && data.so_luong.length > 0) {
          const labels = data.so_luong.map(x => `Tháng ${x.thang}`);
          const values = data.so_luong.map(x => x.so_luong_bai_dang);
          this.barChartData = {
            labels: [...labels],
            datasets: [{
              ...this.barChartData.datasets[0],
              data: [...values]
            }]
          };

          this.cd.detectChanges();

          setTimeout(() => {
            this.lineChart?.update();
            this.cd.detectChanges();
          },0);
        }
        this.cd.markForCheck();
      },
      error: (err) => {
        console.error('Lỗi khi lấy số liệu ứng viên:', err);
      }
    });
  }
  laySoLuongUngVienTungBai() {
    const ma_cong_ty = this.auth.layThongTinNguoiDung().thong_tin_chi_tiet?.ma_cong_ty;
    const thong_tin = {
      ma_cong_ty: ma_cong_ty
    }
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/laySoLuongUngVienTungBai', thong_tin)
      .subscribe({
        next: (data) => {
          if (data?.success && Array.isArray(data.danh_sach)) {
            this.so_luong_ung_vien_ung_bai = data.danh_sach.map((item: so_luong_ung_vien) => ({
              tieu_de: item.tieu_de,
              so_luong_ung_vien: item.so_luong_ung_vien,
              ngay_tao: new Date(item.ngay_tao).toLocaleString('vi-VN')
            }));
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        },
        error: (err) => console.error('Lỗi khi lấy số lượng:', err)
      });
  }

  ngOnInit(): void {
    this.laySoLuongUngVienTungBai();
    this.laySoLuongBaiDangCuaCongTy();
  }
}
