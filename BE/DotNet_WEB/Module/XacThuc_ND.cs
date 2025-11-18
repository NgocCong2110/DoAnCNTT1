using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using DotNet_WEB.Class;
using System.Net.Http;
using System.Text.Json;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;


namespace DotNet_WEB.Module
{
    public class XacThuc_ND
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static async Task<bool> guiMaOTP(int otp, string email_yeu_cau)
        {
            try
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 |
                    SecurityProtocolType.Tls13;

                using var smtp = new SmtpClient("smtp-relay.brevo.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("9bd2ea001@smtp-brevo.com", "rWNkpnUTRz5xyZQ9"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 15000
                };

                smtp.SendCompleted += (s, e) =>
                {
                    Console.WriteLine("SendCompleted: " + e.Error?.Message);
                };

                using var mail = new MailMessage
                {
                    From = new MailAddress("cong20365@gmail.com", "JobFinder"),
                    Subject = "Mã OTP kích hoạt tài khoản",
                    Body = $"Xin chào,\n\nMã OTP của bạn là: {otp}. Mã này tồn tại trong 5 phút.\n\nCảm ơn bạn đã sử dụng JobFinder!",
                    IsBodyHtml = false
                };

                mail.To.Add(email_yeu_cau);

                await smtp.SendMailAsync(mail);

                Console.WriteLine("Gửi OTP thành công!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("LỖI EMAIL: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public static bool xacNhanOTP(ma_otp ma_Otp)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select ma_otp_gui_di from ma_otp where ma_otp_gui_di = @ma_otp and email = @email";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_otp", ma_Otp.ma_otp_gui_di);
            cmd.Parameters.AddWithValue("@email", ma_Otp.email);
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        public static async Task<bool> kiemTraMaSoThue(string maSo_Thue)
        {
            string url = $"https://api.vietqr.io/v2/business/{maSo_Thue}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    var root = doc.RootElement;
                    string? code = root.GetProperty("code").GetString();
                    if (code == "00")
                    {
                        var data = root.GetProperty("data");
                        string? tenCongTy = data.GetProperty("name").GetString();
                        string? diaChi = data.GetProperty("address").GetString();
                        Console.WriteLine($"Tên công ty: {tenCongTy}, Địa chỉ: {diaChi}");
                        Console.WriteLine($"Mã số thuế {maSo_Thue} hợp lệ.");
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool xacThucNguoiDung(string email, string matKhau)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string sql = "SELECT COUNT(*) FROM nguoi_dung WHERE email = @Email AND mat_khau = @MatKhau";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@MatKhau", matKhau);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool xacThucQuanTriVien(string email, string mat_khau_maHoa)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string sql = "SELECT COUNT(*) FROM quan_tri WHERE email = @Email AND mat_khau = @MatKhau";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@MatKhau", mat_khau_maHoa);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public static string maHoaMatKhau(string mat_khau)
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