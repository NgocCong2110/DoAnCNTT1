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
using Mysqlx.Crud;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_bai_dang_quan_tri
{
    public class chuc_nang_bai_dang_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<bai_dang_vi_pham> layDanhSachViPham()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select * from bai_dang_vi_pham";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danhSachViPham = new List<bai_dang_vi_pham>();
            while (reader.Read())
            {
                var viPham = new bai_dang_vi_pham
                {
                    ma_bai_vi_pham = reader.IsDBNull(reader.GetOrdinal("ma_bai_vi_pham")) ? 0 : reader.GetInt32("ma_bai_vi_pham"),

                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    ma_nguoi_bao_cao = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_bao_cao")) ? 0 : reader.GetInt32("ma_nguoi_bao_cao"),

                    loai_vi_pham = reader.IsDBNull(reader.GetOrdinal("loai_vi_pham")) ? null : reader.GetString("loai_vi_pham"),

                    noi_dung_bao_cao = reader.IsDBNull(reader.GetOrdinal("noi_dung_bao_cao")) ? null : reader.GetString("noi_dung_bao_cao"),

                    ngay_bao_cao = reader.IsDBNull(reader.GetOrdinal("ngay_bao_cao")) ? DateTime.MinValue : reader.GetDateTime("ngay_bao_cao")
                };
                danhSachViPham.Add(viPham);
            }
            return danhSachViPham;
        }
    }
}