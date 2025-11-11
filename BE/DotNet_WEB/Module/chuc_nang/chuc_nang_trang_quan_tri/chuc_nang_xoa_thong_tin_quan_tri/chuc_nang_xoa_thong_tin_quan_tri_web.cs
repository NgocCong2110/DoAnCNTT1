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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_xoa_thong_tin_quan_tri
{
    public class chuc_nang_xoa_thong_tin_quan_tri_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool xoaCongTy(int ma_Cong_Ty)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            try
            {
                string sql = "DELETE FROM cong_ty WHERE ma_cong_ty = @ma_cong_ty";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_Cong_Ty);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa công ty: " + ex.Message);
                return false;
            }
        }

        public static bool xoaNguoiTimViec(int ma_Nguoi_Tim_Viec)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            try
            {
                string sql = "DELETE FROM nguoi_tim_viec WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_Nguoi_Tim_Viec);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa người tìm việc: " + ex.Message);
                return false;
            }
        }
    }
}