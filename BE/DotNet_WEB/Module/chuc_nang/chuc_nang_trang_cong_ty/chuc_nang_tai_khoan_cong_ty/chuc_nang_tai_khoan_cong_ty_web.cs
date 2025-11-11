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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty
{
    public class chuc_nang_tai_khoan_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        private static readonly string[] allowedFields = new string[]
        {
            "ten_cong_ty",
            "email",
            "dien_thoai",
            "website",
            "nguoi_dai_dien",
            "ma_so_thue",
            "nam_thanh_lap",
            "dia_chi",
            "loai_hinh_cong_ty",
            "quy_mo",
            "trang_thai",
            "mo_ta"
        };
        public static bool capNhatThongTinCongTy(thong_tin_truong_du_lieu_cap_nhat req)
        {
            if (req == null || string.IsNullOrEmpty(req.truong))
                return false;

            if (!allowedFields.Contains(req.truong))
                return false;

            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string sql = $"UPDATE cong_ty SET {req.truong} = @gia_tri, ngay_cap_nhat = NOW() WHERE ma_cong_ty = @ma_cong_ty";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@gia_tri", req.gia_tri);
            cmd.Parameters.AddWithValue("@ma_cong_ty", req.ma_cong_ty);

            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool kiemTraMatKhauCongTy(cong_ty cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string mat_khau = maHoaMatKhau(cong_Ty.mat_khau_dn_cong_ty);
            string sql = "select count(*) from cong_ty where mat_khau_dn_cong_ty = @mat_khau and ma_cong_ty = @ma_cong_ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@mat_khau", mat_khau);
            cmd.Parameters.AddWithValue("@ma_cong_ty", cong_Ty.ma_cong_ty);
            int rows = Convert.ToInt32(cmd.ExecuteScalar());
            return rows > 0;
        }

        public static bool capNhatMatKhauCongTy(cong_ty thong_tin_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string mat_khau_ma_hoa = maHoaMatKhau(thong_tin_cong_ty.mat_khau_dn_cong_ty);

            using var trans = coon.BeginTransaction();
            try
            {
                string sql_cap_nhat_cong_ty = "UPDATE cong_ty SET mat_khau_dn_cong_ty = @mat_khau WHERE ma_cong_ty = @ma_cong_ty";
                using var cmd_cap_nhat_cong_ty = new MySqlCommand(sql_cap_nhat_cong_ty, coon, trans);
                cmd_cap_nhat_cong_ty.Parameters.AddWithValue("@mat_khau", mat_khau_ma_hoa);
                cmd_cap_nhat_cong_ty.Parameters.AddWithValue("@ma_cong_ty", thong_tin_cong_ty.ma_cong_ty);
                int so_dong_cap_nhat_cong_ty = cmd_cap_nhat_cong_ty.ExecuteNonQuery();

                string sql_cap_nhat_nguoi_dung = "UPDATE nguoi_dung SET mat_khau = @mat_khau WHERE ma_cong_ty = @ma_cong_ty";
                using var cmd_cap_nhat_nguoi_dung = new MySqlCommand(sql_cap_nhat_nguoi_dung, coon, trans);
                cmd_cap_nhat_nguoi_dung.Parameters.AddWithValue("@mat_khau", mat_khau_ma_hoa);
                cmd_cap_nhat_nguoi_dung.Parameters.AddWithValue("@ma_cong_ty", thong_tin_cong_ty.ma_cong_ty);
                int so_dong_cap_nhat_nguoi_dung = cmd_cap_nhat_nguoi_dung.ExecuteNonQuery();

                trans.Commit();

                return so_dong_cap_nhat_cong_ty > 0 || so_dong_cap_nhat_nguoi_dung > 0;
            }
            catch (Exception loi_cap_nhat)
            {
                trans.Rollback();
                Console.WriteLine("Lỗi khi cập nhật mật khẩu: " + loi_cap_nhat.Message);
                return false;
            }
        }


        public static List<cong_ty> layThongTinCongTy(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "SELECT * FROM cong_ty WHERE ma_cong_ty = @ma_cong_ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
            var danh_sach = new List<cong_ty>();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var ct = new cong_ty
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                    ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                    ten_dn_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_dn_cong_ty")) ? null : reader.GetString("ten_dn_cong_ty"),
                    nguoi_dai_dien = reader.IsDBNull(reader.GetOrdinal("nguoi_dai_dien")) ? null : reader.GetString("nguoi_dai_dien"),
                    ma_so_thue = reader.IsDBNull(reader.GetOrdinal("ma_so_thue")) ? null : reader.GetString("ma_so_thue"),
                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi"),
                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? null : reader.GetString("dien_thoai"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                    website = reader.IsDBNull(reader.GetOrdinal("website")) ? null : reader.GetString("website"),
                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),
                    anh_bia = reader.IsDBNull(reader.GetOrdinal("anh_bia")) ? null : reader.GetString("anh_bia"),
                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                                        ? LoaiHinhCongTy.congty_CoPhan
                                        : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),
                    quy_mo = reader.IsDBNull(reader.GetOrdinal("quy_mo")) ? null : reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap")) ? null : reader.GetInt32("nam_thanh_lap"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                                ? TrangThaiCongTy.hoat_Dong
                                : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.Now : reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.Now : reader.GetDateTime("ngay_cap_nhat")
                };
                danh_sach.Add(ct);
            }

            reader.Close();

            string sql_phuc_loi = "SELECT * FROM phuc_loi_cong_ty WHERE ma_cong_ty = @ma_cong_ty";
            using (var cmd_phuc_loi = new MySqlCommand(sql_phuc_loi, coon))
            {
                cmd_phuc_loi.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                using var reader_phuc_loi = cmd_phuc_loi.ExecuteReader();
                var danh_sach_phuc_loi = new List<phuc_loi_cong_ty>();

                while (reader_phuc_loi.Read())
                {
                    danh_sach_phuc_loi.Add(new phuc_loi_cong_ty
                    {
                        ma_phuc_loi_cty = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ma_phuc_loi_cty")) ? 0 : reader_phuc_loi.GetInt32("ma_phuc_loi_cty"),
                        ma_cong_ty = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ma_cong_ty")) ? 0 : reader_phuc_loi.GetInt32("ma_cong_ty"),
                        ten_phuc_loi = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("ten_phuc_loi")) ? null : reader_phuc_loi.GetString("ten_phuc_loi"),
                        mo_ta = reader_phuc_loi.IsDBNull(reader_phuc_loi.GetOrdinal("mo_ta")) ? null : reader_phuc_loi.GetString("mo_ta")
                    });
                }
                reader_phuc_loi.Close();

                if (danh_sach.Count > 0)
                    danh_sach[0].phuc_Loi_Cong_Ty = danh_sach_phuc_loi;
            }

            string sql_mang_xa_hoi = "SELECT * FROM lien_ket_mang_xa_hoi WHERE ma_cong_ty = @ma_cong_ty";
            using (var cmd_mang_xa_hoi = new MySqlCommand(sql_mang_xa_hoi, coon))
            {
                cmd_mang_xa_hoi.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                using var reader_mang_xa_hoi = cmd_mang_xa_hoi.ExecuteReader();
                var danh_sach_lien_ket = new List<lien_ket_mang_xa_hoi>();

                while (reader_mang_xa_hoi.Read())
                {
                    danh_sach_lien_ket.Add(new lien_ket_mang_xa_hoi
                    {
                        ma_lien_ket = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ma_lien_ket")) ? 0 : reader_mang_xa_hoi.GetInt32("ma_lien_ket"),
                        ma_cong_ty = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ma_cong_ty")) ? 0 : reader_mang_xa_hoi.GetInt32("ma_cong_ty"),
                        ten_mang_xa_hoi = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("ten_mang_xa_hoi")) ? null : reader_mang_xa_hoi.GetString("ten_mang_xa_hoi"),
                        duong_dan = reader_mang_xa_hoi.IsDBNull(reader_mang_xa_hoi.GetOrdinal("duong_dan")) ? null : reader_mang_xa_hoi.GetString("duong_dan")
                    });
                }
                reader_mang_xa_hoi.Close();

                if (danh_sach.Count > 0)
                    danh_sach[0].lien_Ket_Mang_Xa_Hoi = danh_sach_lien_ket;
            }
            return danh_sach;
        }

        public static async Task<string> capNhatAnhBiaCongTy(IFormFile file, int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string anh_bia_cu = "";
            string lay_anh_bia_cu = "select anh_bia from cong_ty where ma_cong_ty = @ma_cong_ty";
            using (var cmd = new MySqlCommand(lay_anh_bia_cu, coon))
            {
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    anh_bia_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhBiaCongTy");
            if (!Path.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory("LuuTruAnhBiaCongTy");
            }

            var ten_file = $"{Guid.NewGuid()}_{file.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruAnhBiaCongTy/{ten_file}";

            string sql = "update cong_ty set anh_bia = @duong_dan, ngay_cap_nhat = now() where ma_cong_ty = @ma_cong_ty";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan", duong_dan);
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                await cmd.ExecuteNonQueryAsync();
            }

            if (!string.IsNullOrEmpty(anh_bia_cu))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), anh_bia_cu.Replace("/", "\\"));
                if (Path.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
            }
            return duong_dan;
        }

        public static async Task<string> capNhatLogoCongTy(IFormFile file, int ma_cong_ty)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string logo_cu = "";
            string lay_lo_cu = "select logo from cong_ty where ma_cong_ty = @ma_cong_ty";
            using (var cmd_lay_logo = new MySqlCommand(lay_lo_cu, coon))
            {
                cmd_lay_logo.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                var result = await cmd_lay_logo.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value)
                {
                    logo_cu = result.ToString();
                }
            }

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruLogoCongTy");
            if (!Directory.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory(duong_dan_folder);
            }

            var ten_file = $"{Guid.NewGuid()}_{file.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruLogoCongTy/{ten_file}";

            string sql = "Update cong_ty set logo = @duong_dan_logo, ngay_cap_nhat = now() where ma_cong_ty = @ma_cong_ty";
            using (var cmd = new MySqlCommand(sql, coon))
            {
                cmd.Parameters.AddWithValue("@duong_dan_logo", duong_dan);
                cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);
                await cmd.ExecuteNonQueryAsync();
            }

            if (!string.IsNullOrEmpty(logo_cu))
            {
                var duong_dan_cu = Path.Combine(Directory.GetCurrentDirectory(), logo_cu.Replace("/", "\\"));
                if (File.Exists(duong_dan_cu))
                {
                    File.Delete(duong_dan_cu);
                }
            }
            return duong_dan;
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
    public class thong_tin_truong_du_lieu_cap_nhat
    {
        public int ma_cong_ty { get; set; }
        public string? truong { get; set; }
        public string? gia_tri { get; set; }
    }

}