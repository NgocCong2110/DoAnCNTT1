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


namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.viec_lam
{
    public class viec_lam_noi_bat
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<so_luong_nganh_nghe> layNganhNgheNoiBat()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select nganh_nghe, count(*) as so_luong from viec_lam 
            group by nganh_nghe order by so_luong desc";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<so_luong_nganh_nghe>();
            while(reader.Read())
            {
                var sl = new so_luong_nganh_nghe
                {
                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                    so_luong = reader.IsDBNull(reader.GetOrdinal("so_luong")) ? 0 : reader.GetInt32("so_luong")
                };
                danh_sach.Add(sl);
            }
            return danh_sach;
        }
    }
}