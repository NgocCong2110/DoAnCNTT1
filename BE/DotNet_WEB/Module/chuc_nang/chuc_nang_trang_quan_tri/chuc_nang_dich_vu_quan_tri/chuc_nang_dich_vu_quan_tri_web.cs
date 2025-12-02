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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_dich_vu_quan_tri
{
    public class chuc_nang_dich_vu_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<thanh_toan> layLichSuThanhToan()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from thanh_toan";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thanh_toan>();
            while (reader.Read())
            {
                var ds = new thanh_toan
                {
                    ma_thanh_toan = reader.IsDBNull(reader.GetOrdinal("ma_thanh_toan")) ? 0 : reader.GetInt32("ma_thanh_toan"),

                    ma_don_hang = reader.IsDBNull(reader.GetOrdinal("ma_don_hang")) ? 0 : reader.GetInt32("ma_don_hang"),

                    so_tien = reader.IsDBNull(reader.GetOrdinal("ma_dso_tienon_hang")) ? 0 : reader.GetDecimal("so_tien"),

                    response_code = reader.IsDBNull(reader.GetOrdinal("response_code")) ? null : reader.GetString("response_code"),

                    transaction_no = reader.IsDBNull(reader.GetOrdinal("transaction_no")) ? null : reader.GetString("transaction_no"),

                    bank_code = reader.IsDBNull(reader.GetOrdinal("bank_code")) ? null : reader.GetString("bank_code"),

                    ngay_thanh_toan = reader.IsDBNull(reader.GetOrdinal("ngay_thanh_toan")) ? DateTime.MinValue : reader.GetDateTime("ngay_thanh_toan"),

                    trang_thai_thanh_toan = reader.IsDBNull(reader.GetOrdinal("trang_thai_thanh_toan")) ? TrangThaiThanhToan.None : (TrangThaiThanhToan)Enum.Parse(typeof(TrangThaiThanhToan), reader.GetString("trang_thai_thanh_toan")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(ds);
            }
            return danh_sach;
        }

        public static bool taoDichVuMoi(dich_vu dich_Vu)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "insert into dich_vu (ten_dich_vu, mo_ta, gia) values (@ten_Dich_Vu, @mo_Ta, @gia)";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ten_Dich_Vu", dich_Vu.ten_dich_vu);
            cmd.Parameters.AddWithValue("@gia", dich_Vu.so_tien);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}