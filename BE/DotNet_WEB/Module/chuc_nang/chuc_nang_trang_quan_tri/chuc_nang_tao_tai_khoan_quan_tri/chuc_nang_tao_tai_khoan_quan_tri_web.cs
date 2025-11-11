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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_tao_tai_khoan_quan_tri
{
    public class chuc_nang_tao_tai_khoan_quan_tri_web
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
            string them_Quan_Tri = "insert into quan_tri(email, mat_khau) values (@email, @mat_khau)";
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
    }
}