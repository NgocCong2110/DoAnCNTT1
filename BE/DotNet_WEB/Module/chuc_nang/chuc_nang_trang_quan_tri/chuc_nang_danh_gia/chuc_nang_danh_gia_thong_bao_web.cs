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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_danh_gia
{
    public class chuc_nang_danh_gia_thong_bao_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<danh_gia> layToanBoDanhSachDanhGia()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from danh_gia";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<danh_gia>();
            while (reader.Read())
            {
                var danh_Gia = new danh_gia
                {
                    ma_danh_gia = reader.IsDBNull(reader.GetOrdinal("ma_danh_gia")) ? 0 : reader.GetInt32("ma_danh_gia"),

                    ma_nguoi_danh_gia = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_danh_gia")) ? 0 : reader.GetInt32("ma_nguoi_danh_gia"),

                    ten_nguoi_danh_gia = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_danh_gia")) ? null : reader.GetString("ten_nguoi_danh_gia"),

                    so_diem_danh_gia = reader.IsDBNull(reader.GetOrdinal("so_diem_danh_gia")) ? 0 : reader.GetInt32("so_diem_danh_gia"),

                    noi_dung_danh_gia = reader.IsDBNull(reader.GetOrdinal("noi_dung_danh_gia")) ? null : reader.GetString("noi_dung_danh_gia"),

                    trang_thai_danh_gia = reader.IsDBNull(reader.GetOrdinal("trang_thai_danh_gia")) ? TrangThaiDanhGia.chua_Hien_Thi : (TrangThaiDanhGia)Enum.Parse(typeof(TrangThaiDanhGia), reader.GetString("trang_thai_danh_gia")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(danh_Gia);
            }
            return danh_sach;
        }

        public static bool capNhatTrangThaiDanhGia(danh_gia danh_Gia)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "update danh_gia set trang_thai_danh_gia = @trang_thai_danh_gia where ma_danh_gia = @ma_danh_gia";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@trang_thai_danh_gia", danh_Gia.trang_thai_danh_gia.ToString());
            cmd.Parameters.AddWithValue("@ma_danh_gia", danh_Gia.ma_danh_gia);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}