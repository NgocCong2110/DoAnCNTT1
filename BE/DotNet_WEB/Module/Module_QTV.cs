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


namespace DotNet_WEB.Module
{
    public class Module_QTV
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";


        public static bool themQuanTriVien(quan_tri quan_Tri)
        {
            if (quan_Tri.mat_khau == null || quan_Tri.email == null)
            {
                return false;
            }
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string them_Quan_Tri = "insert into quan_tri(email_quan_tri, mat_khau_quan_tri) values (@email, @mat_khau)";
            string matKhauMaHoa = XacThuc_ND.maHoaMatKhau(quan_Tri.mat_khau);
            using var cmd = new MySqlCommand(them_Quan_Tri, conn);
            cmd.Parameters.AddWithValue("@email", quan_Tri.email);
            cmd.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
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
                var nguoiTimViec = new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.GetInt32("ma_nguoi_tim_viec"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    email = reader.GetString("email"),
                };
                danhSachNguoiTimViec.Add(nguoiTimViec);
            }
            return danhSachNguoiTimViec;
        }

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
                    ma_cong_ty = reader.GetInt32("ma_cong_ty"),
                    ten_dn_cong_ty = reader.GetString("ten_dn_cong_ty"),
                    ten_cong_ty = reader.GetString("ten_cong_ty"),
                    nguoi_dai_dien = reader.GetString("nguoi_dai_dien"),
                    ma_so_thue = reader.GetString("ma_so_thue"),
                    dia_chi = reader.GetString("dia_chi"),
                    dien_thoai = reader.GetString("dien_thoai"),
                    email = reader.GetString("email"),
                    website = reader.GetString("website"),
                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),
                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                    ? LoaiHinhCongTy.None
                    : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),
                    quy_mo = reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap")) 
                    ? null 
                    : (int?)reader.GetInt16("nam_thanh_lap"),
                    anh_bia = reader.IsDBNull(reader.GetOrdinal("anh_bia")) ? null : reader.GetString("anh_bia"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                    ? TrangThaiCongTy.None
                    : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };

                danhSachCongTy.Add(congTy);
            }

            return danhSachCongTy;
        }

        public static List<thong_ke_nguoi_dung> laySoLuongNguoiDung()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select count(*) as so_luong_nguoi_dung, month(ngay_tao) as thang from nguoi_dung a group by month(ngay_tao) order by thang";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danhSachNguoiDung = new List<thong_ke_nguoi_dung>();
            while (reader.Read())
            {
                var nguoiDung = new thong_ke_nguoi_dung
                {
                    so_luong_nguoi_dung = reader.GetInt32("so_luong_nguoi_dung"),
                    thang = reader.GetInt32("thang"),
                };
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }

        public static List<bai_dang_vi_pham> layDanhSachViPham()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select * from bai_dang_vi_pham";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danhSachViPham = new List<bai_dang_vi_pham>();
            while (reader.Read())
            {
                var viPham = new bai_dang_vi_pham
                {
                    ma_bai_vi_pham = reader.GetInt32("ma_bai_vi_pham"),
                    ten_nguoi_dang = reader.GetString("ten_nguoi_dang"),
                    tieu_de = reader.GetString("tieu_de"),
                    noi_dung = reader.GetString("noi_dung"),
                    ma_nguoi_bao_cao = reader.GetInt32("ma_nguoi_bao_cao"),
                    noi_dung_bao_cao = reader.GetString("noi_dung_bao_cao"),
                    ngay_bao_cao = reader.GetDateTime("ngay_bao_cao")
                };
                danhSachViPham.Add(viPham);
            }
            return danhSachViPham;
        }
    }
}



//class rieng
public class thong_ke_nguoi_dung
{
    public int so_luong_nguoi_dung { get; set; }
    public int thang { get; set; }
}