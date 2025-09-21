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
import { TrangDanhSachCongTy } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/nguoi-dung/trang-danh-sach-cong-ty/trang-danh-sach-cong-ty';import { TrangDanhSachViPham } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/tin-tuyen-dung/trang-danh-sach-vi-pham/trang-danh-sach-vi-pham';
import { TrangDanhMucNganhNghe } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/danh-muc/trang-danh-muc-nganh-nghe/trang-danh-muc-nganh-nghe';
import { TrangDanhMucKyNang } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/danh-muc/trang-danh-muc-ky-nang/trang-danh-muc-ky-nang';
import { TrangDanhSachGoiDichVuQuanTri } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/dich-vu/trang-danh-sach-goi-dich-vu-quan-tri/trang-danh-sach-goi-dich-vu-quan-tri';
import { TrangLichSuGiaoDich } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/dich-vu/trang-lich-su-giao-dich/trang-lich-su-giao-dich';
import { TrangThemGoiDichVuMoiQuanTri } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/dich-vu/trang-them-goi-dich-vu-moi-quan-tri/trang-them-goi-dich-vu-moi-quan-tri';
import { TrangThongKeDoanhThu } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/thong-ke/trang-thong-ke-doanh-thu/trang-thong-ke-doanh-thu';
import { TrangThongKeNguoiDung } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/thong-ke/trang-thong-ke-nguoi-dung/trang-thong-ke-nguoi-dung';
import { TrangGuiThongBao } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/thong-bao/trang-gui-thong-bao/trang-gui-thong-bao';
import { TrangDanhSachThongBao } from './Trang_WEB/trang-quan-tri/component-qtv/component-side-bar-quan-tri/thong-bao/trang-danh-sach-thong-bao/trang-danh-sach-thong-bao';
import { TrangThongTinTaiKhoanNtv } from './Trang_WEB/trang-nguoi-tim-viec/component-ntv/component-side-bar-ntv/tai-khoan/trang-thong-tin-tai-khoan-ntv/trang-thong-tin-tai-khoan-ntv';
import { TrangDanhSachChungChi } from './Trang_WEB/trang-nguoi-tim-viec/component-ntv/component-side-bar-ntv/ho-so/trang-danh-sach-chung-chi/trang-danh-sach-chung-chi';
import { TrangLichSuUngTuyen } from './Trang_WEB/trang-nguoi-tim-viec/component-ntv/component-side-bar-ntv/ho-so/trang-lich-su-ung-tuyen/trang-lich-su-ung-tuyen';
import { TrangThongBaoNtv } from './Trang_WEB/trang-nguoi-tim-viec/component-ntv/component-side-bar-ntv/thong-bao/trang-thong-bao-ntv/trang-thong-bao-ntv';
import { TrangCvNtv } from './Trang_WEB/trang-nguoi-tim-viec/component-ntv/component-side-bar-ntv/ho-so/trang-cv-ntv/trang-cv-ntv';
import { TrangThongTinTaiKhoanCongTy } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/tai-khoan/trang-thong-tin-tai-khoan-cong-ty/trang-thong-tin-tai-khoan-cong-ty';
import { TrangDanhSachUngVien } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/ung-vien/trang-danh-sach-ung-vien/trang-danh-sach-ung-vien';
import { TrangThongKeBaoCaoUngVien } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/ung-vien/trang-thong-ke-bao-cao-ung-vien/trang-thong-ke-bao-cao-ung-vien';
import { TrangDanhSachBaiDangCongTy } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/bai-dang/trang-danh-sach-bai-dang-cong-ty/trang-danh-sach-bai-dang-cong-ty';
import { TrangBaiDangDaXoaCongTy } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/bai-dang/trang-bai-dang-da-xoa-cong-ty/trang-bai-dang-da-xoa-cong-ty';
import { TrangThongBaoCongTy } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/thong-bao/trang-thong-bao-cong-ty/trang-thong-bao-cong-ty';
import { TrangDanhSachGoiDichVuCongTy } from './Trang_WEB/trang-cong-ty/component-cong-ty/component-side-bar-trang-cong-ty/dich-vu/trang-danh-sach-goi-dich-vu-cong-ty/trang-danh-sach-goi-dich-vu-cong-ty';
import { TrangViecLamDaLuu } from './Trang_WEB/danh-muc/trang-viec-lam-da-luu/trang-viec-lam-da-luu';
import { TrangThongBao } from './Trang_WEB/danh-muc/trang-thong-bao/trang-thong-bao';


// ==== thang nao import vao trang cha thi khoi can import vao trang nay ====
export const routes: Routes = [
    {path: '', redirectTo: '/trang-chu', pathMatch: 'full'},
    { path: 'trang-chu', component: TrangChu },
    { path: 'dang-ky', component: TrangDangKyNTV },
    { path: 'dang-nhap', component: TrangDangNhap },
    { path: 'dang-ky-cong-ty', component: TrangDangKyCongTy },
    { path: 'lua-chon-dang-ky', component: TrangLuaChonDangKy },
    { path: 'trang-tim-viec-xung-quanh', component: TrangTimViecXungQuanh },
    { path: 'trang-tim-viec-theo-tu-khoa', component: TrangTimViecBangTuKhoa },
    { path: 'trang-viec-lam-da-luu', component: TrangViecLamDaLuu},
    { path: 'trang-thong-bao', component: TrangThongBao},
    {

        // component trang nguoi tim viec
        path: 'trang-nguoi-tim-viec',
        component: TrangNguoiTimViec,
        children: [
            { path: 'trang-thong-tin-tai-khoan-ntv', component: TrangThongTinTaiKhoanNtv },

            { path: 'trang-danh-sach-chung-chi', component: TrangDanhSachChungChi },

            { path: 'trang-lich-su-ung-tuyen', component: TrangLichSuUngTuyen },

            { path: 'trang-thong-bao-ntv', component: TrangThongBaoNtv },
            
            { path: 'trang-cv-ntv', component: TrangCvNtv }
        ]
    },
        // component trang cong ty
    {
        path: 'trang-cong-ty',
        component: TrangCongTy,
        children: [
            { path: 'trang-thong-tin-tai-khoan-cong-ty', component: TrangThongTinTaiKhoanCongTy },

            { path: 'trang-danh-sach-ung-vien', component: TrangDanhSachUngVien },

            { path: 'trang-thong-ke-bao-cao-ung-vien', component: TrangThongKeBaoCaoUngVien },

            { path: 'trang-danh-sach-bai-dang-cong-ty', component: TrangDanhSachBaiDangCongTy },

            { path: 'trang-bai-dang-da-xoa-cong-ty', component: TrangBaiDangDaXoaCongTy },

            { path: 'trang-thong-bao-cong-ty', component: TrangThongBaoCongTy },

            { path: 'trang-danh-sach-goi-dich-vu-cong-ty', component: TrangDanhSachGoiDichVuCongTy }
        ]
    },

    {

        // component trang quan tri
        path: 'trang-quan-tri',
        component: TrangQuanTri,
        children: [
            { path: 'trang-thong-tin-tai-khoan', component: TrangThongTinTaiKhoan },

            { path: 'trang-tao-tai-khoan-quan-tri', component: TrangTaoTaiKhoanQuanTri },

            { path: 'trang-danh-sach-nguoi-tim-viec', component: TrangDanhSachNguoiTimViec },

            { path: 'trang-danh-sach-cong-ty', component: TrangDanhSachCongTy },

            { path: 'trang-danh-sach-vi-pham', component: TrangDanhSachViPham },

            { path: 'trang-danh-muc-nganh-nghe', component: TrangDanhMucNganhNghe },

            { path: 'trang-danh-muc-ky-nang', component: TrangDanhMucKyNang },

            { path: 'trang-danh-sach-goi-dich-vu', component: TrangDanhSachGoiDichVuQuanTri },

            { path: 'trang-lich-su-giao-dich', component: TrangLichSuGiaoDich },

            { path: 'trang-them-goi-dich-vu-moi', component: TrangThemGoiDichVuMoiQuanTri },

            { path: 'trang-thong-ke-doanh-thu', component: TrangThongKeDoanhThu },

            { path: 'trang-thong-ke-nguoi-dung', component: TrangThongKeNguoiDung },

            { path: 'trang-gui-thong-bao', component: TrangGuiThongBao },

            { path: 'trang-danh-sach-thong-bao', component: TrangDanhSachThongBao }
        ]
    }
];
