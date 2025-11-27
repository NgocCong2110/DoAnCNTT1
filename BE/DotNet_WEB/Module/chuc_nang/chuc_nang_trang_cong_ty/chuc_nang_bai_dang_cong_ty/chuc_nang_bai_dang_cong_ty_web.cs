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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_bai_dang_cong_ty
{
    public class chuc_nang_bai_dang_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<bai_dang> layBaiDangTheoIDCongTy(int ma_Cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from bai_dang where ma_nguoi_dang = @ma_Cong_Ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach_bai_dang = new List<bai_dang>();

            while (reader.Read())
            {
                var baiDang = new bai_dang
                {
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                };

                danh_sach_bai_dang.Add(baiDang);
            }
            return danh_sach_bai_dang;
        }



        public static bool anBaiDangCongTy(bai_dang bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "update bai_dang set trang_thai = 2 where ma_nguoi_dang = @ma_nguoi_dang and ma_bai_dang = @ma_bai_dang";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_dang", bai_Dang.ma_nguoi_dang);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang.ma_bai_dang);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool boAnBaiDangCongTy(bai_dang bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "update bai_dang set trang_thai = 1 where ma_nguoi_dang = @ma_nguoi_dang and ma_bai_dang = @ma_bai_dang";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_dang", bai_Dang.ma_nguoi_dang);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang.ma_bai_dang);
            return cmd.ExecuteNonQuery() > 0;
        }

        private static readonly string[] allowed_field_viec_lam = new string[]
        {
            "nganh_nghe",
            "vi_tri",
            "kinh_nghiem",
            "muc_luong",
            "dia_diem",
            "loai_hinh",
            "mo_ta",
            "yeu_cau",
            "quyen_loi_cong_viec",
            "trinh_do_hoc_van_yeu_cau",
            "thoi_gian_lam_viec",
            "thoi_han_nop_cv",
        };

        public static bool capNhatBaiDang(thong_tin_truong_du_lieu_cap_nhat_bai_dang req)
        {
            if (req == null || string.IsNullOrEmpty(req.truong))
                return false;

            if (!allowed_field_viec_lam.Contains(req.truong))
                return false;

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var ts = conn.BeginTransaction();

            try
            {
                string cap_nhat_viec_lam;

                if (req.truong == "muc_luong")
                {
                    cap_nhat_viec_lam = @"
                UPDATE viec_lam 
                SET muc_luong = @gia_tri,
                    muc_luong_thap_nhat = @muc_luong_thap_nhat,
                    muc_luong_cao_nhat = @muc_luong_cao_nhat,
                    ngay_cap_nhat = NOW()
                WHERE ma_bai_dang = @ma_bai_dang AND ma_cong_ty = @ma_cong_ty";
                }
                else
                {
                    cap_nhat_viec_lam = @$"
                UPDATE viec_lam 
                SET {req.truong} = @gia_tri,
                    ngay_cap_nhat = NOW()
                WHERE ma_bai_dang = @ma_bai_dang AND ma_cong_ty = @ma_cong_ty";
                }

                using var cmd_viec_lam = new MySqlCommand(cap_nhat_viec_lam, conn, ts);
                cmd_viec_lam.Parameters.AddWithValue("@gia_tri", req.gia_tri ?? "");
                cmd_viec_lam.Parameters.AddWithValue("@ma_bai_dang", req.ma_bai_dang);
                cmd_viec_lam.Parameters.AddWithValue("@ma_cong_ty", req.ma_cong_ty);

                if (req.truong == "muc_luong")
                {
                    cmd_viec_lam.Parameters.AddWithValue("@muc_luong_thap_nhat", req.muc_luong_thap_nhat ?? (object)DBNull.Value);
                    cmd_viec_lam.Parameters.AddWithValue("@muc_luong_cao_nhat", req.muc_luong_cao_nhat ?? (object)DBNull.Value);
                }

                int r1 = cmd_viec_lam.ExecuteNonQuery();

                string cap_nhat_bai_dang =
                    @"UPDATE bai_dang 
              SET ngay_cap_nhat = NOW() 
              WHERE ma_bai_dang = @ma_bai_dang AND ma_nguoi_dang = @ma_cong_ty";

                using var cmd_bai_dang = new MySqlCommand(cap_nhat_bai_dang, conn, ts);
                cmd_bai_dang.Parameters.AddWithValue("@ma_bai_dang", req.ma_bai_dang);
                cmd_bai_dang.Parameters.AddWithValue("@ma_cong_ty", req.ma_cong_ty);

                int r2 = cmd_bai_dang.ExecuteNonQuery();

                ts.Commit();

                return r1 > 0 || r2 > 0;
            }
            catch
            {
                ts.Rollback();
                return false;
            }
        }



    }
    public class thong_tin_truong_du_lieu_cap_nhat_bai_dang
    {
        public int ma_cong_ty { get; set; }
        public int ma_bai_dang { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
        public decimal? muc_luong_thap_nhat { get; set; }
        public decimal? muc_luong_cao_nhat { get; set; }
    }
}