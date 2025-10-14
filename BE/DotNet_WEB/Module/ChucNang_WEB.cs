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

namespace DotNet_WEB.Module
{
    public class ChucNang_WEB
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool kiemTraTaiKhoanDangKy(nguoi_dung nguoi_Dung)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select 1 from nguoi_dung where email = @email";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@email", nguoi_Dung.email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return false;
            }
            return true;
        }
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
            if (cong_Ty.mat_khau_dn_cong_ty == null || cong_Ty.email == null)
            {
                return false;
            }
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            using var transaction = conn.BeginTransaction();
            string matKhauMaHoa = XacThuc_ND.maHoaMatKhau(cong_Ty.mat_khau_dn_cong_ty);
            try
            {
                string them_CongTy = @"
            INSERT INTO cong_ty (ten_dn_cong_ty, email, mat_khau_dn_cong_ty) 
            VALUES (@ten_dn_cong_ty, @email, @mat_khau_dn_cong_ty);
            SELECT LAST_INSERT_ID();";

                using var cmd = new MySqlCommand(them_CongTy, conn, transaction);
                cmd.Parameters.AddWithValue("@ten_dn_cong_ty", cong_Ty.ten_dn_cong_ty);
                cmd.Parameters.AddWithValue("@email", cong_Ty.email);
                cmd.Parameters.AddWithValue("@mat_khau_dn_cong_ty", matKhauMaHoa);

                long ma_CongTy = Convert.ToInt64(cmd.ExecuteScalar());

                string them_NguoiDung = @"
            INSERT INTO nguoi_dung (loai_nguoi_dung, ma_cong_ty, ten_dang_nhap, mat_khau, email) 
            VALUES ('cong_Ty', @ma_cong_ty, @ten_dang_nhap, @mat_khau, @email);";

                using var cmd2 = new MySqlCommand(them_NguoiDung, conn, transaction);
                cmd2.Parameters.AddWithValue("@ma_cong_ty", ma_CongTy);
                cmd2.Parameters.AddWithValue("@ten_dang_nhap", cong_Ty.ten_dn_cong_ty);
                cmd2.Parameters.AddWithValue("@mat_khau", matKhauMaHoa);
                cmd2.Parameters.AddWithValue("@email", cong_Ty.email);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

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
                        cmd.Parameters.AddWithValue("@email",email);
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


        public static List<nguoi_dung> thongTinNguoiDungBangEmail(string email)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string query = "SELECT * FROM nguoi_dung WHERE email = @email";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@email", email);

            var danhSachNguoiDung = new List<nguoi_dung>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var nguoiDung = new nguoi_dung
                {
                    ma_nguoi_dung = reader.GetInt32("ma_nguoi_dung"),

                    loai_nguoi_dung = reader.IsDBNull(reader.GetOrdinal("loai_nguoi_dung")) ? LoaiNguoiDung.None : (LoaiNguoiDung)Enum.Parse(typeof(LoaiNguoiDung), reader.GetString("loai_nguoi_dung")),

                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? null : reader.GetInt32("ma_nguoi_tim_viec"),

                    ten_dang_nhap = reader.IsDBNull(reader.GetOrdinal("ten_dang_nhap")) ? string.Empty : reader.GetString("ten_dang_nhap")
                };

                if (nguoiDung.loai_nguoi_dung == LoaiNguoiDung.cong_Ty && nguoiDung.ma_cong_ty.HasValue)
                {
                    nguoiDung.cong_ty = layChiTietCongTy(nguoiDung.ma_cong_ty.Value);
                }
                else if (nguoiDung.loai_nguoi_dung == LoaiNguoiDung.nguoi_Tim_Viec && nguoiDung.ma_nguoi_tim_viec.HasValue)
                {
                    nguoiDung.nguoi_tim_viec = layChiTietNguoiTimViec(nguoiDung.ma_nguoi_tim_viec.Value);
                }
                danhSachNguoiDung.Add(nguoiDung);
            }
            return danhSachNguoiDung;
        }

        public static List<quan_tri> layDanhSachQuanTri(string email_Quan_Tri)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from quan_tri where email = @email_Quan_Tri";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("email_Quan_Tri", email_Quan_Tri);
            using var reader = cmd.ExecuteReader();
            var thong_tin = new List<quan_tri>();
            if (reader.Read())
            {
                var quan_Tri = new quan_tri
                {
                    ma_quan_tri = reader.GetInt32("ma_quan_tri"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    ho_ten = reader.GetString("ho_ten"),
                    email = reader.GetString("email")
                };
                thong_tin.Add(quan_Tri);
            }
            return thong_tin;
        }

        private static cong_ty? layChiTietCongTy(int maCongTy)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string query = "SELECT * FROM cong_ty WHERE ma_cong_ty = @ma";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@ma", maCongTy);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new cong_ty
                {
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    ten_dn_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_dn_cong_ty")) ? string.Empty : reader.GetString("ten_dn_cong_ty"),

                    ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? string.Empty : reader.GetString("ten_cong_ty"),

                    nguoi_dai_dien = reader.IsDBNull(reader.GetOrdinal("nguoi_dai_dien")) ? string.Empty : reader.GetString("nguoi_dai_dien"),

                    ma_so_thue = reader.IsDBNull(reader.GetOrdinal("ma_so_thue")) ? string.Empty : reader.GetString("ma_so_thue"),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? string.Empty : reader.GetString("dia_chi"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? string.Empty : reader.GetString("dien_thoai"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),

                    website = reader.IsDBNull(reader.GetOrdinal("website")) ? string.Empty : reader.GetString("website"),

                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                    ? LoaiHinhCongTy.None
                    : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),

                    quy_mo = reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap"))
                    ? null
                    : (int?)reader.GetInt16("nam_thanh_lap"),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                    ? TrangThaiCongTy.None
                    : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }

        private static nguoi_tim_viec? layChiTietNguoiTimViec(int maNguoiTimViec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string query = "SELECT * FROM nguoi_tim_viec WHERE ma_nguoi_tim_viec = @ma";
            using var cmd = new MySqlCommand(query, coon);
            cmd.Parameters.AddWithValue("@ma", maNguoiTimViec);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec"),

                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? string.Empty : reader.GetString("ho_ten"),

                    ten_dang_nhap = reader.IsDBNull(reader.GetOrdinal("ten_dang_nhap")) ? string.Empty : reader.GetString("ten_dang_nhap"),

                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString("email"),

                    dien_thoai = reader.IsDBNull(reader.GetOrdinal("dien_thoai")) ? string.Empty : reader.GetString("dien_thoai"),

                    mat_khau = reader.IsDBNull(reader.GetOrdinal("mat_khau")) ? string.Empty : reader.GetString("mat_khau"),

                    ngay_sinh = reader.IsDBNull(reader.GetOrdinal("ngay_sinh")) ? DateTime.MinValue : reader.GetDateTime("ngay_sinh"),

                    gioi_tinh = reader.IsDBNull(reader.GetOrdinal("gioi_tinh")) ? GioiTinh.None : (GioiTinh)Enum.Parse(typeof(GioiTinh), reader.GetString("gioi_tinh")),

                    trinh_do_hoc_van = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van")) ? TrinhDoHocVan.None : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van")),

                    dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? string.Empty : reader.GetString("dia_chi"),

                    anh_dai_dien = reader.IsDBNull(reader.GetOrdinal("anh_dai_dien")) ? string.Empty : reader.GetString("anh_dai_dien"),

                    quoc_tich = reader.IsDBNull(reader.GetOrdinal("quoc_tich")) ? string.Empty : reader.GetString("quoc_tich"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? string.Empty : reader.GetString("mo_ta"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }

        public static List<bai_dang> layDanhSachBaiDang()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
        SELECT 
            ma_bai_dang,
            ma_nguoi_dang,
            ten_nguoi_dang,
            tieu_de,
            noi_dung,
            loai_bai,
            trang_thai,
            ngay_tao,
            ngay_cap_nhat
        FROM bai_dang;
    ";

            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();

            var danh_sach_bai_dang = new List<bai_dang>();
            while (reader.Read())
            {
                var danh_sach = new bai_dang
                {
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ma_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_dang")) ? 0 : reader.GetInt32("ma_nguoi_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    loai_bai = reader.IsDBNull(reader.GetOrdinal("loai_bai")) ? LoaiBai.None : (LoaiBai)Enum.Parse(typeof(LoaiBai), reader.GetString("loai_bai")),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai")) ? TrangThaiBai.None : (TrangThaiBai)Enum.Parse(typeof(TrangThaiBai), reader.GetString("trang_thai")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat"),

                };
                danh_sach_bai_dang.Add(danh_sach);
            }

            return danh_sach_bai_dang;
        }

        public static viec_lam? layViecLamTheoBaiDang(int ma_bai_dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"SELECT * FROM viec_lam WHERE ma_bai_dang = @ma_bai_dang LIMIT 1";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", ma_bai_dang);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new viec_lam
                {
                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),

                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                    vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),

                    kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),

                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),

                    loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }

        public static List<bai_dang> layBaiDangTheoMa(int ma_Bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from bai_dang where ma_bai_dang = @ma_Bai_Dang";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Bai_Dang", ma_Bai_Dang);
            using var reader = cmd.ExecuteReader();
            var thong_tin = new List<bai_dang>();
            if (reader.Read())
            {
                var bai_d = new bai_dang
                {
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ma_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_dang")) ? 0 : reader.GetInt32("ma_nguoi_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    loai_bai = reader.IsDBNull(reader.GetOrdinal("loai_bai")) ? LoaiBai.None : (LoaiBai)Enum.Parse(typeof(LoaiBai), reader.GetString("loai_bai")),

                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai")) ? TrangThaiBai.None : (TrangThaiBai)Enum.Parse(typeof(TrangThaiBai), reader.GetString("trang_thai")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
                thong_tin.Add(bai_d);
            }
            return thong_tin;
        }

        public static bool themBaiDangMoi(bai_dang bai_Dang, viec_lam viec_Lam)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string sqlBaiDang = @"INSERT INTO bai_dang(ma_nguoi_dang, ten_nguoi_dang, tieu_de, noi_dung, loai_bai, trang_thai, ngay_tao, ngay_cap_nhat) 
                              VALUES(@ma_nguoi_dang, @ten_nguoi_dang, @tieu_de, @noi_dung, @loai_bai, @trang_thai, @ngay_tao, @ngay_cap_nhat)";
                using var cmdBaiDang = new MySqlCommand(sqlBaiDang, coon, trans);
                cmdBaiDang.Parameters.AddWithValue("@ma_nguoi_dang", bai_Dang.ma_nguoi_dang);
                cmdBaiDang.Parameters.AddWithValue("@ten_nguoi_dang", bai_Dang.ten_nguoi_dang);
                cmdBaiDang.Parameters.AddWithValue("@tieu_de", bai_Dang.tieu_de);
                cmdBaiDang.Parameters.AddWithValue("@noi_dung", bai_Dang.noi_dung);
                cmdBaiDang.Parameters.AddWithValue("@loai_bai", bai_Dang.loai_bai.ToString());
                cmdBaiDang.Parameters.AddWithValue("@trang_thai", bai_Dang.trang_thai.ToString());
                cmdBaiDang.Parameters.AddWithValue("@ngay_tao", bai_Dang.ngay_tao);
                cmdBaiDang.Parameters.AddWithValue("@ngay_cap_nhat", bai_Dang.ngay_cap_nhat);
                cmdBaiDang.ExecuteNonQuery();

                long maBaiDang = cmdBaiDang.LastInsertedId;

                if (viec_Lam != null)
                {
                    string sqlViecLam = @"INSERT INTO viec_lam(ma_cong_ty, nganh_nghe, vi_tri, kinh_nghiem, tieu_de, mo_ta, yeu_cau, muc_luong, dia_diem, loai_hinh, so_luong, ma_bai_dang) 
                                  VALUES(@ma_cong_ty, @nganh_nghe, @vi_tri, @kinh_nghiem, @tieu_de, @mo_ta, @yeu_cau, @muc_luong, @dia_diem, @loai_hinh, @so_luong, @ma_bai_dang)";
                    using var cmdViecLam = new MySqlCommand(sqlViecLam, coon, trans);
                    cmdViecLam.Parameters.AddWithValue("@ma_cong_ty", viec_Lam.ma_cong_ty);
                    cmdViecLam.Parameters.AddWithValue("@nganh_nghe", viec_Lam.nganh_nghe);
                    cmdViecLam.Parameters.AddWithValue("@vi_tri", viec_Lam.vi_tri);
                    cmdViecLam.Parameters.AddWithValue("@kinh_nghiem", viec_Lam.kinh_nghiem);
                    cmdViecLam.Parameters.AddWithValue("@tieu_de", viec_Lam.tieu_de);
                    cmdViecLam.Parameters.AddWithValue("@mo_ta", viec_Lam.mo_ta);
                    cmdViecLam.Parameters.AddWithValue("@yeu_cau", viec_Lam.yeu_cau);
                    cmdViecLam.Parameters.AddWithValue("@muc_luong", viec_Lam.muc_luong);
                    cmdViecLam.Parameters.AddWithValue("@dia_diem", viec_Lam.dia_diem);
                    cmdViecLam.Parameters.AddWithValue("@loai_hinh", viec_Lam.loai_hinh.ToString());
                    cmdViecLam.Parameters.AddWithValue("@ma_bai_dang", maBaiDang);
                    cmdViecLam.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }

        public static string layHoTenNguoiDung(int ma_nguoi_dung)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string lay_ho_ten_ntv = @"SELECT ntv.ho_ten FROM nguoi_tim_viec ntv 
        JOIN nguoi_dung nd ON ntv.ma_nguoi_tim_viec = nd.ma_nguoi_dung WHERE nd.ma_nguoi_dung = @ma_nguoi_dung";
            using (var cmd = new MySqlCommand(lay_ho_ten_ntv, coon))
            {
                cmd.Parameters.AddWithValue("@ma_nguoi_dung", ma_nguoi_dung);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetString("ho_ten");
                }
            }
            string lay_ho_ten_cong_ty = @"SELECT ct.ten_cong_ty FROM cong_ty ct 
        JOIN nguoi_dung nd ON ct.ma_cong_ty = nd.ma_nguoi_dung WHERE nd.ma_nguoi_dung = @ma_nguoi_dung";
            using (var cmd2 = new MySqlCommand(lay_ho_ten_cong_ty, coon))
            {
                cmd2.Parameters.AddWithValue("@ma_nguoi_dung", ma_nguoi_dung);
                using var reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    return reader2.GetString("ten_cong_ty");
                }
            }
            return "Không tìm thấy người dùng";
        }

        public static bool luuBaiDangViPham(bai_dang_vi_pham bai_Dang_Vi_Pham)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string luu_bai_vi_pham = "insert into bai_dang_vi_pham values(@ma_bai_vi_pham, @ten_nguoi_dang, @tieu_de, @noi_dung, @ma_nguoi_bao_cao, @noi_dung_bao_cao, @ngay_bao_cao)";
            using var cmd = new MySqlCommand(luu_bai_vi_pham, coon);
            cmd.Parameters.AddWithValue("@ma_bai_vi_pham", bai_Dang_Vi_Pham.ma_bai_vi_pham);
            cmd.Parameters.AddWithValue("@ten_nguoi_dang", bai_Dang_Vi_Pham.ten_nguoi_dang);
            cmd.Parameters.AddWithValue("@tieu_de", bai_Dang_Vi_Pham.tieu_de);
            cmd.Parameters.AddWithValue("@noi_dung", bai_Dang_Vi_Pham.noi_dung);
            cmd.Parameters.AddWithValue("@ma_nguoi_bao_cao", bai_Dang_Vi_Pham.ma_nguoi_bao_cao);
            cmd.Parameters.AddWithValue("@noi_dung_bao_cao", bai_Dang_Vi_Pham.noi_dung_bao_cao);
            cmd.Parameters.AddWithValue("@ngay_bao_cao", bai_Dang_Vi_Pham.ngay_bao_cao);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static bool luuBaiDang(bai_dang_da_luu bai_Dang_Da_Luu)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string luu_bai_dang = "INSERT INTO bai_dang_da_luu (ma_bai_dang, ma_nguoi_luu) VALUES (@ma_bai_dang, @ma_nguoi_luu)";
            using var cmd = new MySqlCommand(luu_bai_dang, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang_Da_Luu.ma_bai_dang);
            cmd.Parameters.AddWithValue("@ma_nguoi_luu", bai_Dang_Da_Luu.ma_nguoi_luu);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static List<bai_dang> layDanhSachBaiDangDaLuu(int ma_Nguoi_Luu)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string lay_danh_sach_bai_dang_da_luu = @"select b.ma_bai_dang, b.ma_nguoi_dang, b.ten_nguoi_dang, 
                                                    b.tieu_de, b.noi_dung, b.ngay_tao, b.ngay_cap_nhat
                                                    from bai_dang_da_luu l inner join bai_dang b on l.ma_bai_dang = b.ma_bai_dang 
                                                    where l.ma_nguoi_luu = @ma_Nguoi_Luu";
            using var cmd = new MySqlCommand(lay_danh_sach_bai_dang_da_luu, coon);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Luu", ma_Nguoi_Luu);
            using var reader = cmd.ExecuteReader();
            var danh_sach_da_luu = new List<bai_dang>();
            while (reader.Read())
            {
                var bai_Dang = new bai_dang
                {
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ma_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_dang")) ? 0 : reader.GetInt32("ma_nguoi_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
                danh_sach_da_luu.Add(bai_Dang);
            }
            return danh_sach_da_luu;
        }

        public static bool ungTuyenCongViec(int ma_Viec, int ma_Cong_Ty, int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string ung_tuyen = "INSERT INTO ung_tuyen (ma_viec, ma_cong_ty, ma_nguoi_tim_viec, trang_thai) VALUES (@ma_Viec, @ma_Cong_Ty, @ma_Nguoi_Tim_Viec, @trang_Thai)";
            using var cmd = new MySqlCommand(ung_tuyen, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ma_Viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_Nguoi_Tim_Viec);
            cmd.Parameters.AddWithValue("@trang_Thai", "dang_Cho");
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static List<thong_bao> layDanhSachThongBao(thong_bao_kieu_nguoi_dung tb_knd)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            if (tb_knd.kieu_nguoi_dung == "nguoi_Tim_Viec")
            {
                sql = @"select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, qt.ho_ten,
                                tb.ma_quan_tri, tb.ma_cong_ty, ct.ten_cong_ty, cttm.dia_diem, cttm.thoi_gian, tb.ngay_tao, tb.ma_nguoi_tim_viec
                            from thong_bao tb 
                            LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                            LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                            LEFT JOIN chi_tiet_thu_moi cttm on tb.ma_thong_bao = cttm.ma_thong_bao
                            WHERE tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                                OR (tb.loai_thong_bao = 'thu_Moi_Phong_Van' AND tb.ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec)
                            ORDER BY tb.ma_thong_bao ASC";
            }
            if (tb_knd.kieu_nguoi_dung != "nguoi_Tim_Viec")
            {
                sql = @"select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, qt.ho_ten,
                                tb.ma_quan_tri, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao
                            from thong_bao tb 
                            LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                            LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                            WHERE tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                            ORDER BY tb.ma_thong_bao ASC";
            }
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("ma_Nguoi_Tim_Viec", tb_knd.ma_nguoi_tim_viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao = new List<thong_bao>();
            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    loai_thong_bao = reader.IsDBNull(reader.GetOrdinal("loai_thong_bao")) ? LoaiThongBao.None : (LoaiThongBao)Enum.Parse(typeof(LoaiThongBao), reader.GetString("loai_thong_bao")),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                if (danh_sach.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {

                    int ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec");

                    danh_sach.chi_tiet_thu_moi = layChiTietThuMoi(ma_nguoi_tim_viec);

                }

                if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten")
                    };
                }
                if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    };
                }
                danh_sach_thong_bao.Add(danh_sach);
            }
            return danh_sach_thong_bao;
        }

        private static List<chi_tiet_thu_moi> layChiTietThuMoi(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select thoi_gian, dia_diem from chi_tiet_thu_moi where ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", ma_Nguoi_Tim_Viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<chi_tiet_thu_moi>();
            while (reader.Read())
            {
                var chi_tiet = new chi_tiet_thu_moi
                {
                    thoi_gian = reader.IsDBNull(reader.GetOrdinal("thoi_gian")) ? DateTime.MinValue : reader.GetDateTime("thoi_gian"),

                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem")
                };

                danh_sach.Add(chi_tiet);
            }
            return danh_sach;
        }

        public static List<thong_bao> chonThongBaoCoDinh(LoaiThongBao loai_Thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
        SELECT tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, 
               tb.ma_quan_tri, qt.ho_ten, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao, tb.ma_nguoi_tim_viec
        FROM thong_bao tb 
        LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
        LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
        WHERE tb.loai_thong_bao = @loai_Thong_Bao
        ORDER BY tb.ma_thong_bao ASC";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@loai_Thong_Bao", loai_Thong_Bao.ToString());

            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao_co_dinh = new List<thong_bao>();

            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    loai_thong_bao = reader.IsDBNull(reader.GetOrdinal("loai_thong_bao"))
                                     ? LoaiThongBao.None
                                     : (LoaiThongBao)Enum.Parse(typeof(LoaiThongBao), reader.GetString("loai_thong_bao")),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };

                if (danh_sach.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {
                    int ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? 0 : reader.GetInt32("ma_nguoi_tim_viec");
                    danh_sach.chi_tiet_thu_moi = layChiTietThuMoi(ma_nguoi_tim_viec);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten")
                    };
                }

                // Cong ty
                if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                    };
                }

                danh_sach_thong_bao_co_dinh.Add(danh_sach);
            }

            return danh_sach_thong_bao_co_dinh;
        }


        public static bool xoaBaiDang(int ma_Bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();

            try
            {
                string sql_vieclam = "DELETE FROM viec_lam WHERE ma_bai_dang = @ma_Bai_Dang";
                using (var cmd1 = new MySqlCommand(sql_vieclam, coon, trans))
                {
                    cmd1.Parameters.AddWithValue("@ma_Bai_Dang", ma_Bai_Dang);
                    cmd1.ExecuteNonQuery();
                }

                string sql_baidang = "DELETE FROM bai_dang WHERE ma_bai_dang = @ma_Bai_Dang";
                int rowef;
                using (var cmd2 = new MySqlCommand(sql_baidang, coon, trans))
                {
                    cmd2.Parameters.AddWithValue("@ma_Bai_Dang", ma_Bai_Dang);
                    rowef = cmd2.ExecuteNonQuery();
                }

                trans.Commit();
                return rowef > 0;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }

        public static bool guiThongBaoMoi(thong_bao thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "insert into thong_bao (tieu_de, noi_dung, loai_thong_bao, ma_quan_tri, ma_cong_ty, ma_nguoi_nhan, ngay_tao, ngay_cap_nhat) values(@tieu_de, @noi_dung, @loai_thong_bao, @ma_quan_tri, @ma_cong_ty, @ma_nguoi_nhan, @ngay_tao, @ngay_cap_nhat)";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@tieu_de", thong_Bao.tieu_de);
            cmd.Parameters.AddWithValue("@noi_dung", thong_Bao.noi_dung);
            cmd.Parameters.AddWithValue("@loai_thong_bao", thong_Bao.loai_thong_bao);
            cmd.Parameters.AddWithValue("@ma_quan_tri", thong_Bao.ma_quan_tri);
            cmd.Parameters.AddWithValue("@ma_cong_ty", thong_Bao.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", thong_Bao.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@ngay_tao", thong_Bao.ngay_tao);
            cmd.Parameters.AddWithValue("@ngay_cap_nhat", thong_Bao.ngay_cap_nhat);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static List<viec_lam> duaRaDanhSachDeXuat(string chuoi_yeu_cau)
        {
            var chuoiYeuCau = Normalize(chuoi_yeu_cau);

            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "laptrinhvien", "Công nghệ thông tin" },
                { "developer", "Công nghệ thông tin" },
                { "coder", "Công nghệ thông tin" },
                { "tester", "Công nghệ thông tin" },
                { "ke toan", "Tài chính - kế toán" },
                { "accountant", "Tài chính - kế toán" },
                { "giaovien", "Giáo dục - đào tạo" },
                { "teacher", "Giáo dục - đào tạo" }
            };

            string mappedNganh = "";

            if (mapping.TryGetValue(chuoiYeuCau, out string? nganh))
            {
                mappedNganh = nganh;
            }
            else
            {

                using var coon = new MySqlConnection(chuoi_KetNoi);
                coon.Open();

                string sql = "SELECT DISTINCT nganh_nghe FROM viec_lam";
                using var cmd = new MySqlCommand(sql, coon);
                using var reader = cmd.ExecuteReader();

                double bestScore = 0;
                while (reader.Read())
                {
                    string nganh_nghe = reader.GetString("nganh_nghe");
                    double acc = Similarity(chuoiYeuCau, Normalize(nganh_nghe));

                    if (acc > bestScore)
                    {
                        bestScore = acc;
                        mappedNganh = nganh_nghe;
                    }
                }
            }

            var ketQua = new List<viec_lam>();
            using (var coon = new MySqlConnection(chuoi_KetNoi))
            {
                coon.Open();
                string sql = "SELECT * FROM viec_lam WHERE nganh_nghe = @nganh";
                using var cmd = new MySqlCommand(sql, coon);
                cmd.Parameters.AddWithValue("@nganh", mappedNganh);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vl = new viec_lam
                    {
                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                        dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem")
                    };
                    ketQua.Add(vl);
                }
            }

            return ketQua;
        }

        public static string taoDonHang(tao_don_hang tdh)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            string payment_Url = "";
            try
            {
                decimal gia_dich_vu = 0;
                string lay_gia_dich_vu = "SELECT gia FROM dich_vu WHERE ma_dich_vu = @ma_Dich_Vu";
                using (var cmd = new MySqlCommand(lay_gia_dich_vu, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    var kq = cmd.ExecuteScalar();
                    if (kq == null) throw new Exception("Dịch vụ không tồn tại");
                    gia_dich_vu = Convert.ToDecimal(kq);
                }

                int ma_don_hang;
                string them_don_hang = @"
            INSERT INTO don_hang (ma_cong_ty, tong_tien, trang_thai_don_hang, ngay_tao)
            VALUES (@ma_Cong_Ty, @tong_tien, 'cho_Thanh_Toan', NOW());
            SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(them_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Cong_Ty", tdh.ma_cong_ty);
                    cmd.Parameters.AddWithValue("@tong_tien", gia_dich_vu);
                    var res = cmd.ExecuteScalar();
                    if (res == null) throw new Exception("Không lấy được ID đơn hàng");
                    ma_don_hang = Convert.ToInt32(res);
                }

                string tao_chi_tiet_don_hang = @"
            INSERT INTO chi_tiet_don_hang (ma_don_hang, ma_dich_vu, so_luong, don_gia, trang_thai_don_hang)
            VALUES (@ma_Don_Hang, @ma_Dich_Vu, 1, @don_Gia, 'cho_Thanh_Toan')";

                using (var cmd = new MySqlCommand(tao_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@ma_Dich_Vu", tdh.ma_dich_vu);
                    cmd.Parameters.AddWithValue("@don_Gia", gia_dich_vu);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();

                payment_Url = GenerateVNPayUrl(ma_don_hang, gia_dich_vu);
            }
            catch
            {
                try { trans.Rollback(); } catch { }
                throw;
            }
            return payment_Url;
        }

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

        private static string GenerateVNPayUrl(int ma_don_hang, decimal tongTien)
        {
            var vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", "BROPIXNH" },
                { "vnp_Amount", ((long)(tongTien * 100)).ToString() },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", ma_don_hang.ToString() },
                { "vnp_OrderInfo", $"Thanh toán đơn hàng {ma_don_hang}" },
                { "vnp_OrderType", "other" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", "https://1e3b4214677f.ngrok-free.app/api/API_WEB/VNPayReturn" },
                { "vnp_IpAddr", "127.0.0.1" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_SecureHashType", "SHA256" }
            };

            var sortedParams = vnp_Params.OrderBy(k => k.Key);
            var queryString = new StringBuilder();
            var hashData = new StringBuilder();

            foreach (var kv in sortedParams)
            {
                if (queryString.Length > 0)
                {
                    queryString.Append("&");
                    hashData.Append("&");
                }
                queryString.Append($"{kv.Key}={WebUtility.UrlEncode(kv.Value)}");
                hashData.Append($"{kv.Key}={kv.Value}");
            }

            var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("I7LL3FX1ZJQZ6OCXQ9EGY9DVT0W0Q3EE"));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(hashData.ToString()));
            var vnp_SecureHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

            return $"{"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"}?{queryString}&vnp_SecureHash={vnp_SecureHash}";
        }

        public static bool capNhatTrangThaiDonHang(int ma_don_hang, decimal so_tien, string vnpay_res)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string cap_nhat_don_hang = "UPDATE don_hang SET trang_thai_don_hang = 'da_Thanh_Toan' WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.ExecuteNonQuery();
                }

                string cap_nhat_chi_tiet_don_hang = "UPDATE chi_tiet_don_hang SET trang_thai_don_hang = 'da_Thanh_Toan' WHERE ma_don_hang=@ma_Don_Hang";
                using (var cmd = new MySqlCommand(cap_nhat_chi_tiet_don_hang, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.ExecuteNonQuery();
                }

                string them_thanh_toan_moi = @"INSERT INTO thanh_toan (ma_don_hang, so_tien, response_code, ngay_thanh_toan, trang_thai_thanh_toan) " +
                        "VALUES (@ma_Don_Hang, @so_Tien, @res, now(), 'da_Thanh_Toan')";
                using (var cmd = new MySqlCommand(them_thanh_toan_moi, coon, trans))
                {
                    cmd.Parameters.AddWithValue("@ma_Don_Hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@so_Tien", so_tien);
                    cmd.Parameters.AddWithValue("@res", vnpay_res);
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch
            {
                trans.Rollback();
                return false;
            }
        }



        public static List<viec_lam_ket_qua> deXuatViecLamSelector(viec_lam viec_Lam)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from viec_lam";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<viec_lam_ket_qua>();
            while (reader.Read())
            {
                var vl = new viec_lam_ket_qua
                {
                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),
                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),
                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                    kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),
                    loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),
                    diem_phu_hop = 0
                };
                var vl_nganh_nghe = Normalize(vl.nganh_nghe);
                var vl_dia_diem = Normalize(vl.dia_diem);
                var vl_muc_luong = Normalize(vl.muc_luong);
                var vl_kinh_nghiem = Normalize(vl.kinh_nghiem);
                var vl_loai_hinh = Normalize(vl.loai_hinh.ToString());

                var req_nganh_nghe = Normalize(viec_Lam.nganh_nghe);
                var req_dia_diem = Normalize(viec_Lam.dia_diem);
                var req_muc_luong = Normalize(viec_Lam.muc_luong);
                var req_kinh_nghiem = Normalize(viec_Lam.kinh_nghiem);
                var req_loai_hinh = Normalize(viec_Lam.loai_hinh.ToString());

                if (!string.IsNullOrEmpty(req_dia_diem) && vl_dia_diem.Contains(req_dia_diem))
                {
                    vl.diem_phu_hop += 3;
                }

                if (!string.IsNullOrEmpty(req_nganh_nghe) && vl_nganh_nghe.Contains(req_nganh_nghe))
                {
                    vl.diem_phu_hop += 6;
                }

                if (!string.IsNullOrEmpty(req_muc_luong) && vl_muc_luong.Contains(req_muc_luong))
                {
                    vl.diem_phu_hop += 2;
                }

                if (!string.IsNullOrEmpty(req_kinh_nghiem) && vl_kinh_nghiem.Contains(req_kinh_nghiem))
                {
                    vl.diem_phu_hop += 2;
                }

                if (!string.IsNullOrEmpty(req_loai_hinh) && vl_loai_hinh.Contains(req_loai_hinh))
                {
                    vl.diem_phu_hop += 1;
                }

                if (vl.diem_phu_hop > 0)
                    danh_sach.Add(vl);
            }
            return danh_sach.OrderByDescending(j => j.diem_phu_hop).Take(5).ToList();
        }












        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            result = Regex.Replace(result, @"[\s_\-.,/]+", "");

            return result;
        }

        public static double Similarity(string s1, string s2)
        {
            int distance = Levenshtein(s1, s2);
            int maxLen = Math.Max(s1.Length, s2.Length);
            return 1.0 - (double)distance / maxLen;
        }

        public static int Levenshtein(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost
                    );
                }
            }
            return d[n, m];
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