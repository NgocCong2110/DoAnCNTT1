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
using DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_cv_nguoi_tim_viec;
using DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_tai_khoan;
using DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_ung_tuyen_nguoi_tim_viec;

namespace DotNet_WEB.Module
{
    public class Module_NTV
    {
        public static List<ung_tuyen> layDanhSachUngTuyen(int ma_Nguoi_Tim_Viec)
        {
            return chuc_nang_ung_tuyen_nguoi_tim_viec_web.layDanhSachUngTuyen(ma_Nguoi_Tim_Viec);
        }

        public static async Task<bool> dangTaiCV(IFormFile cvFile, int ma_Nguoi_Tim_Viec)
        {
            return await chuc_nang_cv_nguoi_tim_viec_web.dangTaiCV(cvFile, ma_Nguoi_Tim_Viec);
        }

        public static List<cv_nguoi_tim_viec> layDanhSachCV(int ma_Nguoi_Tim_Viec)
        {
            return chuc_nang_cv_nguoi_tim_viec_web.layDanhSachCV(ma_Nguoi_Tim_Viec);
        }

        public static List<cv_online_nguoi_tim_viec> layDanhSachCVOnlineNguoiTimViec(int ma_nguoi_tim_viec)
        {
            return chuc_nang_cv_nguoi_tim_viec_web.layDanhSachCVOnlineNguoiTimViec(ma_nguoi_tim_viec);
        }

        public static bool xoaCVNguoiTimViec(cv_online_nguoi_tim_viec cv_Online_Nguoi_Tim_Viec)
        {
            return chuc_nang_cv_nguoi_tim_viec_web.xoaCVNguoiTimViec(cv_Online_Nguoi_Tim_Viec);
        }

        public static bool kiemTraCVUngTuyen(ung_tuyen ung_Tuyen)
        {
            return chuc_nang_cv_nguoi_tim_viec_web.kiemTraCVUngTuyen(ung_Tuyen);
        }

        public static bool capNhatThongTinNguoiTimViec(thong_tin_truong_du_lieu_cap_nhat_ntv req)
        {
            return chuc_nang_tai_khoan_ntv_web.capNhatThongTinNguoiTimViec(req);
        }

        public static async Task<string> capNhatAnhDaiDienNguoiTimViec(IFormFile anh_dai_dien, int ma_nguoi_tim_viec)
        {
            return await chuc_nang_tai_khoan_ntv_web.capNhatAnhDaiDienNguoiTimViec(anh_dai_dien, ma_nguoi_tim_viec);
        }

        public static bool kiemTraMatKhauNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            return chuc_nang_tai_khoan_ntv_web.kiemTraMatKhauNguoiTimViec(nguoi_Tim_Viec);
        } 

        public static bool capNhatMatKhauNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            return chuc_nang_tai_khoan_ntv_web.capNhatMatKhauNguoiTimViec(nguoi_Tim_Viec);
        } 
    }
}