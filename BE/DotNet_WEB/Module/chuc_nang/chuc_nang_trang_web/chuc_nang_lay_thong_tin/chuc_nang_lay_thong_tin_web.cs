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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_lay_thong_tin
{
    public class chuc_nang_lay_thong_tin_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<nguoi_dung> thongTinNguoiDungBangEmail(string email)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string query = "SELECT * FROM nguoi_dung WHERE email = @email";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@email", email);

            var danhSachNguoiDung = new List<nguoi_dung>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var nguoiDung = new nguoi_dung
                {
                    ma_nguoi_dung = reader.GetInt32("ma_nguoi_dung"),

                    loai_nguoi_dung = reader.IsDBNull(reader.GetOrdinal("loai_nguoi_dung")) ? LoaiNguoiDung.None : (LoaiNguoiDung)Enum.Parse(typeof(LoaiNguoiDung), reader.GetString("loai_nguoi_dung")),

                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? null : reader.GetInt32("ma_nguoi_tim_viec"),

                    ten_dang_nhap = reader.IsDBNull(reader.GetOrdinal("ten_dang_nhap")) ? string.Empty : reader.GetString("ten_dang_nhap")
                };

                if (nguoiDung.loai_nguoi_dung == LoaiNguoiDung.cong_Ty && nguoiDung.ma_cong_ty.HasValue)
                {
                    nguoiDung.cong_ty = layChiTietCongTy(nguoiDung.ma_cong_ty.Value);
                }
                else if (nguoiDung.loai_nguoi_dung == LoaiNguoiDung.nguoi_Tim_Viec && nguoiDung.ma_nguoi_tim_viec.HasValue)
                {
                    nguoiDung.nguoi_tim_viec = layChiTietNguoiTimViec(nguoiDung.ma_nguoi_tim_viec.Value);
                }
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }

        private static cong_ty? layChiTietCongTy(int maCongTy)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string query = "SELECT * FROM cong_ty WHERE ma_cong_ty = @ma";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@ma", maCongTy);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new cong_ty
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    ten_dn_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_dn_cong_ty")) ? string.Empty : reader.GetString("ten_dn_cong_ty"),

                    ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? string.Empty : reader.GetString("ten_cong_ty"),

                    nguoi_dai_dien = reader.IsDBNull(reader.GetOrdinal("nguoi_dai_dien")) ? string.Empty : reader.GetString("nguoi_dai_dien"),

                    ma_so_thue = reader.IsDBNull(reader.GetOrdinal("ma_so_thue")) ? string.Empty : reader.GetString("ma_so_thue"),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? string.Empty : reader.GetString("dia_chi"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? string.Empty : reader.GetString("dien_thoai"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),

                    website = reader.IsDBNull(reader.GetOrdinal("website")) ? string.Empty : reader.GetString("website"),

                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                    ? LoaiHinhCongTy.None
                    : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),

                    quy_mo = reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap"))
                    ? null
                    : (int?)reader.GetInt16("nam_thanh_lap"),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                    ? TrangThaiCongTy.None
                    : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }

        private static nguoi_tim_viec? layChiTietNguoiTimViec(int maNguoiTimViec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string query = "SELECT * FROM nguoi_tim_viec WHERE ma_nguoi_tim_viec = @ma";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@ma", maNguoiTimViec);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? string.Empty : reader.GetString("ho_ten"),

                    ten_dang_nhap = reader.IsDBNull(reader.GetOrdinal("ten_dang_nhap")) ? string.Empty : reader.GetString("ten_dang_nhap"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? string.Empty : reader.GetString("dien_thoai"),

                    mat_khau = reader.IsDBNull(reader.GetOrdinal("mat_khau")) ? string.Empty : reader.GetString("mat_khau"),

                    ngay_sinh = reader.IsDBNull(reader.GetOrdinal("ngay_sinh")) ? DateTime.MinValue : reader.GetDateTime("ngay_sinh"),

                    gioi_tinh = reader.IsDBNull(reader.GetOrdinal("gioi_tinh")) ? GioiTinh.None : (GioiTinh)Enum.Parse(typeof(GioiTinh), reader.GetString("gioi_tinh")),

                    trinh_do_hoc_van = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van")) ? TrinhDoHocVan.None : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van")),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? string.Empty : reader.GetString("dia_chi"),

                    anh_dai_dien = reader.IsDBNull(reader.GetOrdinal("anh_dai_dien")) ? string.Empty : reader.GetString("anh_dai_dien"),

                    quoc_tich = reader.IsDBNull(reader.GetOrdinal("quoc_tich")) ? string.Empty : reader.GetString("quoc_tich"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? string.Empty : reader.GetString("mo_ta"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }

        public static string layHoTenNguoiDung(int ma_nguoi_dung)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string lay_ho_ten_ntv = @"SELECT ntv.ho_ten FROM nguoi_tim_viec ntv 
        JOIN nguoi_dung nd ON ntv.ma_nguoi_tim_viec = nd.ma_nguoi_dung WHERE nd.ma_nguoi_dung = @ma_nguoi_dung";
            using (var cmd = new MySqlCommand(lay_ho_ten_ntv, coon))
            {
                cmd.Parameters.AddWithValue("@ma_nguoi_dung", ma_nguoi_dung);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetString("ho_ten");
                }
            }
            string lay_ho_ten_cong_ty = @"SELECT ct.ten_cong_ty FROM cong_ty ct 
        JOIN nguoi_dung nd ON ct.ma_cong_ty = nd.ma_nguoi_dung WHERE nd.ma_nguoi_dung = @ma_nguoi_dung";
            using (var cmd2 = new MySqlCommand(lay_ho_ten_cong_ty, coon))
            {
                cmd2.Parameters.AddWithValue("@ma_nguoi_dung", ma_nguoi_dung);
                using var reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    return reader2.GetString("ten_cong_ty");
                }
            }
            return "Không tìm thấy người dùng";
        }

        public static List<quan_tri> layThongTinQuanTri(string email_Quan_Tri)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from quan_tri where email = @email_Quan_Tri";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("email_Quan_Tri", email_Quan_Tri);
            using var reader = cmd.ExecuteReader();
            var thong_tin = new List<quan_tri>();
            if (reader.Read())
            {
                var quan_Tri = new quan_tri
                {
                    ma_quan_tri = reader.GetInt32("ma_quan_tri"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    ho_ten = reader.GetString("ho_ten"),
                    email = reader.GetString("email")
                };
                thong_tin.Add(quan_Tri);
            }
            return thong_tin;
        }
    }
}