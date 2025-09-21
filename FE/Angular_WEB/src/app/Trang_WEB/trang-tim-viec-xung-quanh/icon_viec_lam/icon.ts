// icon.ts
import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root' // đăng ký global, không cần providers ở component
})
export class IconService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  async getIcons() {
    if (isPlatformBrowser(this.platformId)) {
      const L = await import('leaflet');

      return {
        cong_nghe_thong_tin: L.icon({
          iconUrl: "anh_WEB/anh_icon_WEB/anh_icon_cntt.webp",
          iconSize: [32, 32],
          iconAnchor: [16, 32],
          popupAnchor: [0, -32]
        }),
        ke_toan: L.icon({
          iconUrl: "anh_WEB/anh_icon_WEB/anh_icon_ke_toan.webp",
          iconSize: [32, 32],
          iconAnchor: [16, 32],
          popupAnchor: [0, -32]
        }),
        ngan_hang: L.icon({
          iconUrl: "anh_WEB/anh_icon_WEB/anh_icon_ngan_hang.webp",
          iconSize: [32, 32],
          iconAnchor: [16, 32],
          popupAnchor: [0, -32]
        })
      };
    }
    return {};
  }
}
