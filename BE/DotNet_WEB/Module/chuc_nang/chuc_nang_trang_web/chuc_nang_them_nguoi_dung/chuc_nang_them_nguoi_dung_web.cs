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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_them_nguoi_dung
{
    public class chuc_nang_them_nguoi_dung_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool themNguoiTimViec(nguoi_tim_viec nguoi_Tim_Viec)
        {
            if (nguoi_Tim_Viec.mat_khau == null || nguoi_Tim_Viec.email == null)
            {
                return false;
            }
            string matKhauMaHoa = XacThuc_ND.maHoaMatKhau(nguoi_Tim_Viec.mat_khau);

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
                cmd.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
                //lay last_insert_id
                long maNguoi_TimViec = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung (loai_nguoi_dung, ma_nguoi_tim_viec, ten_dang_nhap, mat_khau, email) 
            VALUES ('nguoi_Tim_Viec', @ma_nguoi_tim_viec, @ten_dang_nhap, @mat_khau, @email);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_nguoi_tim_viec", maNguoi_TimViec);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", nguoi_Tim_Viec.ten_dang_nhap);
                cmd2.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
                cmd2.Parameters.AddWithValue("@email", nguoi_Tim_Viec.email);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Lỗi khi thêm người tìm việc: " + ex.Message);
                return false;
            }
        }

        public static bool themCongTy(cong_ty cong_Ty)
        {
            if (cong_Ty.mat_khau_dn_cong_ty == null || cong_Ty.email == null)
            {
                return false;
            }
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            string matKhauMaHoa = XacThuc_ND.maHoaMatKhau(cong_Ty.mat_khau_dn_cong_ty);
            try
            {
                string them_CongTy = @"
            INSERT INTO cong_ty (ten_dn_cong_ty, email, mat_khau_dn_cong_ty) 
            VALUES (@ten_dn_cong_ty, @email, @mat_khau_dn_cong_ty);
            SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(them_CongTy, conn, transaction);
                cmd.Parameters.AddWithValue("@ten_dn_cong_ty", cong_Ty.ten_dn_cong_ty);
                cmd.Parameters.AddWithValue("@email", cong_Ty.email);
                cmd.Parameters.AddWithValue("@mat_khau_dn_cong_ty", matKhauMaHoa);

                long ma_CongTy = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung (loai_nguoi_dung, ma_cong_ty, ten_dang_nhap, mat_khau, email) 
            VALUES ('cong_Ty', @ma_cong_ty, @ten_dang_nhap, @mat_khau, @email);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_cong_ty", ma_CongTy);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", cong_Ty.ten_dn_cong_ty);
                cmd2.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
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
    }
}