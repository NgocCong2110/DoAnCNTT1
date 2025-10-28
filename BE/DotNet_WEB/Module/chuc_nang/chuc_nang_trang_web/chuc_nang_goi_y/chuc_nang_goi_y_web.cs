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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_goi_y
{
    public class chuc_nang_goi_y_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<thongTinGoiY> goiYTuKhoa(string tu_khoa)
        {
            var danh_sach = new List<thongTinGoiY>();

            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            tu_khoa = Normalize(tu_khoa);

            string sql = @"
        SELECT DISTINCT nganh_nghe AS ten, NULL AS logo, 'viec_lam' AS loai
        FROM viec_lam
        WHERE LOWER(
            REPLACE(
                REPLACE(
                    REPLACE(nganh_nghe, ' ', ''),
                '_', ''),
            '-', '')
        ) LIKE @tu_khoa

        UNION ALL

        SELECT ten_cong_ty AS ten, logo AS logo, 'cong_ty' AS loai
        FROM cong_ty
        WHERE LOWER(
            REPLACE(
                REPLACE(
                    REPLACE(ten_cong_ty, ' ', ''),
                '_', ''),
            '-', '')
        ) LIKE @tu_khoa
    ";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@tu_khoa", "%" + tu_khoa + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var goi_y = new thongTinGoiY
                {
                    ten = reader.IsDBNull(reader.GetOrdinal("ten")) ? null : reader.GetString("ten"),
                    loai = reader.IsDBNull(reader.GetOrdinal("loai")) ? null : reader.GetString("loai"),
                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                };
                danh_sach.Add(goi_y);
            }

            return danh_sach;
        }




        public static string? Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            result = Regex.Replace(result, @"[\s_\-.,/]+", "");

            return string.IsNullOrWhiteSpace(result) ? null : result;
        }
    }
    public class thongTinGoiY
    {
        public string? ten { get; set; }
        public string? loai { get; set; }
        public string? logo { get; set; }
    }
}