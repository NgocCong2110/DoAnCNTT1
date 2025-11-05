using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using System.Text.Json.Nodes;
using Mysqlx.Crud;
using Newtonsoft.Json.Linq;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_logo;
using Org.BouncyCastle.Tls;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty;
using System.Security.Cryptography.X509Certificates;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_viec_lam_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_anh_bia_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_phuc_loi_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_mang_xa_hoi_cong_ty;
using System.Threading.Tasks;

namespace DotNet_WEB.Module
{
    public class Module_CTY
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static async Task<string> capNhatLogoCongTy(IFormFile file, int ma_cong_ty)
        {
            return await thay_doi_logo_cong_ty.capNhatLogoCongTy(file, ma_cong_ty);
        }

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

        public static List<bai_dang> layBaiDangTheoIDCongTy(int ma_Cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from bai_dang where ma_nguoi_dang = @ma_Cong_Ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach_bai_dang = new List<bai_dang>();

            while (reader.Read())
            {
                var baiDang = new bai_dang
                {
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),

                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                };

                danh_sach_bai_dang.Add(baiDang);
            }
            return danh_sach_bai_dang;
        }

        public static List<thong_ke_ung_vien> laySoLuongUngVien(int ma_Cong_Ty)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string layDanhSach = @" select count(*) as so_luong_ung_vien, month(ngay_ung_tuyen) as thang from ung_tuyen a  where ma_cong_ty = @ma_Cong_Ty group by month(ngay_ung_tuyen) order by thang";

            using var cmd = new MySqlCommand(layDanhSach, conn);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<thong_ke_ung_vien>();

            while (reader.Read())
            {
                var ung_Vien = new thong_ke_ung_vien
                {
                    so_luong_ung_vien = reader.GetInt32("so_luong_ung_vien"),
                    thang = reader.GetInt32("thang")
                };
                danh_sach.Add(ung_Vien);
            }
            return danh_sach;
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

        public static bool capNhatThongTinCongTy(thong_tin_truong_du_lieu_cap_nhat req)
        {
            return chuc_nang_tai_khoan_cong_ty_web.capNhatThongTinCongTy(req);
        }

        public static List<cong_ty> layThongTinCongTy(int ma_cong_ty)
        {
            return chuc_nang_tai_khoan_cong_ty_web.layThongTinCongTy(ma_cong_ty);
        }

        public static List<viec_lam> layDanhSachViecLamCuaCongTy(int ma_cong_ty)
        {
            return chuc_nang_viec_lam_cong_ty_web.layDanhSachViecLamCuaCongTy(ma_cong_ty);
        }

        public static async Task<string> capNhatAnhBiaCongTy(IFormFile file, int ma_cong_ty)
        {
            return await chuc_nang_anh_bia_cong_ty_web.capNhatAnhBiaCongTy(file, ma_cong_ty);
        }

        public static bool capNhatPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            return chuc_nang_phuc_loi_cong_ty_web.capNhatPhucLoiCongTy(phuc_Loi_Cong_Ty);
        }

        public static bool xoaPhucLoiCongTy(phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            return chuc_nang_phuc_loi_cong_ty_web.xoaPhucLoiCongTy(phuc_Loi_Cong_Ty);
        }

        public static bool capNhatLienKetMangXaHoi(mang_xa_hoi_cong_ty_cap_nhat mang_Xa_Hoi_Cong_Ty_Cap_Nhat)
        {
            return chuc_nang_mang_xa_hoi_cong_ty_web.capNhatLienKetMangXaHoi(mang_Xa_Hoi_Cong_Ty_Cap_Nhat);
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

        public static List<dich_vu> layDanhSachDichVu()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"Select * from dich_vu";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<dich_vu>();
            while (reader.Read())
            {
                var dv = new dich_vu
                {
                    ma_dich_vu = reader.IsDBNull(reader.GetOrdinal("ma_dich_vu")) ? 0 : reader.GetInt32("ma_dich_vu"),

                    ten_dich_vu = reader.IsDBNull(reader.GetOrdinal("ten_dich_vu")) ? null : reader.GetString("ten_dich_vu"),

                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                    gia = reader.IsDBNull(reader.GetOrdinal("gia")) ? 0 : reader.GetDecimal("gia"),
                };
                danh_sach.Add(dv);
            }
            return danh_sach;
        }
    }
}




public class thong_ke_ung_vien
{
    public int so_luong_ung_vien { get; set; }
    public int thang { get; set; }
}
