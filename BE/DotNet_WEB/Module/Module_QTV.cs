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
                var nguoi_Tim_Viec = new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.GetInt32("ma_nguoi_tim_viec"),
                    ho_ten = reader.GetString("ho_ten"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    email = reader.GetString("email"),
                    dien_thoai = reader.GetString("dien_thoai"),
                    mat_khau = reader.GetString("mat_khau"),
                    ngay_sinh = reader.GetDateTime("ngay_sinh"),
                    gioi_tinh = (GioiTinh)Enum.Parse(typeof(GioiTinh), reader.GetString("gioi_tinh")),
                    trinh_do_hoc_van = (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van")),
                    dia_chi = reader.GetString("dia_chi"),
                    anh_dai_dien = reader.GetString("anh_dai_dien"),
                    quoc_tich = reader.GetString("quoc_tich"),
                    mo_ta = reader.GetString("mo_ta"),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };
                danhSachNguoiTimViec.Add(nguoi_Tim_Viec);
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
                    ma_thanh_toan = reader.GetInt32("ma_thanh_toan"),
                    ma_don_hang = reader.GetInt32("ma_don_hang"),
                    so_tien = reader.GetDecimal("so_tien"),
                    response_code = reader.GetString("response_code"),
                    transaction_no = reader.GetString("transaction_no"),
                    bank_code = reader.GetString("bank_code"),
                    ngay_thanh_toan = reader.GetDateTime("ngay_thanh_toan"),
                    trang_thai_thanh_toan = (TrangThaiThanhToan)Enum.Parse(typeof(TrangThaiThanhToan), reader.GetString("trang_thai_thanh_toan")),
                    ngay_tao = reader.GetDateTime("ngay_tao")
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
            cmd.Parameters.AddWithValue("@mo_Ta", dich_Vu.mo_ta);
            cmd.Parameters.AddWithValue("@gia", dich_Vu.gia);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool xoaCongTy(int ma_Cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string xoa_tb_nd = "delete from nguoi_dung where ma_cong_ty = @ma_cong_ty";
                using (var cmd = new MySqlCommand(xoa_tb_nd, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", ma_Cong_Ty);
                    cmd.ExecuteNonQuery();
                }

                string xoa_tb_ct = "delete from cong_ty where ma_cong_ty = @ma_cong_ty";
                using (var cmd = new MySqlCommand(xoa_tb_ct, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_cong_ty", ma_Cong_Ty);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }

        public static bool xoaNguoiTimViec(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string xoa_tb_nd = "delete from nguoi_dung where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using (var cmd = new MySqlCommand(xoa_tb_nd, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_Nguoi_Tim_Viec);
                    cmd.ExecuteNonQuery();
                }

                string xoa_tb_ct = "delete from nguoi_tim_viec where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using (var cmd = new MySqlCommand(xoa_tb_ct, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_Nguoi_Tim_Viec);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }
    }
}



//class rieng
public class thong_ke_nguoi_dung
{
    public int so_luong_nguoi_dung { get; set; }
    public int thang { get; set; }
}