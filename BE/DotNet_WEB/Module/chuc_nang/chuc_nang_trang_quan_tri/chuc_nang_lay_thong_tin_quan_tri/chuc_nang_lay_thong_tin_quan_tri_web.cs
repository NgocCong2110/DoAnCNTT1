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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_lay_thong_tin_quan_tri
{
    public class chuc_nang_lay_thong_tin_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<cong_ty> layDanhSachCongTy()
        {
            var danhSachCongTy = new List<cong_ty>();

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string layDanhSach = "SELECT * FROM cong_ty";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var congTy = new cong_ty
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    ten_dn_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_dn_cong_ty")) ? null : reader.GetString("ten_dn_cong_ty"),

                    ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),

                    nguoi_dai_dien = reader.IsDBNull(reader.GetOrdinal("nguoi_dai_dien")) ? null : reader.GetString("nguoi_dai_dien"),

                    ma_so_thue = reader.IsDBNull(reader.GetOrdinal("ma_so_thue")) ? null : reader.GetString("ma_so_thue"),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? null : reader.GetString("dien_thoai"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),

                    website = reader.IsDBNull(reader.GetOrdinal("website")) ? null : reader.GetString("website"),

                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                    ? LoaiHinhCongTy.None
                    : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),

                    quy_mo = reader.IsDBNull(reader.GetOrdinal("quy_mo")) ? null : reader.GetString("quy_mo"),

                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap"))
                    ? null
                    : (int?)reader.GetInt16("nam_thanh_lap"),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                    ? TrangThaiCongTy.None
                    : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };

                danhSachCongTy.Add(congTy);
            }

            return danhSachCongTy;
        }
        public static List<nguoi_tim_viec> layDanhSachNguoiTimViec()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select * from nguoi_tim_viec";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danhSachNguoiTimViec = new List<nguoi_tim_viec>();
            while (reader.Read())
            {
                var nguoi_Tim_Viec = new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten"),

                    ten_dang_nhap = reader.IsDBNull(reader.GetOrdinal("ten_dang_nhap")) ? null : reader.GetString("ten_dang_nhap"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? null : reader.GetString("dien_thoai"),

                    mat_khau = reader.IsDBNull(reader.GetOrdinal("mat_khau")) ? null : reader.GetString("mat_khau"),

                    ngay_sinh = reader.IsDBNull(reader.GetOrdinal("ngay_sinh")) ? DateTime.MinValue : reader.GetDateTime("ngay_sinh"),

                    gioi_tinh = reader.IsDBNull(reader.GetOrdinal("gioi_tinh")) ? GioiTinh.None : (GioiTinh)Enum.Parse(typeof(GioiTinh), reader.GetString("gioi_tinh")),

                    trinh_do_hoc_van = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van")) ? TrinhDoHocVan.None : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van")),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi"),

                    anh_dai_dien = reader.IsDBNull(reader.GetOrdinal("anh_dai_dien")) ? null : reader.GetString("anh_dai_dien"),

                    quoc_tich = reader.IsDBNull(reader.GetOrdinal("quoc_tich")) ? null : reader.GetString("quoc_tich"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
                danhSachNguoiTimViec.Add(nguoi_Tim_Viec);
            }
            return danhSachNguoiTimViec;
        }
    }
}