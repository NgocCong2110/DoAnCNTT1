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
SELECT u.ma_ung_tuyen, u.ma_viec, u.ma_nguoi_tim_viec, u.ma_cong_ty, u.ma_cv, u.duong_dan_file_cv_upload,
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
            string cap_nhat_trang_thai = "update ung_tuyen set trang_thai = @trang_thai where ma_cong_ty = @ma_cong_ty and ma_nguoi_tim_viec = @ma_ntv";
            using var cmd = new MySqlCommand(cap_nhat_trang_thai, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ung_Tuyen.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_ntv", ung_Tuyen.ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@trang_thai", "tu_Choi");
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool xoaUngVien(ung_tuyen ung_Tuyen)
        {
            try
            {
                using var coon = new MySqlConnection(chuoi_KetNoi);
                coon.Open();
                string sql = "DELETE FROM ung_tuyen WHERE ma_cong_ty = @ma_Cong_Ty AND ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec";
                using var cmd = new MySqlCommand(sql, coon);
                cmd.Parameters.AddWithValue("@ma_Cong_Ty", ung_Tuyen.ma_cong_ty);
                cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ung_Tuyen.ma_nguoi_tim_viec);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa ứng viên: " + ex.Message);
                return false;
            }
        }

        public static bool guiThuMoiPhongVan(thong_tin_phong_van ttpv)
        {
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


                    trans.Commit();
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
    }
}