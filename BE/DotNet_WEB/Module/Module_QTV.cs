using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_thong_bao;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_danh_gia;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_tai_khoan;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_thong_ke_quan_tri;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_xoa_thong_tin_quan_tri;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_lay_thong_tin_quan_tri;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_bai_dang_quan_tri;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_dich_vu_quan_tri;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_tao_tai_khoan_quan_tri;

namespace DotNet_WEB.Module
{
    public class Module_QTV
    {
        //tao tai khoan quan tri
        public static bool themQuanTriVien(quan_tri quan_Tri)
        {
            return chuc_nang_tao_tai_khoan_quan_tri_web.themQuanTriVien(quan_Tri);
        }
        //tao tai khoan quan tri

        //gui thong bao
        public static bool guiThongBaoToiServer(thong_bao thong_Bao)
        {
            return chuc_nang_thong_bao_web.guiThongBaoToiServer(thong_Bao);
        }

        public static List<thong_bao> thongBaoQuanTriRieng(int ma_quan_tri)
        {
            return chuc_nang_thong_bao_web.thongBaoQuanTriRieng(ma_quan_tri);
        }

        public static bool xoaThongBaoQuanTri(thong_bao thong_Bao)
        {
            return chuc_nang_thong_bao_web.xoaThongBaoQuanTri(thong_Bao);
        }
        //gui thong bao

        //lay thong tin nguoi dung
        public static List<nguoi_tim_viec> layDanhSachNguoiTimViec()
        {
            return chuc_nang_lay_thong_tin_quan_tri_web.layDanhSachNguoiTimViec();
        }

        public static List<cong_ty> layDanhSachCongTy()
        {
            return chuc_nang_lay_thong_tin_quan_tri_web.layDanhSachCongTy();
        }
        //lay thong tin nguoi dung

        //cap nhat thong tin quan tri
        public static bool capNhatThongTinQuanTri(thong_tin_truong_du_lieu_cap_nhat_quan_tri tt)
        {
            return chuc_nang_tai_khoan_quan_tri_web.capNhatThongTinQuanTri(tt);
        }

        public static async Task<string> capNhatAnhDaiDienQuanTriVien(IFormFile file, int ma_quan_tri)
        {
            return await chuc_nang_tai_khoan_quan_tri_web.capNhatAnhDaiDienQuanTriVien(file, ma_quan_tri);
        }

        public static bool kiemTraMatKhauQuanTri(quan_tri quan_Tri)
        {
            return chuc_nang_tai_khoan_quan_tri_web.kiemTraMatKhauQuanTri(quan_Tri);
        }
        
        public static bool capNhatMatKhauQuanTri(quan_tri quan_Tri)
        {
            return chuc_nang_tai_khoan_quan_tri_web.capNhatMatKhauQuanTri(quan_Tri);
        }
        //cap nhat thong tin quan tri

        //thong ke
        public static List<thong_ke_nguoi_dung> laySoLuongNguoiDung()
        {
            return chuc_nang_thong_ke_quan_tri_web.laySoLuongNguoiDung();
        }

        public static List<thong_ke_phan_loai> laySoLuongCongTyVaNguoiTimViec()
        {
            return chuc_nang_thong_ke_quan_tri_web.laySoLuongCongTyVaNguoiTimViec();
        }

        public static List<dang_ky_moi> layDanhSachDangKyMoi()
        {
            return chuc_nang_thong_ke_quan_tri_web.layDanhSachDangKyMoi();
        }

        public static List<thong_ke_tin_tuyen_dung> laySoLuongTinTuyenDung()
        {
            return chuc_nang_thong_ke_quan_tri_web.laySoLuongTinTuyenDung();
        }

        public static List<tin_tuyen_dung_moi> layDanhSachTinTuyenDungMoi()
        {
            return chuc_nang_thong_ke_quan_tri_web.layDanhSachTinTuyenDungMoi();
        }
        //thong ke

        //lay danh sach bai dang vi pham
        public static List<bai_dang_vi_pham> layDanhSachViPham()
        {
            return chuc_nang_bai_dang_quan_tri_web.layDanhSachViPham();
        }
        //lay danh sach bai dang vi pham

        //dich vu
        public static List<thanh_toan> layLichSuThanhToan()
        {
            return chuc_nang_dich_vu_quan_tri_web.layLichSuThanhToan();
        }

        public static bool taoDichVuMoi(dich_vu dich_Vu)
        {
            return chuc_nang_dich_vu_quan_tri_web.taoDichVuMoi(dich_Vu);
        }
        //dich vu


        //xoa thong tin
        public static bool xoaCongTy(int ma_Cong_Ty)
        {
            return chuc_nang_xoa_thong_tin_quan_tri_web.xoaCongTy(ma_Cong_Ty);
        }

        public static bool xoaNguoiTimViec(int ma_Nguoi_Tim_Viec)
        {
            return chuc_nang_xoa_thong_tin_quan_tri_web.xoaNguoiTimViec(ma_Nguoi_Tim_Viec);
        }
        //xoa thong tin

        //danh gia web
        public static List<danh_gia> layToanBoDanhSachDanhGia()
        {
            return chuc_nang_danh_gia_thong_bao_web.layToanBoDanhSachDanhGia();
        }

        public static bool capNhatTrangThaiDanhGia(danh_gia danh_Gia)
        {
            return chuc_nang_danh_gia_thong_bao_web.capNhatTrangThaiDanhGia(danh_Gia);
        }

        public static bool xoaDanhGia(int ma_danh_gia)
        {
            return chuc_nang_danh_gia_thong_bao_web.xoaDanhGia(ma_danh_gia);
        }
        //danh gia web

    }
}
