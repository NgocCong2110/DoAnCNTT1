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


namespace DotNet_WEB.Module
{
    public class Module_NTV
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<ung_tuyen> layDanhSachUngTuyen(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select ut.ngay_ung_tuyen, vl.nganh_nghe, vl.vi_tri, vl.kinh_nghiem, vl.muc_luong, vl.yeu_cau,
	                            ct.ten_cong_ty, ct.dia_chi, ut.trang_thai
                            from ung_tuyen ut
                            join nguoi_tim_viec ntv on ut.ma_nguoi_tim_viec = ntv.ma_nguoi_tim_viec
                            join viec_lam vl on ut.ma_viec = vl.ma_viec
                            join cong_ty ct on ut.ma_cong_ty = ct.ma_cong_ty
                            where ut.ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_Nguoi_Tim_Viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach_lich_su = new List<ung_tuyen>();
            while (reader.Read())
            {
                var ung_Tuyen = new ung_tuyen
                {
                    ngay_ung_tuyen = reader.IsDBNull(reader.GetOrdinal("ngay_ung_tuyen")) ? DateTime.MinValue : reader.GetDateTime("ngay_ung_tuyen"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai")) ? TrangThaiUngTuyen.None : (TrangThaiUngTuyen)Enum.Parse(typeof(TrangThaiUngTuyen), reader.GetString("trang_thai")),
                    viec_Lam = new viec_lam
                    {
                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),
                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),
                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                        yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau")
                    },
                    cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                        dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi")
                    }
                };

                danh_sach_lich_su.Add(ung_Tuyen);
            }
            return danh_sach_lich_su;
        }

        public static async Task<bool> UploadCvAsync(IFormFile cvFile, int ma_Nguoi_Tim_Viec)
        {
            if (cvFile == null || cvFile.Length == 0)
                return false;

            var duongDanFolder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCV");
            if (!Directory.Exists(duongDanFolder))
                Directory.CreateDirectory(duongDanFolder);

            var tenFile = $"{Guid.NewGuid()}_{cvFile.FileName}";
            var duongDanFile = Path.Combine(duongDanFolder, tenFile);

            using (var stream = new FileStream(duongDanFile, FileMode.Create))
            {
                await cvFile.CopyToAsync(stream);
            }

            using (var conn = new MySqlConnection(chuoi_KetNoi))
            {
                string sql = @"INSERT INTO cv_nguoi_tim_viec 
                           (ma_nguoi_tim_viec, ten_file, duong_dan_file, ngay_tao) 
                           VALUES (@ma, @ten, @duong, now())";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ma", ma_Nguoi_Tim_Viec);
                cmd.Parameters.AddWithValue("@ten", cvFile.FileName);
                cmd.Parameters.AddWithValue("@duong", duongDanFile);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }

        public static bool luuCv(cv_online_nguoi_tim_viec cv_on)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            string sql = @"INSERT INTO cv_nguoi_tim_viec_online 
                       (ma_nguoi_tim_viec, ho_ten, email, dien_thoai, ngay_sinh, dia_chi,
                        truong_hoc, chuyen_nganh, kinh_nghiem, ky_nang, du_an, muc_tieu, ngay_tao)
                       VALUES (@ma, @hoTen, @email, @dienThoai, @ngaySinh, @diaChi,
                               @truongHoc, @chuyenNganh, @kinhNghiem, @kyNang, @duAn, @mucTieu, GETDATE())";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma", (int)cv_on.ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@hoTen", cv_on.ho_ten);
            cmd.Parameters.AddWithValue("@email", cv_on.email);
            cmd.Parameters.AddWithValue("@dienThoai", cv_on.dien_thoai);
            cmd.Parameters.AddWithValue("@ngaySinh", cv_on.ngay_sinh);
            cmd.Parameters.AddWithValue("@diaChi", cv_on.dia_chi);
            cmd.Parameters.AddWithValue("@truongHoc", cv_on.truong_hoc);
            cmd.Parameters.AddWithValue("@chuyenNganh", cv_on.chuyen_nganh);
            cmd.Parameters.AddWithValue("@kinhNghiem", cv_on.kinh_nghiem);
            cmd.Parameters.AddWithValue("@kyNang", cv_on.ky_nang);
            cmd.Parameters.AddWithValue("@duAn", cv_on.du_an);
            cmd.Parameters.AddWithValue("@mucTieu", cv_on.muc_tieu);

            coon.Open();
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static List<cv_nguoi_tim_viec> layDanhSachCV(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from cv_nguoi_tim_viec where ma_nguoi_tim_viec = @ma";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma", ma_Nguoi_Tim_Viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<cv_nguoi_tim_viec>();
            while (reader.Read())
            {
                var cv = new cv_nguoi_tim_viec
                {
                    ma_cv = reader.IsDBNull(reader.GetOrdinal("ma_cv")) ? 0 : reader.GetInt32("ma_cv"),

                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ten_file = reader.IsDBNull(reader.GetOrdinal("ten_file")) ? null : reader.GetString("ten_file"),

                    duong_dan_file = reader.IsDBNull(reader.GetOrdinal("duong_dan_file")) ? null : reader.GetString("duong_dan_file"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danh_sach.Add(cv);
            }
            return danh_sach;
        }

        public static bool capNhatThongTinNguoiTimViec(int ma_Nguoi_Tim_Viec, string field, string value)
        {
            string[] allowedFields = { "email", "dien_thoai", "ngay_sinh", "gioi_tinh",
                               "trinh_do_hoc_van", "dia_chi", "quoc_tich", "mo_ta" };

            if (!allowedFields.Contains(field))
                throw new ArgumentException("Field không hợp lệ");

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string sql = $"UPDATE nguoi_tim_viec SET {field} = @value WHERE ma_nguoi_tim_viec = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@value", value);
            cmd.Parameters.AddWithValue("@id", ma_Nguoi_Tim_Viec);

            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}