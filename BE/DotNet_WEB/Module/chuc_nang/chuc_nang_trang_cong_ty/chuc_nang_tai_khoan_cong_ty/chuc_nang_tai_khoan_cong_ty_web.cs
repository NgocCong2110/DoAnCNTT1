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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty
{
    public class chuc_nang_tai_khoan_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        private static readonly string[] allowedFields = new string[]
        {
            "ten_cong_ty",
            "email",
            "dien_thoai",
            "website",
            "nguoi_dai_dien",
            "ma_so_thue",
            "nam_thanh_lap",
            "dia_chi",
            "loai_hinh_cong_ty",
            "quy_mo",
            "trang_thai",
            "mo_ta"
        };
        public static bool capNhatThongTinCongTy(thong_tin_truong_du_lieu_cap_nhat req)
        {
            if (req == null || string.IsNullOrEmpty(req.truong))
                return false;

            if (!allowedFields.Contains(req.truong))
                return false;

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string sql = $"UPDATE cong_ty SET {req.truong} = @gia_tri, ngay_cap_nhat = NOW() WHERE ma_cong_ty = @ma_cong_ty";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@gia_tri", req.gia_tri);
            cmd.Parameters.AddWithValue("@ma_cong_ty", req.ma_cong_ty);

            return cmd.ExecuteNonQuery() > 0;
        }
        public static List<cong_ty> layThongTinCongTy(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "SELECT * FROM cong_ty WHERE ma_cong_ty = @ma_cong_ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
            var danh_sach = new List<cong_ty>();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var ct = new cong_ty
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                    ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                    ten_dn_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_dn_cong_ty")) ? null : reader.GetString("ten_dn_cong_ty"),
                    nguoi_dai_dien = reader.IsDBNull(reader.GetOrdinal("nguoi_dai_dien")) ? null : reader.GetString("nguoi_dai_dien"),
                    ma_so_thue = reader.IsDBNull(reader.GetOrdinal("ma_so_thue")) ? null : reader.GetString("ma_so_thue"),
                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi"),
                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? null : reader.GetString("dien_thoai"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    website = reader.IsDBNull(reader.GetOrdinal("website")) ? null : reader.GetString("website"),
                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),
                    anh_bia = reader.IsDBNull(reader.GetOrdinal("anh_bia")) ? null : reader.GetString("anh_bia"),
                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                                        ? LoaiHinhCongTy.congty_CoPhan
                                        : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),
                    quy_mo = reader.IsDBNull(reader.GetOrdinal("quy_mo")) ? null : reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap")) ? null : reader.GetInt32("nam_thanh_lap"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                                ? TrangThaiCongTy.hoat_Dong
                                : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.Now : reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.Now : reader.GetDateTime("ngay_cap_nhat")
                };
                danh_sach.Add(ct);
            }

            reader.Close();

            string sql_phuc_loi = "SELECT * FROM phuc_loi_cong_ty WHERE ma_cong_ty = @ma_cong_ty";
            using (var cmd_phuc_loi = new MySqlCommand(sql_phuc_loi, coon))
            {
                cmd_phuc_loi.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                using var reader_phuc_loi = cmd_phuc_loi.ExecuteReader();   
                var danh_sach_phuc_loi = new List<phuc_loi_cong_ty>();

                while (reader_phuc_loi.Read())
                {
                    danh_sach_phuc_loi.Add(new phuc_loi_cong_ty
                    {
                        ma_phuc_loi_cty = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ma_phuc_loi_cty")) ? 0 : reader_phuc_loi.GetInt32("ma_phuc_loi_cty"),
                        ma_cong_ty = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ma_cong_ty")) ? 0 : reader_phuc_loi.GetInt32("ma_cong_ty"),
                        ten_phuc_loi = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ten_phuc_loi")) ? null : reader_phuc_loi.GetString("ten_phuc_loi"),
                        mo_ta = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("mo_ta")) ? null : reader_phuc_loi.GetString("mo_ta")
                    });
                }
                reader_phuc_loi.Close();

                if (danh_sach.Count > 0)
                    danh_sach[0].phuc_Loi_Cong_Ty = danh_sach_phuc_loi;
            }

            string sql_mang_xa_hoi = "SELECT * FROM lien_ket_mang_xa_hoi WHERE ma_cong_ty = @ma_cong_ty";
            using (var cmd_mang_xa_hoi = new MySqlCommand(sql_mang_xa_hoi, coon))
            {
                cmd_mang_xa_hoi.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                using var reader_mang_xa_hoi = cmd_mang_xa_hoi.ExecuteReader();
                var danh_sach_lien_ket = new List<lien_ket_mang_xa_hoi>();

                while (reader_mang_xa_hoi.Read())
                {
                    danh_sach_lien_ket.Add(new lien_ket_mang_xa_hoi
                    {
                        ma_lien_ket = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ma_lien_ket")) ? 0 : reader_mang_xa_hoi.GetInt32("ma_lien_ket"),
                        ma_cong_ty = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ma_cong_ty")) ? 0 : reader_mang_xa_hoi.GetInt32("ma_cong_ty"),
                        ten_mang_xa_hoi = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ten_mang_xa_hoi")) ? null : reader_mang_xa_hoi.GetString("ten_mang_xa_hoi"),
                        duong_dan =  reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("duong_dan")) ? null : reader_mang_xa_hoi.GetString("duong_dan")
                    });
                }
                reader_mang_xa_hoi.Close();

                if (danh_sach.Count > 0)
                    danh_sach[0].lien_Ket_Mang_Xa_Hoi = danh_sach_lien_ket;
            }
            return danh_sach;
        }

    }
    public class thong_tin_truong_du_lieu_cap_nhat
    {
        public int ma_cong_ty { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
    }

}