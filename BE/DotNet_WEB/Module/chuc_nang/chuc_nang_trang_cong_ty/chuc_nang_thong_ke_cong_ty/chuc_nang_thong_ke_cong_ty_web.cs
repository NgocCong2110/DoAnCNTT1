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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_thong_ke_cong_ty
{
    public class chuc_nang_thong_ke_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<thong_ke_ung_vien> laySoLuongUngVien(int ma_Cong_Ty)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = @" select count(*) as so_luong_ung_vien, month(ngay_ung_tuyen) as thang from ung_tuyen a  where ma_cong_ty = @ma_Cong_Ty group by month(ngay_ung_tuyen) order by thang";

            using var cmd = new MySqlCommand(layDanhSach, conn);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thong_ke_ung_vien>();

            while (reader.Read())
            {
                var ung_Vien = new thong_ke_ung_vien
                {
                    so_luong_ung_vien = reader.IsDBNull(reader.GetOrdinal("so_luong_ung_vien")) ? 0 : reader.GetInt32("so_luong_ung_vien"),
                    thang = reader.IsDBNull(reader.GetOrdinal("thang")) ? 0 : reader.GetInt32("thang")
                };
                danh_sach.Add(ung_Vien);
            }
            return danh_sach;
        }

        public static List<thong_ke_bai_dang_cua_cong_ty> laySoLuongBaiDangCuaCongTy(int ma_cong_ty)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = @"select count(*) as so_luong, month(ngay_tao) as thang from bai_dang where ma_nguoi_dang = 1 group by month(ngay_tao) order by thang";

            using var cmd = new MySqlCommand(layDanhSach, conn);
            cmd.Parameters.AddWithValue("@ma_nguoi_dang", ma_cong_ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thong_ke_bai_dang_cua_cong_ty>();

            while (reader.Read())
            {
                var ung_Vien = new thong_ke_bai_dang_cua_cong_ty
                {
                    so_luong_bai_dang = reader.IsDBNull(reader.GetOrdinal("so_luong_bai_dang")) ? 0 : reader.GetInt32("so_luong_bai_dang"),
                    thang = reader.IsDBNull(reader.GetOrdinal("thang")) ? 0 : reader.GetInt32("thang")
                };
                danh_sach.Add(ung_Vien);
            }
            return danh_sach;
        }
    }
    public class thong_ke_ung_vien
    {
        public int so_luong_ung_vien { get; set; }
        public int thang { get; set; }
    }

    public class thong_ke_bai_dang_cua_cong_ty
    {
        public int so_luong_bai_dang { get; set; }
        public int thang { get; set; }
    }
}