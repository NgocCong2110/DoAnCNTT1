import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ChartData, ChartOptions } from 'chart.js';
import { HttpClient } from '@angular/common/http';
import { BaseChartDirective } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  so_luong: { loai_nguoi_dung: string; so_luong: number }[];
}

interface nguoi_dung_moi {
  ma_cong_ty: number;
  loai_nguoi_dung: string;
  email: string | null;
  ngay_tao: string;
}

@Component({
  selector: 'app-component-thong-ke-phan-loai-nguoi-dung',
  standalone: true,
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './component-thong-ke-phan-loai-nguoi-dung.html',
  styleUrls: ['./component-thong-ke-phan-loai-nguoi-dung.css']
})
export class ComponentThongKePhanLoaiNguoiDung implements OnInit {
  @ViewChild(BaseChartDirective) pieChart?: BaseChartDirective;

  danh_sach_nguoi_dung_moi: any;

  constructor(private httpclient: HttpClient, private cd: ChangeDetectorRef, private router: Router) { }

  pieChartData!: ChartData<'pie'>;
  pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom',
        labels: { color: '#333', font: { size: 14 } }
      },
      title: {
        display: true,
        text: 'Tỷ lệ loại người dùng',
        font: { size: 18 }
      }
    }
  };

  ngOnInit(): void {
    this.layDanhSachPhanLoaiNguoiDung();
    this.layDanhSachNguoiDungMoi();
  }

  layDanhSachPhanLoaiNguoiDung() {
    this.httpclient
      .post<API_RESPONSE>(
        'http://localhost:7000/api/API_WEB/laySoLuongCongTyVaNguoiTimViec',
        {}
      )
      .subscribe({
        next: (data) => {
          if (data?.success && Array.isArray(data.so_luong) && data.so_luong.length > 0) {
            const labels = data.so_luong.map((x) =>
              x.loai_nguoi_dung === 'cong_Ty' ? 'Công ty' : 'Người tìm việc'
            );
            const values = data.so_luong.map((x) => x.so_luong);

            this.pieChartData = {
              labels,
              datasets: [
                {
                  data: values,
                  backgroundColor: ['#36A2EB', '#FF6384'],
                  borderColor: '#fff',
                  borderWidth: 2
                }
              ]
            };

            this.cd.detectChanges();
            setTimeout(() => {
              this.pieChart?.update();
            }, 0);
          }
        },
        error: (err) => {
          console.error('Lỗi khi lấy dữ liệu người dùng:', err);
        }
      });
  }
  layDanhSachNguoiDungMoi() {
    this.httpclient.post<any>('http://localhost:7000/api/API_WEB/layDanhSachDangKyMoi', {})
      .subscribe({
        next: (data) => {
          if (data?.success && Array.isArray(data.danh_sach)) {
            this.danh_sach_nguoi_dung_moi = data.danh_sach.map((item: nguoi_dung_moi) => ({
              ma_cong_ty: item.ma_cong_ty,
              email: item.email || '(chưa có email)',
              kieu_nguoi_dung:
                item.loai_nguoi_dung === 'cong_Ty'
                  ? 'Công ty'
                  : item.loai_nguoi_dung === 'nguoi_Tim_Viec'
                    ? 'Người tìm việc'
                    : 'Không xác định',
              ngay_tao: new Date(item.ngay_tao).toLocaleString('vi-VN')
            }));
            this.cd.detectChanges();
          }
          this.cd.markForCheck();
        },
        error: (err) => console.error('Lỗi khi lấy danh sách người dùng mới:', err)
      });
  }

  xemTrangCongTy(nguoi_dung: any){
    if(nguoi_dung.kieu_nguoi_dung == 'Người tìm việc'){
      return;
    }
    if(nguoi_dung.kieu_nguoi_dung == 'Công ty'){
      const ma_cong_ty = nguoi_dung.ma_cong_ty;
      this.router.navigate(['trang-gioi-thieu-cong-ty'], {queryParams: {ma_cong_ty}})
    }
  }
}
