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


namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_viec_lam_cong_ty
{
    public class chuc_nang_viec_lam_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<viec_lam> layDanhSachViecLamCuaCongTy(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"SELECT vl.*, ct.logo, ct.ten_cong_ty FROM viec_lam vl join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty
                 WHERE ct.ma_cong_ty = @ma_cong_ty ";
            using var cmd = new MySqlCommand(sql, coon);
            var danh_sach = new List<viec_lam>();
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vl = new viec_lam
                {
                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    cong_Ty = new cong_ty
                    {
                        logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    },

                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                    vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),

                    kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),

                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                    muc_luong_cao_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_cao_nhat")) ? null : reader.GetDecimal("muc_luong_cao_nhat"),

                    muc_luong_thap_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_thap_nhat")) ? null : reader.GetDecimal("muc_luong_thap_nhat"),

                    quyen_loi_cong_viec = reader.IsDBNull(reader.GetOrdinal("quyen_loi_cong_viec")) ? null : reader.GetString("quyen_loi_cong_viec"),

                    trinh_do_hoc_van_yeu_cau = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van_yeu_cau")) ? TrinhDoHocVan.khong_Yeu_Cau : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van_yeu_cau")),

                    thoi_gian_lam_viec = reader.IsDBNull(reader.GetOrdinal("thoi_gian_lam_viec")) ? null : reader.GetString("thoi_gian_lam_viec"),

                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),

                    thoi_han_nop_cv = reader.IsDBNull(reader.GetOrdinal("thoi_han_nop_cv")) ? DateTime.MinValue : reader.GetDateTime("thoi_han_nop_cv"),

                    loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
                danh_sach.Add(vl);
            }
            return danh_sach;
        }
        public static List<ung_tuyen> layDanhSachViecLamNoiBatCuaCongTy(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"SELECT vl.*, COUNT(ut.ma_viec) AS so_luong_ung_vien, vl.tieu_de
                            FROM ung_tuyen ut
                            JOIN viec_lam vl ON ut.ma_viec = vl.ma_viec
                            WHERE vl.ma_cong_ty = @ma_cong_ty
                            GROUP BY vl.ma_viec
                            ORDER BY so_luong_ung_vien DESC
                        limit 5";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<ung_tuyen>();
            while (reader.Read())
            {
                var ut = new ung_tuyen
                {
                    viec_Lam = new viec_lam
                    {
                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),
                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                        loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),
                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang")
                    }
                };
                danh_sach.Add(ut);
            }
            return danh_sach;
        }
    }
}