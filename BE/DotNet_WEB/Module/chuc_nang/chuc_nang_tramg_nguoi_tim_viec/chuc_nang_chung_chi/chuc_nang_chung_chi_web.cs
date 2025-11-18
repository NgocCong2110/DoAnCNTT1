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
using System.Runtime.CompilerServices;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_chung_chi
{
    public class chuc_nang_chung_chi_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<chung_chi> layDanhSachChungChi(int ma_nguoi_tim_viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from chung_chi where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
            var danh_sach = new List<chung_chi>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var chung_Chi = new chung_chi
                {
                    ma_chung_chi = reader.IsDBNull(reader.GetOrdinal("ma_chung_chi")) ? 0 : reader.GetInt32("ma_chung_chi"),

                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ten_chung_chi = reader.IsDBNull(reader.GetOrdinal("ten_chung_chi")) ? null : reader.GetString("ten_chung_chi"),

                    don_vi_cap = reader.IsDBNull(reader.GetOrdinal("don_vi_cap")) ? null : reader.GetString("don_vi_cap"),

                    ngay_cap = reader.IsDBNull(reader.GetOrdinal("ngay_cap")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap"),

                    ngay_het_han = reader.IsDBNull(reader.GetOrdinal("ngay_het_han")) ? DateTime.MinValue : reader.GetDateTime("ngay_het_han"),

                    ten_tep = reader.IsDBNull(reader.GetOrdinal("ten_tep")) ? null : reader.GetString("ten_tep"),

                    duong_dan_file = reader.IsDBNull(reader.GetOrdinal("duong_dan_file")) ? null : reader.GetString("duong_dan_file"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(chung_Chi);
            }
            return danh_sach;
        }

        public static async Task<bool> dangTaiChungChi(int ma_nguoi_tim_viec, string ten_chung_chi, string don_vi_cap, DateTime ngay_cap, DateTime ngay_het_han, IFormFile ten_tep)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruChungChiNguoiTimViec");
            if (!Directory.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory(duong_dan_folder);
            }
            var ten_file = $"{Guid.NewGuid()}_{ten_tep.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await ten_tep.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruChungChiNguoiTimViec/{ten_file}";
            string sql = @"INSERT INTO chung_chi
                   (ma_nguoi_tim_viec, ten_chung_chi, don_vi_cap, ngay_cap, ngay_het_han, duong_dan_file)
                   VALUES (@ma_nguoi_tim_viec, @ten_chung_chi, @don_vi_cap, @ngay_cap, @ngay_het_han, @duong_dan_file)";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@ten_chung_chi", ten_chung_chi);
            cmd.Parameters.AddWithValue("@don_vi_cap", don_vi_cap);
            cmd.Parameters.AddWithValue("@ngay_cap", ngay_cap);
            cmd.Parameters.AddWithValue("@ngay_het_han", ngay_het_han);
            cmd.Parameters.AddWithValue("@duong_dan_file", duong_dan);
            try
            {
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu vào DB: " + ex.Message);
                return false;
            }
        }

        public static async Task<bool> xoaChungChi(chung_chi chung_Chi)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string? ten_file_chung_chi_xoa = "";
            string sql = "select duong_dan_file from chung_chi where ma_chung_chi = @ma_chung_chi and ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@ma_chung_chi", chung_Chi.ma_chung_chi);
                cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", chung_Chi.ma_nguoi_tim_viec);
                var res = await cmd.ExecuteScalarAsync();
                if (res != null && res != DBNull.Value)
                {
                    ten_file_chung_chi_xoa = res.ToString();
                }
            }

            if (!string.IsNullOrEmpty(ten_file_chung_chi_xoa))
            {
                var chung_chi_xoa = Path.Combine(Directory.GetCurrentDirectory(), ten_file_chung_chi_xoa.Replace("/", "\\"));
                if (File.Exists(chung_chi_xoa))
                {
                    File.Delete(chung_chi_xoa);
                }
                string sql_xoa_chung_chi = "delete from chung_chi where ma_chung_chi = @ma_chung_chi and ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using (var cmd = new MySqlCommand(sql_xoa_chung_chi, coon))
                {
                    cmd.Parameters.AddWithValue("@ma_chung_chi", chung_Chi.ma_chung_chi);
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", chung_Chi.ma_nguoi_tim_viec);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
            return false;
        }
    }
}