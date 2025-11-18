using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using System.Net.Mail;
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
            if (string.IsNullOrEmpty(cong_Ty.mat_khau_dn_cong_ty) || string.IsNullOrEmpty(cong_Ty.email))
                return false;

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            try
            {
                string matKhauMaHoa = XacThuc_ND.maHoaMatKhau(cong_Ty.mat_khau_dn_cong_ty);

                string them_CongTy = @"
            INSERT INTO cong_ty 
            (ten_cong_ty, ten_dn_cong_ty, mat_khau_dn_cong_ty, nguoi_dai_dien, ma_so_thue, nam_thanh_lap,
             dia_chi, dien_thoai, email, loai_hinh_cong_ty, trang_thai, ngay_tao)
            VALUES
            (@ten_cong_ty, @ten_dn_cong_ty, @mat_khau_dn_cong_ty, @nguoi_dai_dien, @ma_so_thue, @nam_thanh_lap,
             @dia_chi, @dien_thoai, @email, @loai_hinh_cong_ty, @trang_thai, @ngay_tao);
            SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(them_CongTy, conn, transaction);
                cmd.Parameters.AddWithValue("@ten_cong_ty", cong_Ty.ten_cong_ty);
                cmd.Parameters.AddWithValue("@ten_dn_cong_ty", cong_Ty.ten_dn_cong_ty);
                cmd.Parameters.AddWithValue("@mat_khau_dn_cong_ty", matKhauMaHoa);
                cmd.Parameters.AddWithValue("@nguoi_dai_dien", cong_Ty.nguoi_dai_dien);
                cmd.Parameters.AddWithValue("@ma_so_thue", cong_Ty.ma_so_thue);
                cmd.Parameters.AddWithValue("@nam_thanh_lap", cong_Ty.nam_thanh_lap);
                cmd.Parameters.AddWithValue("@dia_chi", cong_Ty.dia_chi);
                cmd.Parameters.AddWithValue("@dien_thoai", cong_Ty.dien_thoai);
                cmd.Parameters.AddWithValue("@email", cong_Ty.email);
                cmd.Parameters.AddWithValue("@loai_hinh_cong_ty", cong_Ty.loai_hinh_cong_ty.ToString());
                cmd.Parameters.AddWithValue("@trang_thai", cong_Ty.trang_thai.ToString());
                cmd.Parameters.AddWithValue("@ngay_tao", cong_Ty.ngay_tao);

                long ma_CongTy = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung 
            (loai_nguoi_dung, ma_cong_ty, ten_dang_nhap, mat_khau, email, ngay_tao)
            VALUES
            ('cong_Ty', @ma_cong_ty, @ten_dang_nhap, @mat_khau, @email, @ngay_tao);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_cong_ty", ma_CongTy);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", cong_Ty.ten_dn_cong_ty);
                cmd2.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
                cmd2.Parameters.AddWithValue("@email", cong_Ty.email);
                cmd2.Parameters.AddWithValue("@ngay_tao", cong_Ty.ngay_tao);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
                _ = thongBaoTaoTaiKhoanCongTy(cong_Ty.email);
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
        public static async Task<bool> thongBaoTaoTaiKhoanCongTy(string email_yeu_cau)
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
                    Subject = "Thông báo về tài khoản công ty",
                    Body = $"Xin chào,\n\n Tài khoản công ty của bạn đã được tạo thành công\n\nTruy cập website để sử dụng các tính năng của JobFinder.\n\nCảm ơn bạn đã sử dụng JobFinder!",
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
    }
}