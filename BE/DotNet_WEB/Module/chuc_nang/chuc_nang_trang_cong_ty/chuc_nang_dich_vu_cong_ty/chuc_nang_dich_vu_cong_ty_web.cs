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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_dich_vu_cong_ty
{
    public class chuc_nang_dich_vu_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<thanh_toan_dich_vu> layDanhSachDichVu(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
        SELECT ttdv.ma_cong_ty, ttdv.ma_dich_vu, ttdv.trang_thai_thanh_toan,
               c.ten_cong_ty,
               dv.ten_dich_vu, dv.so_tien, dv.ngay_tao
        FROM thanh_toan_dich_vu ttdv
        JOIN cong_ty c ON ttdv.ma_cong_ty = c.ma_cong_ty
        JOIN dich_vu dv ON ttdv.ma_dich_vu = dv.ma_dich_vu
        WHERE ttdv.ma_cong_ty = @ma_cong_ty";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thanh_toan_dich_vu>();

            while (reader.Read())
            {
                var ttDv = new thanh_toan_dich_vu
                {
                    ma_cong_ty = reader.GetInt32("ma_cong_ty"),
                    cong_Ty = new cong_ty
                    {
                        ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    },
                    ma_dich_vu = reader.IsDBNull(reader.GetOrdinal("ma_dich_vu")) ? null : reader.GetString("ma_dich_vu"),
                    dich_Vu = new dich_vu
                    {
                        ma_dich_vu = reader.IsDBNull(reader.GetOrdinal("ma_dich_vu")) ? null : reader.GetString("ma_dich_vu"),
                        ten_dich_vu = reader.IsDBNull(reader.GetOrdinal("ten_dich_vu")) ? null : reader.GetString("ten_dich_vu"),
                        so_tien = reader.IsDBNull(reader.GetOrdinal("so_tien")) ? 0 : reader.GetDecimal("so_tien"),
                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                    },
                    trang_thai_thanh_toan = reader.IsDBNull(reader.GetOrdinal("trang_thai_thanh_toan"))
                        ? TrangThaiThanhToan.chua_Thanh_Toan
                        : (TrangThaiThanhToan)Enum.Parse(typeof(TrangThaiThanhToan), reader.GetString("trang_thai_thanh_toan"))
                };

                danh_sach.Add(ttDv);
            }

            return danh_sach;
        }

    }
}