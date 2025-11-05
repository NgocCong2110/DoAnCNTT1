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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_mang_xa_hoi_cong_ty
{
    public class chuc_nang_mang_xa_hoi_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool capNhatLienKetMangXaHoi(mang_xa_hoi_cong_ty_cap_nhat mxh)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"INSERT INTO lien_ket_mang_xa_hoi (ma_cong_ty, ten_mang_xa_hoi, duong_dan)
            VALUES (@ma_cong_ty, @ten_mang_xa_hoi, @duong_dan)";
            int r = 0;
            foreach (var mxhct in mxh.lien_Ket_Mang_Xa_Hoi)
            {
                using (var cmd = new MySqlCommand(sql, coon))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", mxh.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@ten_mang_xa_hoi", mxhct.ten_mang_xa_hoi);
                    cmd.Parameters.AddWithValue("@duong_dan", mxhct.duong_dan);
                    r += cmd.ExecuteNonQuery();
                }
            }
            return r > 0;
        }
        
        public static bool xoaLienKetMangXaHoiCongTy(mang_xa_hoi_cong_ty_cap_nhat mxh)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"delete from lien_ket_mang_xa_hoi where ma_cong_ty = @ma_cong_ty and ma_lien_ket = @ma_lien_ket";
            int r = 0;
            foreach(var mxhct in mxh.lien_Ket_Mang_Xa_Hoi)
            {
                using (var cmd = new MySqlCommand(sql, coon))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", mxh.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@ma_phuc_loi_cong_ty", mxhct.ma_lien_ket);
                    r += cmd.ExecuteNonQuery();
                }
            }
            return r > 0;
        }
    }

    public class mang_xa_hoi_cong_ty_cap_nhat
    {
        public int ma_cong_ty { get; set; }
        public List<lien_ket_mang_xa_hoi>? lien_Ket_Mang_Xa_Hoi { get; set; }
    } 
}