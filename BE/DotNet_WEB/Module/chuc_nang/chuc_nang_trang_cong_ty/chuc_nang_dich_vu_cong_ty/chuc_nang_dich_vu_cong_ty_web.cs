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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_dich_vu_cong_ty
{
    public class chuc_nang_dich_vu_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<dich_vu> layDanhSachDichVu()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"Select * from dich_vu";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<dich_vu>();
            while (reader.Read())
            {
                var dv = new dich_vu
                {
                    ma_dich_vu = reader.IsDBNull(reader.GetOrdinal("ma_dich_vu")) ? 0 : reader.GetInt32("ma_dich_vu"),

                    ten_dich_vu = reader.IsDBNull(reader.GetOrdinal("ten_dich_vu")) ? null : reader.GetString("ten_dich_vu"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    gia = reader.IsDBNull(reader.GetOrdinal("gia")) ? 0 : reader.GetDecimal("gia"),
                };
                danh_sach.Add(dv);
            }
            return danh_sach;
        }
    }
}