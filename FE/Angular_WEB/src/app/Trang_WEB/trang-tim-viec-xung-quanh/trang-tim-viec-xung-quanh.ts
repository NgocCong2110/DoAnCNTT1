import { Component, AfterViewInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IconService } from './icon_viec_lam/icon';

@Component({
  selector: 'app-trang-tim-viec-xung-quanh',
  standalone: true,
  imports: [FormsModule, CommonModule],
  providers: [IconService], 
  templateUrl: './trang-tim-viec-xung-quanh.html',
  styleUrls: ['./trang-tim-viec-xung-quanh.css']
})
export class TrangTimViecXungQuanh implements AfterViewInit {
  private map!: any;
  private vi_tri_nguoi_dung!: any;
  nganh_nghe: string = '';
  ban_kinh: number = 5;
  danh_sach_nganh_nghe = [
    { ten: 'Lập trình viên', nganh: 'cong_nghe_thong_tin', lat: 21.0285, lng: 105.8542 },
    { ten: 'Kế toán tổng hợp', nganh: 'ke_toan', lat: 21.0300, lng: 105.8400 }
  ];
  private diem_danh_dau: any[] = [];
  private icons: any = {};

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private iconService: IconService
  ) {}

  async ngAfterViewInit(): Promise<void> {
    if (isPlatformBrowser(this.platformId)) {
      const L = await import('leaflet');

      this.icons = await this.iconService.getIcons();

      this.map = L.map('map').setView([21.0278, 105.8342], 13);
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
      }).addTo(this.map);

      this.map.locate({ setView: true, maxZoom: 15 });
      this.map.on('locationfound', (e: any) => {
        this.vi_tri_nguoi_dung = e.latlng;
        L.marker(e.latlng).addTo(this.map).bindPopup('Bạn đang ở đây');
      });
    }
  }

  async timKiem() {
    if (!this.vi_tri_nguoi_dung) {
      alert('Chưa xác định được vị trí của bạn');
      return;
    }

    const L = await import('leaflet');

    this.diem_danh_dau.forEach(marker => this.map.removeLayer(marker));
    this.diem_danh_dau = [];

    this.danh_sach_nganh_nghe.forEach(thuoc_tinh_nganh => {
      const vi_tri_nganh = L.latLng(thuoc_tinh_nganh.lat, thuoc_tinh_nganh.lng);
      const ban_kinh_nguoi_dung = this.vi_tri_nguoi_dung.distanceTo(vi_tri_nganh) / 1000;

      if (
        (this.nganh_nghe === '' || thuoc_tinh_nganh.nganh === this.nganh_nghe) &&
        ban_kinh_nguoi_dung <= this.ban_kinh
      ) {
        const danh_dau = L.marker(vi_tri_nganh, { icon: this.icons[thuoc_tinh_nganh.nganh] })
          .bindPopup(
            `<b>${thuoc_tinh_nganh.ten}</b><br/>Ngành: ${thuoc_tinh_nganh.nganh}<br/>Cách bạn: ${ban_kinh_nguoi_dung.toFixed(2)} km`
          );

        danh_dau.addTo(this.map);
        this.diem_danh_dau.push(danh_dau);
      }
    });
  }
}
