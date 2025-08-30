import { Routes } from '@angular/router';
import { TrangDangKyNTV } from './Trang_WEB/trang-dang-ky-ntv/trang-dang-ky-ntv';
import { TrangDangNhap } from './Trang_WEB/trang-dang-nhap/trang-dang-nhap';
import { TrangDangKyCongTy } from './Trang_WEB/trang-dang-ky-cong-ty/trang-dang-ky-cong-ty';
import { TrangChu } from './Trang_WEB/trang-chu/trang-chu';
import { TrangLuaChonDangKy } from './Trang_WEB/trang-lua-chon-dang-ky/trang-lua-chon-dang-ky';
import { TrangQuanTri } from './Trang_WEB/trang-quan-tri/trang-quan-tri';
import { TrangNguoiTimViec } from './Trang_WEB/trang-nguoi-tim-viec/trang-nguoi-tim-viec';
import { TrangCongTy } from './Trang_WEB/trang-cong-ty/trang-cong-ty';
import { TrangTimViecXungQuanh } from './Trang_WEB/trang-tim-viec-xung-quanh/trang-tim-viec-xung-quanh';
import { TrangTimViecBangTuKhoa } from './Trang_WEB/trang-tim-viec-bang-tu-khoa/trang-tim-viec-bang-tu-khoa';
import { TrangThongTinTaiKhoan } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/tai-khoan/trang-thong-tin-tai-khoan/trang-thong-tin-tai-khoan';
import { TrangTaoTaiKhoanQuanTri } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/tai-khoan/trang-tao-tai-khoan-quan-tri/trang-tao-tai-khoan-quan-tri';
import { TrangDanhSachNguoiTimViec } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/nguoi-dung/trang-danh-sach-nguoi-tim-viec/trang-danh-sach-nguoi-tim-viec';
import { TrangDanhSachCongTy } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/nguoi-dung/trang-danh-sach-cong-ty/trang-danh-sach-cong-ty';
import { TrangDuyetBaiDang } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/tin-tuyen-dung/trang-duyet-bai-dang/trang-duyet-bai-dang';
import { TrangDanhSachViPham } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/tin-tuyen-dung/trang-danh-sach-vi-pham/trang-danh-sach-vi-pham';

// ==== thang nao import vao trang cha thi khoi can import vao trang nay ====
export const routes: Routes = [
    {path: '', redirectTo: '/trang-chu', pathMatch: 'full'},
    { path: 'trang-chu', component: TrangChu },
    { path: 'dang-ky', component: TrangDangKyNTV },
    { path: 'dang-nhap', component: TrangDangNhap },
    { path: 'dang-ky-cong-ty', component: TrangDangKyCongTy },
    { path: 'lua-chon-dang-ky', component: TrangLuaChonDangKy },
    { path: 'trang-nguoi-tim-viec', component: TrangNguoiTimViec },
    { path: 'trang-cong-ty', component: TrangCongTy },
    { path: 'trang-tim-viec-xung-quanh', component: TrangTimViecXungQuanh },
    { path: 'trang-tim-viec-theo-tu-khoa', component: TrangTimViecBangTuKhoa },
    {
        path: 'trang-quan-tri',
        component: TrangQuanTri,
        children: [
            { path: 'trang-thong-tin-tai-khoan', component: TrangThongTinTaiKhoan },
            { path: 'trang-tao-tai-khoan-quan-tri', component: TrangTaoTaiKhoanQuanTri },
            { path: 'trang-danh-sach-nguoi-tim-viec', component: TrangDanhSachNguoiTimViec },
            { path: 'trang-danh-sach-cong-ty', component: TrangDanhSachCongTy },
            { path: 'trang-duyet-bai-dang', component: TrangDuyetBaiDang },
            { path: 'trang-danh-sach-vi-pham', component: TrangDanhSachViPham }
        ]
    }
];
