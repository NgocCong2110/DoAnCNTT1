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
using Mysqlx.Crud;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_anh_bia_cong_ty
{
    public class chuc_nang_anh_bia_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static async Task<string> capNhatAnhBiaCongTy(IFormFile file, int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string anh_bia_cu = "";
            string lay_anh_bia_cu = "select anh_bia from cong_ty where ma_cong_ty = @ma_cong_ty";
            using ( var cmd = new MySqlCommand(lay_anh_bia_cu, coon))
            {
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    anh_bia_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhBiaCongTy");
            if (!Path.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory("LuuTruAnhBiaCongTy");
            }

            var ten_file = $"{Guid.NewGuid()}_{file.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruAnhBiaCongTy/{ten_file}";

            string sql = "update cong_ty set anh_bia = @duong_dan where ma_cong_ty = @ma_cong_ty";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan", duong_dan);
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                await cmd.ExecuteNonQueryAsync();
            }

            if (!string.IsNullOrEmpty(anh_bia_cu))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), anh_bia_cu.Replace("/", "\\"));
                if (Path.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
            }
            return duong_dan;
        }
    }
}