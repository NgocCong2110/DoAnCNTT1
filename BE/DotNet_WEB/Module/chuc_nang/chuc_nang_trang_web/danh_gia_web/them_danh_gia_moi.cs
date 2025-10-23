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
    public class them_danh_gia_moi
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool themDanhGiaMoi(danh_gia danh_Gia)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"insert into danh_gia(ma_nguoi_danh_gia, ten_nguoi_danh_gia, so_diem_danh_gia, noi_dung_danh_gia, trang_thai_danh_gia, ngay_tao) 
                values(@ma_nguoi_danh_gia, @ten_nguoi_danh_gia, @so_diem_danh_gia, @noi_dung_danh_gia, @trang_thai_danh_gia, now())";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_danh_gia", danh_Gia.ma_nguoi_danh_gia);
            cmd.Parameters.AddWithValue("@ten_nguoi_danh_gia", danh_Gia.ten_nguoi_danh_gia);
            cmd.Parameters.AddWithValue("@so_diem_danh_gia", danh_Gia.so_diem_danh_gia);
            cmd.Parameters.AddWithValue("@noi_dung_danh_gia", danh_Gia.noi_dung_danh_gia);
            cmd.Parameters.AddWithValue("@trang_thai_danh_gia", "chua_Hien_Thi");
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}