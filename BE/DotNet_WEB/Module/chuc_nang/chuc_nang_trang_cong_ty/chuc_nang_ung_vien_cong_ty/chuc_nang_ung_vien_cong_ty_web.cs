using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using System.Net.Mail;
using System.Net;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_ung_vien_cong_ty
{
    public class chuc_nang_ung_vien_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<ung_tuyen> layDanhSachUngVien(int ma_Cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
SELECT u.ma_ung_tuyen, u.ma_viec, u.ma_nguoi_tim_viec, u.ma_cong_ty, u.ma_nguoi_nhan, u.ma_cv, u.duong_dan_file_cv_upload,
       u.ngay_ung_tuyen, u.trang_thai, u.trang_thai_duyet,
       v.ma_viec, v.ma_cong_ty, v.vi_tri, v.kinh_nghiem, v.tieu_de,
       v.mo_ta, v.yeu_cau, v.muc_luong, v.dia_diem, v.loai_hinh,
       v.ngay_tao, v.ngay_cap_nhat, v.ma_bai_dang,
       n.ho_ten, n.email, cv.ten_cv, cv.duong_dan_file_pdf
FROM ung_tuyen u
INNER JOIN viec_lam v ON u.ma_viec = v.ma_viec
INNER JOIN nguoi_tim_viec n ON u.ma_nguoi_tim_viec = n.ma_nguoi_tim_viec
LEFT JOIN cv_online_nguoi_tim_viec cv ON u.ma_cv = cv.ma_cv
WHERE u.ma_cong_ty = @ma_Cong_Ty;";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach_ung_tuyen = new List<ung_tuyen>();

            while (reader.Read())
            {
                var ntv = new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),
                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                };

                var ungTuyen = new ung_tuyen
                {
                    ma_ung_tuyen = reader.IsDBNull(reader.GetOrdinal("ma_ung_tuyen")) ? 0 : reader.GetInt32("ma_ung_tuyen"),

                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? null : reader.GetInt32("ma_cong_ty"),

                    ma_nguoi_nhan = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_nhan")) ? null : reader.GetInt32("ma_nguoi_nhan"),

                    ma_cv = reader.IsDBNull(reader.GetOrdinal("ma_cv")) ? 0 : reader.GetInt32("ma_cv"),

                    duong_dan_file_cv_upload = reader.IsDBNull(reader.GetOrdinal("duong_dan_file_cv_upload")) ? null : reader.GetString("duong_dan_file_cv_upload"),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai")) ? TrangThaiUngTuyen.None : (TrangThaiUngTuyen)Enum.Parse(typeof(TrangThaiUngTuyen), reader.GetString("trang_thai")),

                    trang_thai_duyet = reader.IsDBNull(reader.GetOrdinal("trang_thai_duyet")) ? TrangThaiDuyetUngTuyen.None : (TrangThaiDuyetUngTuyen)Enum.Parse(typeof(TrangThaiDuyetUngTuyen), reader.GetString("trang_thai_duyet")),

                    ngay_ung_tuyen = reader.IsDBNull(reader.GetOrdinal("ngay_ung_tuyen")) ? DateTime.MinValue : reader.GetDateTime("ngay_ung_tuyen"),

                    viec_Lam = new viec_lam
                    {
                        ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                        ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),

                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                        yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),

                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                        dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),

                        loai_hinh = (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),

                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                        ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat"),

                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang")
                    },

                    cv_Online_Nguoi_Tim_Viec = new cv_online_nguoi_tim_viec
                    {
                        ten_cv = reader.IsDBNull(reader.GetOrdinal("ten_cv")) ? null : reader.GetString("ten_cv"),

                        duong_dan_file_pdf = reader.IsDBNull(reader.GetOrdinal("duong_dan_file_pdf")) ? null : reader.GetString("duong_dan_file_pdf")
                    },

                    nguoi_tim_viec = ntv
                };

                danh_sach_ung_tuyen.Add(ungTuyen);
            }

            return danh_sach_ung_tuyen;
        }
        public static bool tuChoiUngVien(ung_tuyen ung_Tuyen)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string cap_nhat_trang_thai = "update ung_tuyen set trang_thai = @trang_thai, trang_thai_duyet = @trang_thai_duyet where ma_cong_ty = @ma_cong_ty and ma_nguoi_tim_viec = @ma_ntv and ma_viec = @ma_viec";
            using var cmd = new MySqlCommand(cap_nhat_trang_thai, coon);
            cmd.Parameters.AddWithValue("@ma_viec", ung_Tuyen.ma_viec);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ung_Tuyen.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_ntv", ung_Tuyen.ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@trang_thai", "tu_Choi");
            cmd.Parameters.AddWithValue("@trang_thai_duyet", "da_Duyet");
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool xoaUngVien(ung_tuyen ung_Tuyen)
        {
            try
            {
                using var coon = new MySqlConnection(chuoi_KetNoi);
                coon.Open();

                string? cv_nop_len = null;
                string sql_cv = @"
            SELECT duong_dan_file_cv_upload 
            FROM ung_tuyen 
            WHERE ma_nguoi_tim_viec = @ma_nguoi_tim_viec 
              AND ma_viec = @ma_viec 
              AND ma_cong_ty = @ma_cong_ty";

                using (var cmd1 = new MySqlCommand(sql_cv, coon))
                {
                    cmd1.Parameters.AddWithValue("@ma_cong_ty", ung_Tuyen.ma_cong_ty);
                    cmd1.Parameters.AddWithValue("@ma_nguoi_tim_viec", ung_Tuyen.ma_nguoi_tim_viec);
                    cmd1.Parameters.AddWithValue("@ma_viec", ung_Tuyen.ma_viec);

                    var result = cmd1.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        cv_nop_len = result.ToString();
                }

                string sql = @"
            DELETE FROM ung_tuyen 
            WHERE ma_cong_ty = @ma_Cong_Ty 
              AND ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec 
              AND ma_viec = @ma_Viec";

                using var cmd = new MySqlCommand(sql, coon);
                cmd.Parameters.AddWithValue("@ma_Cong_Ty", ung_Tuyen.ma_cong_ty);
                cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ung_Tuyen.ma_nguoi_tim_viec);
                cmd.Parameters.AddWithValue("@ma_Viec", ung_Tuyen.ma_viec);

                bool kq = cmd.ExecuteNonQuery() > 0;

                if (kq && !string.IsNullOrEmpty(cv_nop_len))
                {
                    var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(),
                        cv_nop_len.Replace("/", "\\"));

                    try
                    {
                        if (File.Exists(duong_dan_cu))
                            File.Delete(duong_dan_cu);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi xóa file CV: " + ex.Message);
                    }
                }

                return kq;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa ứng viên: " + ex.Message);
                return false;
            }
        }


        public static bool guiThuMoiPhongVan(thong_tin_phong_van ttpv)
        {
            string email = "";
            try
            {
                using var coon = new MySqlConnection(chuoi_KetNoi);
                coon.Open();
                using var trans = coon.BeginTransaction();
                try
                {
                    int ma_thong_bao = 0;
                    string them_thong_bao = "insert into thong_bao(tieu_de, noi_dung, loai_thong_bao, ma_cong_ty, ma_nguoi_tim_viec, ngay_tao) values(@td, @nd, @ltb, @ma_ct, @ma_ntv, @ngay_tao)";
                    using (var cmd = new MySqlCommand(them_thong_bao, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@td", "Thư mời phỏng vấn");
                        cmd.Parameters.AddWithValue("@nd", ttpv.noi_dung);
                        cmd.Parameters.AddWithValue("@ltb", "thu_Moi_Phong_Van");
                        cmd.Parameters.AddWithValue("@ma_ct", ttpv.ma_cong_ty);
                        cmd.Parameters.AddWithValue("@ma_ntv", ttpv.ma_nguoi_tim_viec);
                        cmd.Parameters.AddWithValue("@ngay_tao", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT LAST_INSERT_ID();";
                        cmd.Parameters.Clear();
                        ma_thong_bao = Convert.ToInt32(cmd.ExecuteScalar()); // lay id ma thong bao moi insert
                    }


                    string chi_tiet_tm = "insert into chi_tiet_thu_moi(ma_thong_bao, ma_cong_ty, thoi_gian, dia_diem, noi_dung, ma_nguoi_tim_viec) values(@ma_tb, @ma_ct, @thoi_gian, @dia_diem, @nd, @ma_ntv)";
                    using (var cmd = new MySqlCommand(chi_tiet_tm, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@ma_tb", ma_thong_bao);
                        cmd.Parameters.AddWithValue("@ma_ct", ttpv.ma_cong_ty);
                        cmd.Parameters.AddWithValue("@thoi_gian", ttpv.thoi_gian);
                        cmd.Parameters.AddWithValue("@dia_diem", ttpv.dia_diem);
                        cmd.Parameters.AddWithValue("@nd", ttpv.noi_dung);
                        cmd.Parameters.AddWithValue("@ma_ntv", ttpv.ma_nguoi_tim_viec);
                        cmd.ExecuteNonQuery();
                    }


                    string cap_nhat_trang_thai = "update ung_tuyen set trang_thai = @trang_Thai, trang_thai_duyet = @trang_thai_duyet where ma_nguoi_tim_viec = @ma_ntv and ma_viec = @ma_viec";
                    using (var cmd = new MySqlCommand(cap_nhat_trang_thai, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@ma_viec", ttpv.ma_viec);
                        cmd.Parameters.AddWithValue("@trang_Thai", ttpv.trang_thai);
                        cmd.Parameters.AddWithValue("@ma_ntv", ttpv.ma_nguoi_tim_viec);
                        cmd.Parameters.AddWithValue("@trang_thai_duyet", ttpv.trang_thai_duyet);
                        cmd.ExecuteNonQuery();
                    }

                    string them_trang_thai_thong_bao = @"insert into trang_thai_thong_bao (ma_nguoi_nhan, ma_thong_bao, loai_nguoi_nhan, trang_thai_doc, trang_thai_an, ngay_tao, ngay_cap_nhat) 
                                            values (@ma_nguoi_nhan, @ma_thong_bao, 'nguoi_Dung', 0, 0, now(), now())";
                    using (var cmd = new MySqlCommand(them_trang_thai_thong_bao, coon, trans))
                    {
                        cmd.Parameters.AddWithValue("@ma_nguoi_nhan", ttpv.ma_nguoi_nhan);
                        cmd.Parameters.AddWithValue("@ma_thong_bao", ma_thong_bao);
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    email = layEmailNguoiTimViec(ttpv.ma_nguoi_tim_viec);
                    _ = capNhatTrangThaiUngTuyen(email);
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
            catch
            {
                return false;
            }
        }
        private static string layEmailNguoiTimViec(int ma_nguoi_tim_viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select email from nguoi_tim_viec where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_nguoi_tim_viec);
            using var reader = cmd.ExecuteReader();
            string email = "";
            if (reader.Read())
            {
                email = reader["email"]?.ToString() ?? "";
            }
            return email;
        }

        private static async Task<bool> capNhatTrangThaiUngTuyen(string email_yeu_cau)
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
                    Subject = "Trạng thái mới về việc làm đã ứng tuyển của bạn",
                    Body = @"Xin chào, Trạng thái ứng tuyển của bạn cho vị trí đã đăng ký vừa được cập nhật. 
                                Hãy đăng nhập vào trang JobFinder để xem chi tiết và nhận những thông tin mới nhất về cơ hội nghề nghiệp. 
                                Cảm ơn bạn đã sử dụng JobFinder!",
                    IsBodyHtml = false
                };

                mail.To.Add(email_yeu_cau);

                Console.WriteLine(email_yeu_cau);

                await smtp.SendMailAsync(mail);

                Console.WriteLine("Gửi email thành công!");
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