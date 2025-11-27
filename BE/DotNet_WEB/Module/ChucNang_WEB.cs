using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using ZstdSharp.Unsafe;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography.X509Certificates;
using System.Net.Quic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_viec_lam;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.danh_gia_web;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_cv;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_otp;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_bai_dang;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_thong_bao;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_ung_tuyen;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_them_nguoi_dung;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_doi_mat_khau;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_lay_thong_tin;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_goi_y;

namespace DotNet_WEB.Module
{
    public class ChucNang_WEB
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool kiemTraTaiKhoanDangKy(nguoi_dung nguoi_Dung)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select 1 from nguoi_dung where email = @email";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@email", nguoi_Dung.email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return false;
            }
            return true;
        }

        public static async Task<string> luuCVOnLine(cv_online_nguoi_tim_viec cv_Online_Nguoi_Tim_Viec)
        {
            return await luuCV.luuCVOnline(cv_Online_Nguoi_Tim_Viec);
        }

        //viec lam
        public static List<so_luong_nganh_nghe> layNganhNgheNoiBat()
        {
            return chuc_nang_viec_lam_web.layNganhNgheNoiBat();
        }

        public static List<nganh_nghe> layDanhSachNganhNghe()
        {
            return chuc_nang_viec_lam_web.layDanhSachNganhNghe();
        }
        
        public static bool themNganhNgheMoi(nganh_nghe nganh_Nghe)
        {
            return chuc_nang_viec_lam_web.themNganhNgheMoi(nganh_Nghe);
        }

        public static bool xoaNganhNghe(nganh_nghe nganh_Nghe)
        {
            return chuc_nang_viec_lam_web.xoaNganhNghe(nganh_Nghe);
        }

        public static viec_lam? layViecLamTheoBaiDang(int ma_bai_dang)
        {

            return chuc_nang_viec_lam_web.layViecLamTheoBaiDang(ma_bai_dang);
        }
        public static List<so_luong_ung_vien_viec_lam> layDanhSachViecLamDuocQuanTam()
        {
            return chuc_nang_viec_lam_web.layDanhSachViecLamDuocQuanTam();
        }
        public static List<viec_lam_ket_qua> duaRaDanhSachDeXuat(string chuoi_yeu_cau)
        {
            return chuc_nang_viec_lam_web.duaRaDanhSachDeXuat(chuoi_yeu_cau);
        }

        public static List<viec_lam_ket_qua> deXuatViecLamSelector(viec_lam viec_Lam)
        {
            return chuc_nang_viec_lam_web.deXuatViecLamSelector(viec_Lam);
        }

        public static List<viec_lam> layDanhSachViecLamLienQuan(bai_dang bai_Dang)
        {
            return chuc_nang_viec_lam_web.layDanhSachViecLamLienQuan(bai_Dang);
        }
        //viec lam


        //danh gia
        public static List<danh_gia> layDanhSachDanhGia()
        {
            return lay_danh_sach_danh_gia.layDanhSachDanhGia();
        }

        public static bool themDanhGiaMoi(danh_gia danh_Gia)
        {
            return them_danh_gia_moi.themDanhGiaMoi(danh_Gia);
        }
        //danh gia








        //them thong tin nguoi dung
        public static bool themNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            return chuc_nang_them_nguoi_dung_web.themNguoiTimViec(nguoi_Tim_Viec);
        }

        public static bool themCongTy(cong_ty cong_Ty)
        {
            return chuc_nang_them_nguoi_dung_web.themCongTy(cong_Ty);
        }
        //them thong tin nguoi dung



        //doi mat khau
        public static bool doiMatKhauMoi(string email, string mat_khau)
        {
            return chuc_nang_doi_mat_khau_web.doiMatKhauMoi(email, mat_khau);
        }

        public static bool doiMatKhauQuanTri(string email, string mat_khau)
        {
            return chuc_nang_doi_mat_khau_web.doiMatKhauQuanTri(email, mat_khau);
        }
        //doi mat khau




        //thong tin nguoi dung
        public static List<nguoi_dung> thongTinNguoiDungBangEmail(string email)
        {
            return chuc_nang_lay_thong_tin_web.thongTinNguoiDungBangEmail(email);
        }

        public static List<quan_tri> layThongTinQuanTri(string email_Quan_Tri)
        {
            return chuc_nang_lay_thong_tin_web.layThongTinQuanTri(email_Quan_Tri);
        }

        public static string layHoTenNguoiDung(int ma_nguoi_dung)
        {
            return chuc_nang_lay_thong_tin_web.layHoTenNguoiDung(ma_nguoi_dung);
        }
        //thong tin nguoi dung




        //bai dang
        public static List<bai_dang> layDanhSachBaiDang()
        {
            return chuc_nang_bai_dang_web.layDanhSachBaiDang();
        }

        public static List<bai_dang> layBaiDangTheoMa(int ma_Bai_Dang)
        {
            return chuc_nang_bai_dang_web.layBaiDangTheoMa(ma_Bai_Dang);
        }

        public static bool themBaiDangMoi(bai_dang bai_Dang, viec_lam viec_Lam, List<int> phuc_Loi)
        {
            return chuc_nang_bai_dang_web.themBaiDangMoi(bai_Dang, viec_Lam, phuc_Loi);
        }

        public static bool luuBaiDangViPham(bai_dang_vi_pham bai_Dang_Vi_Pham)
        {
            return chuc_nang_bai_dang_web.luuBaiDangViPham(bai_Dang_Vi_Pham);
        }

        public static bool luuBaiDang(bai_dang_da_luu bai_Dang_Da_Luu)
        {
            return chuc_nang_bai_dang_web.luuBaiDang(bai_Dang_Da_Luu);
        }

        public static bool huyLuuBaiDang(bai_dang_da_luu bai_Dang_Da_Luu)
        {
            return chuc_nang_bai_dang_web.huyLuuBaiDang(bai_Dang_Da_Luu);
        }

        public static List<bai_dang> layDanhSachBaiDangDaLuu(int ma_Nguoi_Luu)
        {
            return chuc_nang_bai_dang_web.layDanhSachBaiDangDaLuu(ma_Nguoi_Luu);
        }

        public static bool xoaBaiDang(int ma_Bai_Dang)
        {
            return chuc_nang_bai_dang_web.xoaBaiDang(ma_Bai_Dang);
        }

        //bai dang




        //ung tuyen
        public static bool kiemTraUngTuyen(ung_tuyen ung_Tuyen)
        {
            return chuc_nang_ung_tuyen_web.kiemTraUngTuyen(ung_Tuyen);
        }

        public static bool ungTuyenCongViec(ung_tuyen ung_Tuyen)
        {
            return chuc_nang_ung_tuyen_web.ungTuyenCongViec(ung_Tuyen);
        }
        public static async Task<bool> ungTuyenCongViecUploadCV(int ma_viec, int ma_cong_ty, int ma_nguoi_tim_viec, int ma_nguoi_nhan, IFormFile duong_dan_file_cv_upload)
        {
            return await chuc_nang_ung_tuyen_web.ungTuyenCongViecUploadCV(ma_viec, ma_cong_ty, ma_nguoi_tim_viec, ma_nguoi_nhan, duong_dan_file_cv_upload);
        }
        //ung tuyen




        //thong bao
        public static List<thong_bao> layDanhSachThongBao(thong_bao_kieu_nguoi_dung tb_knd)
        {
            return chuc_nang_thong_bao_web.layDanhSachThongBao(tb_knd);
        }

        public static List<thong_bao> layDanhSachThongBaoDaAn(thong_bao_kieu_nguoi_dung tb_knd)
        {
            return chuc_nang_thong_bao_web.layDanhSachThongBaoDaAn(tb_knd);
        }

        public static bool anThongBao(trang_thai_thong_bao trang_Thai_Thong_Bao)
        {
            return chuc_nang_thong_bao_web.anThongBao(trang_Thai_Thong_Bao);
        }

        public static bool boAnThongBao(trang_thai_thong_bao trang_Thai_Thong_Bao)
        {
            return chuc_nang_thong_bao_web.boAnThongBao(trang_Thai_Thong_Bao);
        }

        //thong bao



        //otp
        public static bool kiemTraOTPTonTai(string email)
        {
            return chuc_nang_otp_web.kiemTraOTPTonTai(email);
        }

        public static int taoOTPMoi(string email)
        {
            return chuc_nang_otp_web.taoOTPMoi(email);
        }
        //otp



        //goiYTuKhoa
        public static List<thongTinGoiY> goiYTuKhoa(string tu_khoa)
        {
            return chuc_nang_goi_y_web.goiYTuKhoa(tu_khoa);
        }

    }
}