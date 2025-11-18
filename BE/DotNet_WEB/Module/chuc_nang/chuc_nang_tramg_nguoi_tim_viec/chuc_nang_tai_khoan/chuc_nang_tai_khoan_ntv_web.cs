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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_tai_khoan
{
    public class chuc_nang_tai_khoan_ntv_web
    {
        private static readonly string chuoi_ket_noi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        private static readonly string[] allowed_fields = new string[]
        {
            "ho_ten",
            "ten_dang_nhap",
            "email",
            "dien_thoai",
            "ngay_sinh",
            "gioi_tinh",
            "trinh_do_hoc_van",
            "dia_chi",
            "quoc_tich",
            "mo_ta",
            "anh_dai_dien"
        };

        public static bool capNhatThongTinNguoiTimViec(thong_tin_truong_du_lieu_cap_nhat_ntv req)
        {
            if (req == null || string.IsNullOrEmpty(req.truong))
                return false;

            if (!allowed_fields.Contains(req.truong))
                return false;

            using var conn = new MySqlConnection(chuoi_ket_noi);
            conn.Open();

            string sql = $"UPDATE nguoi_tim_viec SET {req.truong} = @gia_tri, ngay_cap_nhat = NOW() WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@gia_tri", req.gia_tri);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", req.ma_nguoi_tim_viec);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool kiemTraMatKhauNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_ket_noi);
            coon.Open();
            if(string.IsNullOrEmpty(nguoi_Tim_Viec.mat_khau))
            {
                return false;
            }
            string mat_khau = maHoaMatKhau(nguoi_Tim_Viec.mat_khau);
            string sql = "select count(*) from nguoi_tim_viec where mat_khau = @mat_khau and ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@mat_khau", mat_khau);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", nguoi_Tim_Viec.ma_nguoi_tim_viec);
            int rows = Convert.ToInt32(cmd.ExecuteScalar());
            return rows > 0;
        }

        public static bool capNhatMatKhauNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_ket_noi);
            coon.Open();

            if(string.IsNullOrEmpty(nguoi_Tim_Viec.mat_khau))
            {
                return false;
            }

            string mat_khau_ma_hoa = maHoaMatKhau(nguoi_Tim_Viec.mat_khau);

            using var trans = coon.BeginTransaction();
            try
            {
                string sql_cap_nhat_nguoi_tim_viec = "UPDATE nguoi_tim_viec SET mat_khau = @mat_khau WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using var cmd_cap_nhat_nguoi_tim_viec = new MySqlCommand(sql_cap_nhat_nguoi_tim_viec, coon, trans);
                cmd_cap_nhat_nguoi_tim_viec.Parameters.AddWithValue("@mat_khau", mat_khau_ma_hoa);
                cmd_cap_nhat_nguoi_tim_viec.Parameters.AddWithValue("@ma_nguoi_tim_viec", nguoi_Tim_Viec.ma_nguoi_tim_viec);
                int so_dong_cap_nhat_nguoi_tim_viec = cmd_cap_nhat_nguoi_tim_viec.ExecuteNonQuery();

                string sql_cap_nhat_nguoi_dung = "UPDATE nguoi_dung SET mat_khau = @mat_khau WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using var cmd_cap_nhat_nguoi_dung = new MySqlCommand(sql_cap_nhat_nguoi_dung, coon, trans);
                cmd_cap_nhat_nguoi_dung.Parameters.AddWithValue("@mat_khau", mat_khau_ma_hoa);
                cmd_cap_nhat_nguoi_dung.Parameters.AddWithValue("@ma_nguoi_tim_viec", nguoi_Tim_Viec.ma_nguoi_tim_viec);
                int so_dong_cap_nhat_nguoi_dung = cmd_cap_nhat_nguoi_dung.ExecuteNonQuery();

                trans.Commit();

                return so_dong_cap_nhat_nguoi_tim_viec > 0 || so_dong_cap_nhat_nguoi_dung > 0;
            }
            catch (Exception loi_cap_nhat)
            {
                trans.Rollback();
                Console.WriteLine("Lỗi khi cập nhật mật khẩu: " + loi_cap_nhat.Message);
                return false;
            }
        }

        public static async Task<string> capNhatAnhDaiDienNguoiTimViec(IFormFile file, int ma_nguoi_tim_viec)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File avatar rỗng");
            }
            if (ma_nguoi_tim_viec == 0)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            using var coon = new MySqlConnection(chuoi_ket_noi);
            coon.Open();

            string? anh_dai_dien_cu = "";
            string lay_add_cu = "select anh_dai_dien from nguoi_tim_viec where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using (var cmd_lay_danh_dai_dien_cu = new MySqlCommand(lay_add_cu, coon))
            {
                cmd_lay_danh_dai_dien_cu.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
                var result = await cmd_lay_danh_dai_dien_cu.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    anh_dai_dien_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDienNguoiTimViec");
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

            var duong_dan = $"LuuTruAnhDaiDienNguoiTimViec/{ten_file}";

            string sql = "update nguoi_tim_viec set anh_dai_dien = @duong_dan, ngay_cap_nhat = now() where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan", duong_dan);
                cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
                int rows = await cmd.ExecuteNonQueryAsync();
                if (rows == 0)
                {
                    throw new Exception("Không tìm thấy người dùng để cập nhật");
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

    public class thong_tin_truong_du_lieu_cap_nhat_ntv
    {
        public int ma_nguoi_tim_viec { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
    }
}