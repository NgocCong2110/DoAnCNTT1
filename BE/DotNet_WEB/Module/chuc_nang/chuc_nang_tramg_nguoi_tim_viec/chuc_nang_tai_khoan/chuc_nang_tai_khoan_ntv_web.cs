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

        public static async Task<string> capNhatAnhDaiDienNguoiTimViec(IFormFile file, int ma_nguoi_tim_viec)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("File avatar rỗng");
            }
            if(ma_nguoi_tim_viec == 0)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            using var coon = new MySqlConnection(chuoi_ket_noi);
            coon.Open();

            string anh_dai_dien_cu = "";
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

            string sql = "update nguoi_tim_viec set anh_dai_dien = @duong_dan where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan", duong_dan);
                cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
                int rows = await cmd.ExecuteNonQueryAsync();
                if(rows == 0)
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
    }

    public class thong_tin_truong_du_lieu_cap_nhat_ntv
    {
        public int ma_nguoi_tim_viec { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
    }
}