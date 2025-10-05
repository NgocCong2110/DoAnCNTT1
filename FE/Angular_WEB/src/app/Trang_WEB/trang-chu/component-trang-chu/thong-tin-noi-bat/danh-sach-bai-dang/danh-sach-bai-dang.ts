import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { BaiDang } from '../../../../../services/bai-dang-service/bai-dang';
import { BaiDangComponent } from '../bai-dang.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-danh-sach-bai-dang',
  standalone: true, 
  imports: [CommonModule],
  templateUrl: './danh-sach-bai-dang.html',
  styleUrls: ['./danh-sach-bai-dang.css']
})
export class DanhSachBaiDang implements OnInit, OnDestroy {
  danhSachBaiDangFull: BaiDangComponent[] = []; 
  danhSachBaiDang: BaiDangComponent[] = [];  
  loading = true;
  error = '';
  baiDangDangChon: BaiDangComponent | null = null;

  trangHienTai = 1;
  soLuongMoiTrang = 4;
  tongTrang = 1;

  private sub1?: Subscription;
  private sub2?: Subscription;

  constructor(
    private baiDangService: BaiDang,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.sub1 = this.baiDangService.bai_dang_duoc_chon$.subscribe(baiDang => {
      this.baiDangDangChon = baiDang;
      this.cd.detectChanges(); 
    });

    this.sub2 = this.baiDangService.danhSach$.subscribe(danhSach => {
      this.danhSachBaiDangFull = [];
      this.danhSachBaiDangFull = danhSach;
      this.tongTrang = Math.ceil(this.danhSachBaiDangFull.length / this.soLuongMoiTrang);
      this.loadTrang(this.trangHienTai);
      this.loading = false;
      this.cd.detectChanges();
    });

    this.baiDangService.layDanhSachBaiDang().catch(err => {
      this.error = err.message;
      this.loading = false;
      this.cd.detectChanges();
    });
  }

  loadTrang(trang: number) {
    this.trangHienTai = trang;
    const start = (trang - 1) * this.soLuongMoiTrang;
    const end = start + this.soLuongMoiTrang;
    this.danhSachBaiDang = this.danhSachBaiDangFull.slice(start, end);
    this.cd.detectChanges(); 
  }

  chuyenTrang(trang: number) {
    if (trang < 1 || trang > this.tongTrang) return;
    this.loadTrang(trang);
  }

  chonBaiDang(baiDang: BaiDangComponent) {
    this.baiDangService.chonBaiDang(baiDang);
  }

  ngOnDestroy() {
    this.sub1?.unsubscribe();
    this.sub2?.unsubscribe();
  }
}
