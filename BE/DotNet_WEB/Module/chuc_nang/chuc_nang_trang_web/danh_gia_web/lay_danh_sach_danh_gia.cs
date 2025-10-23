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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.danh_gia_web
{
    public class lay_danh_sach_danh_gia
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<danh_gia> layDanhSachDanhGia()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from danh_gia where trang_thai_danh_gia = 'dang_Hien_Thi'";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<danh_gia>();
            while (reader.Read())
            {
                var danh_Gia = new danh_gia
                {
                    ma_nguoi_danh_gia = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_danh_gia")) ? 0 : reader.GetInt32("ma_nguoi_danh_gia"),

                    ten_nguoi_danh_gia = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_danh_gia")) ? null : reader.GetString("ten_nguoi_danh_gia"),

                    so_diem_danh_gia = reader.IsDBNull(reader.GetOrdinal("so_diem_danh_gia")) ? 0 : reader.GetInt32("so_diem_danh_gia"),

                    noi_dung_danh_gia = reader.IsDBNull(reader.GetOrdinal("noi_dung_danh_gia")) ? null : reader.GetString("noi_dung_danh_gia"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(danh_Gia);
            }
            return danh_sach;
        }
    }
}