import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const VaiTroGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);

  const cookieService = inject(CookieService);

  const thong_tin = cookieService.get('thongTin_DangNhap');

  if (!thong_tin) {
    router.navigate(['/dang-nhap']);
    return false;
  }

  const nguoi_dung = JSON.parse(decodeURIComponent(thong_tin));
  const allowedRoles = route.data['allowedRoles'] as Array<string>;
  const vai_tro = nguoi_dung?.kieu_nguoi_dung ?? null;

  if (allowedRoles.includes(vai_tro)) {
    return true;
  }

  router.navigate(['/trang-chu']);
  return false;
};
