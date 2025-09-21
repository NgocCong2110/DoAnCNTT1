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
            public static async Task<bool> xacThucGmail(MailAddress email_NguoiDung)
            {
                try
                {
                    var smtp = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("cong20365@gmail.com", "toffiwhnrerezjuu"),
                        EnableSsl = true
                    };

                    //url có thể thay đổi 
                    string token = "31bGWzmNtgNjF6FNOFY8wbFkmyz_3odjeY5iYDDNFZDsC7Raq";
                    string ngrokUrl = "https://cd77a077d2db.ngrok-free.app";
                    string link_XacThuc = $"{ngrokUrl}/api/API_WEB/verify?token={token}";

                    var mail = new MailMessage
                    {
                        From = new MailAddress("cong20365@gmail.com", "Cong"),
                        Subject = "Xác thực tài khoản",
                        Body = $"<a href='{link_XacThuc}'>Xác thực tài khoản</a>",
                        IsBodyHtml = true
                    };
                    mail.To.Add(email_NguoiDung);

                    await smtp.SendMailAsync(mail);
                }
                catch
                {
                    
                }

                return true; 
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