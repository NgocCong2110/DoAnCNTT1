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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_otp
{
    public class chuc_nang_otp_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool kiemTraOTPTonTai(string email)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select ma_otp_gui_di from ma_otp where email = @email and het_han_luc > now()";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return false;
            }
            return true;
        }

        public static int taoOTPMoi(string email)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string xoa_otp = "delete from ma_otp where email = @email";
                using (var cmd = new MySqlCommand(xoa_otp, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                }
                int ma_otp_rad = RanDomOTP();
                string tao_otp_moi = "insert into ma_otp (email, ma_otp_gui_di, het_han_luc, da_su_dung, so_lan_thu) values(@email, @otp_rad, @thoi_gian_het_han, 0, 0)";
                using (var cmd = new MySqlCommand(tao_otp_moi, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@otp_rad", ma_otp_rad);
                    cmd.Parameters.AddWithValue("@thoi_gian_het_han", DateTime.Now.AddMinutes(5));
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return ma_otp_rad;
            }
            catch
            {
                trans.Rollback();
                return 0;
            }
        }

        public static int RanDomOTP()
        {
            Random random = new Random();
            int num = random.Next(100000, 999999);
            return num;
        }
    }
}