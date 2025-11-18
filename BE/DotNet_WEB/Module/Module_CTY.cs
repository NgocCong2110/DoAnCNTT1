using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using System.Text.Json.Nodes;
using Mysqlx.Crud;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Tls;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty;
using System.Security.Cryptography.X509Certificates;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_viec_lam_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_phuc_loi_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_mang_xa_hoi_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_thong_ke_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_ung_vien_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_dich_vu_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_bai_dang_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_thong_bao_cong_ty;
using System.Threading.Tasks;

namespace DotNet_WEB.Module
{
    public class Module_CTY
    {
        //ung vien
        public static List<ung_tuyen> layDanhSachUngVien(int ma_Cong_Ty)
        {
            return chuc_nang_ung_vien_cong_ty_web.layDanhSachUngVien(ma_Cong_Ty);
        }

        public static bool tuChoiUngVien(ung_tuyen ung_Tuyen)
        {
            return chuc_nang_ung_vien_cong_ty_web.tuChoiUngVien(ung_Tuyen);
        }

        public static bool xoaUngVien(ung_tuyen ung_Tuyen)
        {
            return chuc_nang_ung_vien_cong_ty_web.xoaUngVien(ung_Tuyen);
        }
        
        public static bool guiThuMoiPhongVan(thong_tin_phong_van ttpv)
        {
            return chuc_nang_ung_vien_cong_ty_web.guiThuMoiPhongVan(ttpv);
        }
        //ung vien


        public static List<bai_dang> layBaiDangTheoIDCongTy(int ma_Cong_Ty)
        {
            return chuc_nang_bai_dang_cong_ty_web.layBaiDangTheoIDCongTy(ma_Cong_Ty);
        }


        //thong ke
        public static List<thong_ke_ung_vien> laySoLuongUngVien(int ma_cong_ty)
        {
            return chuc_nang_thong_ke_cong_ty_web.laySoLuongUngVien(ma_cong_ty);
        }

        public static List<thong_ke_bai_dang_cua_cong_ty> laySoLuongBaiDangCuaCongTy(int ma_cong_ty)
        {
            return chuc_nang_thong_ke_cong_ty_web.laySoLuongBaiDangCuaCongTy(ma_cong_ty);
        }

        //thong ke


        //tai khoan
        public static bool kiemTraMatKhauCongTy(cong_ty cong_Ty)
        {
            return chuc_nang_tai_khoan_cong_ty_web.kiemTraMatKhauCongTy(cong_Ty);
        }

        public static bool capNhatMatKhauCongTy(cong_ty cong_Ty)
        {
            return chuc_nang_tai_khoan_cong_ty_web.capNhatMatKhauCongTy(cong_Ty);
        }

        public static bool capNhatMoTaCongTy(cong_ty cong_Ty)
        {
            return chuc_nang_tai_khoan_cong_ty_web.capNhatMoTaCongTy(cong_Ty);
        }

        public static async Task<string> capNhatLogoCongTy(IFormFile file, int ma_cong_ty)
        {
            return await chuc_nang_tai_khoan_cong_ty_web.capNhatLogoCongTy(file, ma_cong_ty);
        }

        public static bool capNhatThongTinCongTy(thong_tin_truong_du_lieu_cap_nhat req)
        {
            return chuc_nang_tai_khoan_cong_ty_web.capNhatThongTinCongTy(req);
        }

        public static List<cong_ty> layThongTinCongTy(int ma_cong_ty)
        {
            return chuc_nang_tai_khoan_cong_ty_web.layThongTinCongTy(ma_cong_ty);
        }

        public static List<viec_lam> layDanhSachViecLamCuaCongTy(int ma_cong_ty)
        {
            return chuc_nang_viec_lam_cong_ty_web.layDanhSachViecLamCuaCongTy(ma_cong_ty);
        }

        public static List<ung_tuyen> layDanhSachViecLamNoiBatCuaCongTy(int ma_cong_ty)
        {
            return chuc_nang_viec_lam_cong_ty_web.layDanhSachViecLamNoiBatCuaCongTy(ma_cong_ty);
        }

        public static async Task<string> capNhatAnhBiaCongTy(IFormFile file, int ma_cong_ty)
        {
            return await chuc_nang_tai_khoan_cong_ty_web.capNhatAnhBiaCongTy(file, ma_cong_ty);
        }

        public static bool capNhatPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            return chuc_nang_phuc_loi_cong_ty_web.capNhatPhucLoiCongTy(phuc_Loi_Cong_Ty);
        }

        public static bool xoaPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            return chuc_nang_phuc_loi_cong_ty_web.xoaPhucLoiCongTy(phuc_Loi_Cong_Ty);
        }

        public static bool capNhatLienKetMangXaHoi(mang_xa_hoi_cong_ty_cap_nhat mang_Xa_Hoi_Cong_Ty_Cap_Nhat)
        {
            return chuc_nang_mang_xa_hoi_cong_ty_web.capNhatLienKetMangXaHoi(mang_Xa_Hoi_Cong_Ty_Cap_Nhat);
        }
        //tai khoan
        
        
        //dich vu
        public static List<dich_vu> layDanhSachDichVu()
        {
            return chuc_nang_dich_vu_cong_ty_web.layDanhSachDichVu();
        }
        //dich vu

        //thong bao
        public static bool guiThongBaoViecLamMoi(thong_bao thong_Bao)
        {
            return chuc_nang_thong_bao_cong_ty_web.guiThongBaoViecLamMoi(thong_Bao);
        }

        public static bool xoaThongBaoCongTy(thong_bao thong_Bao)
        {
            return chuc_nang_thong_bao_cong_ty_web.xoaThongBaoCongTy(thong_Bao);
        }

        public static List<thong_bao> thongBaoCongTyRieng(int ma_cong_ty)
        {
            return chuc_nang_thong_bao_cong_ty_web.thongBaoCongTyRieng(ma_cong_ty);
        }
        //thong bao
    }
}



