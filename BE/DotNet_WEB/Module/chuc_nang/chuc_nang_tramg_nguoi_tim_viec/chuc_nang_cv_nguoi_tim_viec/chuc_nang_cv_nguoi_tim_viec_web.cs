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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_cv_nguoi_tim_viec
{
    public class chuc_nang_cv_nguoi_tim_viec_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<cv_online_nguoi_tim_viec> layDanhSachCVOnlineNguoiTimViec(int ma_nguoi_tim_viec)
        {
            var danh_sach = new List<cv_online_nguoi_tim_viec>();

            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "SELECT * FROM cv_online_nguoi_tim_viec WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var cv_ntv = new cv_online_nguoi_tim_viec
                {
                    ma_cv = reader.IsDBNull(reader.GetOrdinal("ma_cv")) ? 0 : reader.GetInt32("ma_cv"),
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),
                    ten_cv = reader.IsDBNull(reader.GetOrdinal("ten_cv")) ? null : reader.GetString("ten_cv"),
                    anh_dai_dien = reader.IsDBNull(reader.GetOrdinal("anh_dai_dien")) ? null : reader.GetString("anh_dai_dien"),
                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? null : reader.GetString("dien_thoai"),
                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi"),
                    chuyen_nganh = reader.IsDBNull(reader.GetOrdinal("chuyen_nganh")) ? null : reader.GetString("chuyen_nganh"),
                    ky_nang = reader.IsDBNull(reader.GetOrdinal("ky_nang")) ? null : reader.GetString("ky_nang"),
                    du_an = reader.IsDBNull(reader.GetOrdinal("du_an")) ? null : reader.GetString("du_an"),
                    muc_tieu = reader.IsDBNull(reader.GetOrdinal("muc_tieu")) ? null : reader.GetString("muc_tieu"),
                    vi_tri_ung_tuyen = reader.IsDBNull(reader.GetOrdinal("vi_tri_ung_tuyen")) ? null : reader.GetString("vi_tri_ung_tuyen"),
                    duong_dan_file_pdf = reader.IsDBNull(reader.GetOrdinal("duong_dan_file_pdf")) ? null : reader.GetString("duong_dan_file_pdf"),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                    ngay_sinh = reader.IsDBNull(reader.GetOrdinal("ngay_sinh")) ? DateTime.MinValue : reader.GetDateTime("ngay_sinh"),
                    gioi_tinh = reader.IsDBNull(reader.GetOrdinal("gioi_tinh"))
                        ? GioiTinh.None
                        : Enum.TryParse<GioiTinh>(reader.GetString("gioi_tinh"), out var gt) ? gt : GioiTinh.None
                };

                danh_sach.Add(cv_ntv);
            }

            return danh_sach;
        }

        public static bool xoaCVNguoiTimViec(cv_online_nguoi_tim_viec cv_Online_Nguoi_Tim_Viec)
        {
            
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string duong_dan_file_pdf = "";
            string lay_cv = "select duong_dan_file_pdf from cv_online_nguoi_tim_viec where ma_cv = @ma_cv and ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using(var cmd_lay_cv = new MySqlCommand(lay_cv, coon))
            {
                cmd_lay_cv.Parameters.AddWithValue("@ma_nguoi_tim_viec", cv_Online_Nguoi_Tim_Viec.ma_nguoi_tim_viec);
                cmd_lay_cv.Parameters.AddWithValue("@ma_cv", cv_Online_Nguoi_Tim_Viec.ma_cv);
                using var reader = cmd_lay_cv.ExecuteReader();
                if (reader.Read())
                {
                    duong_dan_file_pdf = reader["duong_dan_file_pdf"]?.ToString();
                }
            }
            if (!string.IsNullOrEmpty(duong_dan_file_pdf))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), duong_dan_file_pdf.Replace("/", "\\"));
                if (File.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
                string sql = "delete from cv_online_nguoi_tim_viec where ma_cv = @ma_cv and ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using var cmd_xoa = new MySqlCommand(sql, coon);
                cmd_xoa.Parameters.AddWithValue("@ma_cv", cv_Online_Nguoi_Tim_Viec.ma_cv);
                cmd_xoa.Parameters.AddWithValue("@ma_nguoi_tim_viec", cv_Online_Nguoi_Tim_Viec.ma_nguoi_tim_viec);
                return cmd_xoa.ExecuteNonQuery() > 0;
            }
            return false;
        }

    }
}