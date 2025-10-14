import { Component, AfterViewInit, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IconService } from './icon_viec_lam/icon';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-tim-viec-xung-quanh',
  standalone: true,
  imports: [FormsModule, CommonModule, HeaderWEB],
  providers: [IconService], 
  templateUrl: './trang-tim-viec-xung-quanh.html',
  styleUrls: ['./trang-tim-viec-xung-quanh.css']
})
export class TrangTimViecXungQuanh implements AfterViewInit {
  private map!: any;
  private vi_tri_nguoi_dung!: any;
  nganh_nghe: string = '';
  ban_kinh: number = 5;

  tinh_thanh: any[] = [];
  quan_huyen: any[] = [];
  phuong_xa: any[] = [];
  selectedTinh: any;
  selectedHuyen: any;
  selectedXa: any;

  private diem_danh_dau: any[] = [];
  private icons: any = {};

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private iconService: IconService,
    public httpclient : HttpClient
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

      this.httpclient.get<any[]>('https://provinces.open-api.vn/api/p/')
        .subscribe(data => this.tinh_thanh = data);
    }
  }

  layThongTinTinh(){
    if (this.selectedTinh){
      this.httpclient.get<any>(`https://provinces.open-api.vn/api/p/${this.selectedTinh.code}?depth=2`)
        .subscribe(data => this.quan_huyen = data.districts);
        this.phuong_xa = [];
        this.selectedHuyen = null;
        this.selectedXa = null;
    }
  }

  layThongTinHuyen(){
    if (this.selectedHuyen) {
      this.httpclient.get<any>(`https://provinces.open-api.vn/api/d/${this.selectedHuyen.code}?depth=2`)
        .subscribe(data => this.phuong_xa = data.wards);
      this.selectedXa = null;
    }
  }

  

  layToaDo(){
    const dia_chi = [
      this.selectedXa?.name,
      this.selectedHuyen?.name,
      this.selectedTinh?.name
    ].filter(x => x).join(', ');
    
    if(!dia_chi){
      alert("Vui long chon dia chi");
      return;
    }

    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    this.httpclient.post<any>('http://localhost:65001/api/API_WEB/layToaDoCongTy', JSON.stringify(dia_chi),
      { headers }
    )
      .subscribe({
        next: async (data) => {
          const L = await import('leaflet');
          const lat = parseFloat(data.lat);
          const lng = parseFloat(data.lng);
          this.map.setView([lat, lng], 14);

          const marker = L.marker([lat, lng]).bindPopup(` ${dia_chi}`);
          marker.addTo(this.map);
          this.diem_danh_dau.push(marker);
        },
        error: (err) => {
          alert("Khong tim thay vi tri");
        }
      })
  }
}
