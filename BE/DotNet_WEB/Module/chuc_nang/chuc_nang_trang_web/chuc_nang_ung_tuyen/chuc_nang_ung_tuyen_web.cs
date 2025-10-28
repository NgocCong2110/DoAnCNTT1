using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_ung_tuyen
{
    public class chuc_nang_ung_tuyen_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool kiemTraUngTuyen(ung_tuyen ung_Tuyen)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from ung_tuyen where ma_viec = @ma_Viec and ma_cong_ty = @ma_Cong_Ty and ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ung_Tuyen.ma_viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ung_Tuyen.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ung_Tuyen.ma_nguoi_tim_viec);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        public static bool ungTuyenCongViec(int ma_Viec, int ma_Cong_Ty, int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string ung_tuyen = "INSERT INTO ung_tuyen (ma_viec, ma_cong_ty, ma_nguoi_tim_viec, trang_thai) VALUES (@ma_Viec, @ma_Cong_Ty, @ma_Nguoi_Tim_Viec, @trang_Thai)";
            using var cmd = new MySqlCommand(ung_tuyen, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ma_Viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_Nguoi_Tim_Viec);
            cmd.Parameters.AddWithValue("@trang_Thai", "dang_Cho");
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }
    }
}