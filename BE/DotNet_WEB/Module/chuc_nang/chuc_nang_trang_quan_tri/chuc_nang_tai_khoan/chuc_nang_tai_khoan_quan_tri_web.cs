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
using System.Text;
using System.Security.Cryptography;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_tai_khoan
{
    public class chuc_nang_tai_khoan_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static bool kiemTraMatKhauQuanTri(quan_tri quan_Tri)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string mat_khau = maHoaMatKhau(quan_Tri.mat_khau ?? "");
            string sql = "SELECT COUNT(*) FROM quan_tri WHERE ma_quan_tri = @ma_quan_tri AND mat_khau = @mat_khau";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma_quan_tri", quan_Tri.ma_quan_tri);
            cmd.Parameters.AddWithValue("@mat_khau", mat_khau);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        public static bool capNhatMatKhauQuanTri(quan_tri quan_Tri)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string mat_khau = maHoaMatKhau(quan_Tri.mat_khau ?? "");
            string sql = "UPDATE quan_tri SET mat_khau = @mat_khau, ngay_cap_nhat = NOW() WHERE ma_quan_tri = @ma_quan_tri";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@mat_khau", mat_khau);
            cmd.Parameters.AddWithValue("@ma_quan_tri", quan_Tri.ma_quan_tri);
            return cmd.ExecuteNonQuery() > 0;
        }

        private static readonly string[] allowed_fields = new string[]
        {
            "ho_ten",
            "ten_dang_nhap",
            "email",
            "dien_thoai",
            "ngay_sinh",
            "dia_chi",
            "duong_dan_anh_dai_dien"
        };
        public static bool capNhatThongTinQuanTri(thong_tin_truong_du_lieu_cap_nhat_quan_tri req)
        {
            if (req == null || string.IsNullOrEmpty(req.truong))
                return false;

            if (!allowed_fields.Contains(req.truong))
                return false;

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string sql = $"UPDATE quan_tri SET {req.truong} = @gia_tri, ngay_cap_nhat = NOW() WHERE ma_quan_tri = @ma_quan_tri";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@gia_tri", req.gia_tri);
            cmd.Parameters.AddWithValue("@ma_quan_tri", req.ma_quan_tri);

            return cmd.ExecuteNonQuery() > 0;
        }

         public static async Task<string> capNhatAnhDaiDienQuanTriVien(IFormFile file, int ma_quan_tri)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File avatar rỗng");
            }
            if(ma_quan_tri == 0)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string? anh_dai_dien_cu = "";
            string lay_add_cu = "select duong_dan_anh_dai_dien from quan_tri where ma_quan_tri = @ma_quan_tri";
            using (var cmd_lay_danh_dai_dien_cu = new MySqlCommand(lay_add_cu, coon))
            {
                cmd_lay_danh_dai_dien_cu.Parameters.AddWithValue("@ma_quan_tri", ma_quan_tri);
                var result = await cmd_lay_danh_dai_dien_cu.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    anh_dai_dien_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDienQuanTriVien");
            if (!Directory.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory(duong_dan_folder);
            }
            var ten_file = $"{Guid.NewGuid()}_{file.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruAnhDaiDienQuanTriVien/{ten_file}";

            string sql = "update quan_tri set duong_dan_anh_dai_dien = @duong_dan, ngay_cap_nhat = now() where ma_quan_tri = @ma_quan_tri";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan", duong_dan);
                cmd.Parameters.AddWithValue("@ma_quan_tri", ma_quan_tri);
                int rows = await cmd.ExecuteNonQueryAsync();
                if(rows == 0)
                {
                    throw new Exception("Không tìm thấy quản trị để cập nhật");
                }
            }
            if (!string.IsNullOrEmpty(anh_dai_dien_cu))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), anh_dai_dien_cu.Replace("/", "\\"));
                if (File.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
            }
            return duong_dan;
        }

        public static string maHoaMatKhau(string mat_khau)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(mat_khau);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
    public class thong_tin_truong_du_lieu_cap_nhat_quan_tri
    {
        public int ma_quan_tri { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
    }
}