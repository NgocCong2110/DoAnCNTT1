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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_doi_mat_khau
{
    public class chuc_nang_doi_mat_khau_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
         public static bool doiMatKhauMoi(string email, string mat_khau)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();

            try
            {
                string sqlLoai = "SELECT loai_nguoi_dung FROM nguoi_dung WHERE email = @email";
                LoaiNguoiDung loaiNguoiDung = LoaiNguoiDung.None;

                using (var cmd = new MySqlCommand(sqlLoai, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string? loaiStr = result.ToString();
                        if (!Enum.TryParse(loaiStr, out loaiNguoiDung))
                        {
                            throw new Exception("Loại người dùng trong DB không hợp lệ.");
                        }
                    }
                    else
                    {
                        throw new Exception("Không tìm thấy người dùng với email này.");
                    }
                }

                string? mkMaHoa = maHoaMatKhau(mat_khau);

                string doi_mk_nguoi_dung = "UPDATE nguoi_dung SET mat_khau = @mat_khau WHERE email = @email";
                using (var cmd = new MySqlCommand(doi_mk_nguoi_dung, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@mat_khau", mkMaHoa);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                }

                if (loaiNguoiDung == LoaiNguoiDung.cong_Ty)
                {
                    string doi_mk_cong_ty = "UPDATE cong_ty SET mat_khau_dn_cong_ty = @mat_khau WHERE email = @email";
                    using (var cmd = new MySqlCommand(doi_mk_cong_ty, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@mat_khau", mkMaHoa);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (loaiNguoiDung == LoaiNguoiDung.nguoi_Tim_Viec)
                {
                    string doi_mk_tim_viec = "UPDATE nguoi_tim_viec SET mat_khau = @mat_khau WHERE email = @email";
                    using (var cmd = new MySqlCommand(doi_mk_tim_viec, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@mat_khau", mkMaHoa);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine("Lỗi đổi mật khẩu: " + ex.Message);
                return false;
            }
        }

        public static bool doiMatKhauQuanTri(string email, string mat_khau)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "UPDATE quan_tri SET mat_khau = @mat_khau WHERE email = @email";
            string? mkMaHoa = maHoaMatKhau(mat_khau);
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@mat_khau", mkMaHoa);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.ExecuteNonQuery();
            return true;
        }

        public static string? maHoaMatKhau(string mat_khau)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(mat_khau);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}