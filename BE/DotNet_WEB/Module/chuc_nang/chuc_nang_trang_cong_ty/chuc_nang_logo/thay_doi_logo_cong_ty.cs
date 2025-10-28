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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_logo
{

    public class thay_doi_logo_cong_ty
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static async Task<string> capNhatLogoCongTy(IFormFile file, int ma_cong_ty)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string logo_cu = "";
            string lay_lo_cu = "select logo from cong_ty where ma_cong_ty = @ma_cong_ty";
            using (var cmd_lay_logo = new MySqlCommand(lay_lo_cu, coon))
            {
                cmd_lay_logo.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                var result = await cmd_lay_logo.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    logo_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruLogoCongTy");
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

            var duong_dan = $"LuuTruLogoCongTy/{ten_file}";

            string sql = "Update cong_ty set logo = @duong_dan_logo where ma_cong_ty = @ma_cong_ty";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan_logo", duong_dan);
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                await cmd.ExecuteNonQueryAsync();
            }

            if (!string.IsNullOrEmpty(logo_cu))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), logo_cu.Replace("/", "\\"));
                if (File.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
            }
            return duong_dan;
        }
    }
}