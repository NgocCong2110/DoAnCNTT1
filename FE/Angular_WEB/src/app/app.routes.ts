import { Routes } from '@angular/router';
import { TrangDangKyNTV } from './Trang_WEB/trang-dang-ky-ntv/trang-dang-ky-ntv';
import { TrangDangNhap } from './Trang_WEB/trang-dang-nhap/trang-dang-nhap';
import { TrangDangKyCongTy } from './Trang_WEB/trang-dang-ky-cong-ty/trang-dang-ky-cong-ty';
import { TrangChu } from './Trang_WEB/trang-chu/trang-chu';
import { TrangLuaChonDangKy } from './Trang_WEB/trang-lua-chon-dang-ky/trang-lua-chon-dang-ky';
import { TrangQuanTri } from './Trang_WEB/trang-quan-tri/trang-quan-tri';
import { TrangNguoiTimViec } from './Trang_WEB/trang-nguoi-tim-viec/trang-nguoi-tim-viec';
import { TrangCongTy } from './Trang_WEB/trang-cong-ty/trang-cong-ty';

export const routes: Routes = [
    {path: '', redirectTo: '/trang-chu', pathMatch: 'full'},
    { path: 'trang-chu', component: TrangChu },
    { path: 'dang-ky', component: TrangDangKyNTV },
    { path: 'dang-nhap', component: TrangDangNhap },
    { path: 'dang-ky-cong-ty', component: TrangDangKyCongTy },
    { path: 'lua-chon-dang-ky', component: TrangLuaChonDangKy },
    { path: 'trang-nguoi-tim-viec', component: TrangNguoiTimViec },
    { path: 'trang-cong-ty', component: TrangCongTy },
    {
        // path: 'trang-quan-tri',
        // component: TrangQuanTri,
        // children: [
        //     { path: 'trang-cong-ty', component: TrangCongTy }
        // ]
    }
];
