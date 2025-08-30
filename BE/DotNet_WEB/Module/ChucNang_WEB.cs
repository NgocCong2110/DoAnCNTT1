using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module
{
    public class ChucNang_WEB
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool themNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(nguoi_Tim_Viec.mat_khau);
            nguoi_Tim_Viec.mat_khau = hashCode.ToHashCode().ToString();

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                string them_NguoiTV = @"INSERT INTO nguoi_tim_viec (ten_dang_nhap, email, mat_khau) 
            VALUES (@ten_dang_nhap, @email, @mat_khau);
            SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(them_NguoiTV, conn, transaction);
                cmd.Parameters.AddWithValue("@ten_dang_nhap", nguoi_Tim_Viec.ten_dang_nhap);
                cmd.Parameters.AddWithValue("@email", nguoi_Tim_Viec.email);
                cmd.Parameters.AddWithValue("@mat_khau", nguoi_Tim_Viec.mat_khau);
                //lay last_insert_id
                long maNguoi_TimViec = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung (loai_nguoi_dung, ma_nguoi_tim_viec, ten_dang_nhap, mat_khau, email) 
            VALUES ('nguoi_Tim_Viec', @ma_nguoi_tim_viec, @ten_dang_nhap, @mat_khau, @email);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_nguoi_tim_viec", maNguoi_TimViec);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", nguoi_Tim_Viec.ten_dang_nhap);
                cmd2.Parameters.AddWithValue("@mat_khau", nguoi_Tim_Viec.mat_khau);
                cmd2.Parameters.AddWithValue("@email", nguoi_Tim_Viec.email);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public static bool themCongTy(cong_ty cong_Ty)
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(cong_Ty.mat_khau_dn_cong_ty);
            cong_Ty.mat_khau_dn_cong_ty = hashCode.ToHashCode().ToString();

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                string them_CongTy = @"
            INSERT INTO cong_ty (ten_dn_cong_ty, email, mat_khau_dn_cong_ty) 
            VALUES (@ten_dn_cong_ty, @email, @mat_khau_dn_cong_ty);
            SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(them_CongTy, conn, transaction);
                cmd.Parameters.AddWithValue("@ten_dn_cong_ty", cong_Ty.ten_dn_cong_ty);
                cmd.Parameters.AddWithValue("@email", cong_Ty.email);
                cmd.Parameters.AddWithValue("@mat_khau_dn_cong_ty", cong_Ty.mat_khau_dn_cong_ty);

                long ma_CongTy = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung (loai_nguoi_dung, ma_cong_ty, ten_dang_nhap, mat_khau, email) 
            VALUES ('cong_Ty', @ma_cong_ty, @ten_dang_nhap, @mat_khau, @email);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_cong_ty", ma_CongTy);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", cong_Ty.ten_dn_cong_ty);
                cmd2.Parameters.AddWithValue("@mat_khau", cong_Ty.mat_khau_dn_cong_ty);
                cmd2.Parameters.AddWithValue("@email", cong_Ty.email);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
        public static List<nguoi_dung> layThongTinNguoiDung(string email)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layThongTin = "select * from nguoi_dung where email = @email";
            using var cmd = new MySqlCommand(layThongTin, conn);
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            var danhSachNguoiDung = new List<nguoi_dung>();
            while (reader.Read())
            {
                var nguoiDung = new nguoi_dung
                {
                    loai_nguoi_dung = Enum.Parse<LoaiNguoiDung>(reader.GetString("loai_nguoi_dung")),
                    email = reader.GetString("email"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                };
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }
        public static List<quan_tri> layThongTinQuanTri(string email)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layThongTin = "select * from quan_tri where email = @email";
            using var cmd = new MySqlCommand(layThongTin, conn);
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            var danhSachQuanTri = new List<quan_tri>();
            while (reader.Read())
            {
                var quan_tri = new quan_tri
                {
                    vai_tro = Enum.Parse<VaiTro>(reader.GetString("vai_tro")),
                    email = reader.GetString("email"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                };
                danhSachQuanTri.Add(quan_tri);
            }
            return danhSachQuanTri;
        }
    }
}