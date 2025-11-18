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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_phuc_loi_cong_ty
{
    public class chuc_nang_phuc_loi_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool capNhatPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat pl)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            if (pl == null || pl.phuc_Loi_Cong_Ty == null || pl.phuc_Loi_Cong_Ty.Count == 0)
            {
                return false;
            }
            string sql = @"INSERT INTO phuc_loi_cong_ty (ma_cong_ty, ten_phuc_loi, mo_ta)
            VALUES (@ma_cong_ty, @ten_phuc_loi, @mo_ta)";
            int r = 0;
            foreach (var plct in pl.phuc_Loi_Cong_Ty)
            {
                using (var cmd = new MySqlCommand(sql, coon))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", pl.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@ten_phuc_loi", plct.ten_phuc_loi);
                    cmd.Parameters.AddWithValue("@mo_ta", plct.mo_ta);
                    r += cmd.ExecuteNonQuery();
                }
            }
            return r > 0;
        }

        public static bool xoaPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat pl)
        {
            if (pl == null || pl.phuc_Loi_Cong_Ty == null || pl.phuc_Loi_Cong_Ty.Count == 0)
                return false;
                
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"delete from phuc_loi_cong_ty where ma_cong_ty = @ma_cong_ty and ma_phuc_loi_cty = @ma_phuc_loi_cong_ty";
            int r = 0;
            foreach (var plct in pl.phuc_Loi_Cong_Ty)
            {
                using (var cmd = new MySqlCommand(sql, coon))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", pl.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@ma_phuc_loi_cong_ty", plct.ma_phuc_loi_cty);
                    r += cmd.ExecuteNonQuery();
                }
            }
            return r > 0;
        }
    }

    public class phuc_loi_cong_ty_cap_nhat
    {
        public int ma_cong_ty { get; set; }
        public List<phuc_loi_cong_ty>? phuc_Loi_Cong_Ty { get; set; }
    }
}