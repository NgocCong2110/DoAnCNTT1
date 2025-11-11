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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_thong_ke_quan_tri
{
    public class chuc_nang_thong_ke_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
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
                    so_luong_nguoi_dung = reader.IsDBNull(reader.GetOrdinal("so_luong_nguoi_dung")) ? 0 : reader.GetInt32("so_luong_nguoi_dung"),
                    thang = reader.IsDBNull(reader.GetOrdinal("thang")) ? 0 : reader.GetInt32("thang"),
                };
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }

        public static List<thong_ke_phan_loai> laySoLuongCongTyVaNguoiTimViec()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select loai_nguoi_dung , count(*) as so_luong from nguoi_dung group by(loai_nguoi_dung)";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danhSachNguoiDung = new List<thong_ke_phan_loai>();
            while (reader.Read())
            {
                var nguoiDung = new thong_ke_phan_loai
                {
                    loai_nguoi_dung = reader.IsDBNull(reader.GetOrdinal("loai_nguoi_dung")) ? null : reader.GetString("loai_nguoi_dung"),
                    so_luong = reader.IsDBNull(reader.GetOrdinal("so_luong")) ? 0 : reader.GetInt32("so_luong"),
                };
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }

        public static List<dang_ky_moi> layDanhSachDangKyMoi()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select ma_cong_ty, email, loai_nguoi_dung, ngay_tao from nguoi_dung order by ngay_tao desc limit 10";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<dang_ky_moi>();
            while (reader.Read())
            {
                var nguoiDung = new dang_ky_moi
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    loai_nguoi_dung = reader.IsDBNull(reader.GetOrdinal("loai_nguoi_dung")) ? null : reader.GetString("loai_nguoi_dung"),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(nguoiDung);
            }
            return danh_sach;
        }

        public static List<thong_ke_tin_tuyen_dung> laySoLuongTinTuyenDung()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select count(*) as so_luong, month(ngay_tao) as thang from bai_dang group by thang";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thong_ke_tin_tuyen_dung>();
            while (reader.Read())
            {
                var nguoiDung = new thong_ke_tin_tuyen_dung
                {
                    so_luong = reader.IsDBNull(reader.GetOrdinal("so_luong")) ? 0 : reader.GetInt32("so_luong"),
                    thang = reader.IsDBNull(reader.GetOrdinal("thang")) ? 0 : reader.GetInt32("thang"),
                };
                danh_sach.Add(nguoiDung);
            }
            return danh_sach;
        }
        
        public static List<tin_tuyen_dung_moi> layDanhSachTinTuyenDungMoi()
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = "select vl.ma_bai_dang, vl.tieu_de, ct.ten_cong_ty, vl.muc_luong, vl.ngay_tao from viec_lam vl join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty order by vl.ngay_tao desc limit 10";
            using var cmd = new MySqlCommand(layDanhSach, conn);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<tin_tuyen_dung_moi>();
            while (reader.Read())
            {
                var nguoiDung = new tin_tuyen_dung_moi
                {
                    viec_Lam = new viec_lam
                    {
                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),
                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                    },
                    cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                    }
                };
                danh_sach.Add(nguoiDung);
            }
            return danh_sach;
        }
    }

    public class thong_ke_nguoi_dung
    {
        public int so_luong_nguoi_dung { get; set; }
        public int thang { get; set; }
    }

    public class thong_ke_phan_loai
    {
        public string? loai_nguoi_dung { get; set; }
        public int so_luong { get; set; }
    }

    public class dang_ky_moi
    {
        public int? ma_cong_ty { get; set; }
        public string? email { get; set; }
        public string? loai_nguoi_dung { get; set; }
        public DateTime ngay_tao { get; set; }
    }

    public class thong_ke_tin_tuyen_dung
    {
        public int so_luong { get; set; }
        public int thang { get; set; }
    }
    
    public class tin_tuyen_dung_moi
    {
        public viec_lam? viec_Lam { get; set; }
        public cong_ty? cong_Ty { get; set; }
    }
}