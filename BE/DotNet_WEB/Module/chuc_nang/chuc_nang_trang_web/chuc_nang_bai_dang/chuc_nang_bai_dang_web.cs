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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_bai_dang
{
    public class chuc_nang_bai_dang_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
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


        public static List<bai_dang> layDanhSachBaiDang()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
        SELECT 
    bd.ma_bai_dang,
    bd.ma_nguoi_dang,
    bd.ten_nguoi_dang,
    bd.tieu_de,
    bd.noi_dung,
    bd.loai_bai,
    bd.trang_thai,
    bd.ngay_tao,
    bd.ngay_cap_nhat,
    ct.ma_cong_ty,
    ct.logo
FROM bai_dang bd
LEFT JOIN cong_ty ct ON bd.ma_nguoi_dang = ct.ma_cong_ty
ORDER BY bd.ngay_tao DESC;
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

                    cong_Ty = new cong_ty
                    {
                        logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                    },

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

        public static bool huyLuuBaiDang(bai_dang_da_luu bai_Dang_Da_Luu)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "delete from bai_dang_da_luu where ma_bai_dang = @ma_bai_dang and ma_nguoi_luu = @ma_nguoi_luu";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang_Da_Luu.ma_bai_dang);
            cmd.Parameters.AddWithValue("@ma_nguoi_luu", bai_Dang_Da_Luu.ma_nguoi_luu);
            return cmd.ExecuteNonQuery() > 0;
        }


        public static bool themBaiDangMoi(bai_dang bai_Dang, viec_lam viec_Lam, List<int> phuc_loi)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string sql_Bai_Dang = @"INSERT INTO bai_dang(ma_nguoi_dang, ten_nguoi_dang, tieu_de, noi_dung, loai_bai, trang_thai, ngay_tao, ngay_cap_nhat) 
                              VALUES(@ma_nguoi_dang, @ten_nguoi_dang, @tieu_de, @noi_dung, @loai_bai, @trang_thai, @ngay_tao, @ngay_cap_nhat)";
                using var cmdBaiDang = new MySqlCommand(sql_Bai_Dang, coon, trans);
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
                    string sql_Viec_Lam = @"INSERT INTO viec_lam(ma_cong_ty, nganh_nghe, vi_tri, kinh_nghiem, tieu_de, mo_ta, yeu_cau, muc_luong, muc_luong_thap_nhat, muc_luong_cao_nhat, quyen_loi_cong_viec, trinh_do_hoc_van_yeu_cau, thoi_gian_lam_viec, thoi_han_nop_cv, dia_diem, loai_hinh,  ma_bai_dang) 
                                  VALUES(@ma_cong_ty, @nganh_nghe, @vi_tri, @kinh_nghiem, @tieu_de, @mo_ta, @yeu_cau, @muc_luong, @muc_luong_thap_nhat, @muc_luong_cao_nhat, @quyen_loi_cong_viec, @trinh_do_hoc_van_yeu_cau, @thoi_gian_lam_viec, @thoi_han_nop_cv, @dia_diem, @loai_hinh, @ma_bai_dang)";
                    using var cmdViecLam = new MySqlCommand(sql_Viec_Lam, coon, trans);
                    cmdViecLam.Parameters.AddWithValue("@ma_cong_ty", viec_Lam.ma_cong_ty);
                    cmdViecLam.Parameters.AddWithValue("@nganh_nghe", viec_Lam.nganh_nghe);
                    cmdViecLam.Parameters.AddWithValue("@vi_tri", viec_Lam.vi_tri);
                    cmdViecLam.Parameters.AddWithValue("@kinh_nghiem", viec_Lam.kinh_nghiem);
                    cmdViecLam.Parameters.AddWithValue("@tieu_de", viec_Lam.tieu_de);
                    cmdViecLam.Parameters.AddWithValue("@mo_ta", viec_Lam.mo_ta);
                    cmdViecLam.Parameters.AddWithValue("@yeu_cau", viec_Lam.yeu_cau);
                    cmdViecLam.Parameters.AddWithValue("@muc_luong", viec_Lam.muc_luong);
                    cmdViecLam.Parameters.AddWithValue("@muc_luong_thap_nhat", viec_Lam.muc_luong_thap_nhat);
                    cmdViecLam.Parameters.AddWithValue("@muc_luong_cao_nhat", viec_Lam.muc_luong_cao_nhat);
                    cmdViecLam.Parameters.AddWithValue("@quyen_loi_cong_viec", viec_Lam.quyen_loi_cong_viec);
                    cmdViecLam.Parameters.AddWithValue("@trinh_do_hoc_van_yeu_cau", viec_Lam.trinh_do_hoc_van_yeu_cau.ToString());
                    cmdViecLam.Parameters.AddWithValue("@thoi_gian_lam_viec", viec_Lam.thoi_gian_lam_viec);
                    cmdViecLam.Parameters.AddWithValue("@thoi_han_nop_cv", viec_Lam.thoi_han_nop_cv);
                    cmdViecLam.Parameters.AddWithValue("@dia_diem", viec_Lam.dia_diem);
                    cmdViecLam.Parameters.AddWithValue("@loai_hinh", viec_Lam.loai_hinh.ToString());
                    cmdViecLam.Parameters.AddWithValue("@ma_bai_dang", maBaiDang);
                    cmdViecLam.ExecuteNonQuery();
                    long maViecLam = cmdViecLam.LastInsertedId;
                    if(phuc_loi != null && phuc_loi.Any())
                    {
                        string sql_Phuc_Loi = "insert into phuc_loi_viec_lam(ma_viec, ma_phuc_loi) values(@ma_viec, @ma_phuc_loi)";
                        foreach(var ma_phuc_loi in phuc_loi)
                        {
                            using var cmdPhucLoi = new MySqlCommand(sql_Phuc_Loi, coon, trans);
                            cmdPhucLoi.Parameters.AddWithValue("@ma_viec", maViecLam);
                            cmdPhucLoi.Parameters.AddWithValue("@ma_phuc_loi", ma_phuc_loi);
                            cmdPhucLoi.ExecuteNonQuery();
                        }
                    }
                }

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi thêm bài đăng mới." + ex.Message);
                Console.WriteLine("Chi tiết lỗi: " + ex.StackTrace);
                trans.Rollback();
                return false;
            }
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
                string lay_danh_sach_bai_dang_da_luu = @"select b.ma_bai_dang, b.ma_nguoi_dang, b.ten_nguoi_dang, c.logo,
                                                        b.tieu_de, b.noi_dung, b.ngay_tao, b.ngay_cap_nhat
                                                        from bai_dang_da_luu l 
                                                        inner join bai_dang b on l.ma_bai_dang = b.ma_bai_dang 
                                                        INNER JOIN cong_ty c ON b.ma_nguoi_dang = c.ma_cong_ty
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

                        cong_Ty = new cong_ty
                        {
                            logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                        },

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),

                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                        ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                    };
                    danh_sach_da_luu.Add(bai_Dang);
                }
                return danh_sach_da_luu;
            }

        public static bool luuBaiDangViPham(bai_dang_vi_pham bai_Dang_Vi_Pham)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string luu_bai_vi_pham = "insert into bai_dang_vi_pham (ma_bai_dang, ten_nguoi_dang, tieu_de, noi_dung, ma_nguoi_bao_cao, loai_vi_pham, noi_dung_bao_cao, trang_thai_xu_ly, ngay_bao_cao) values(@ma_bai_dang, @ten_nguoi_dang, @tieu_de, @noi_dung, @ma_nguoi_bao_cao, @loai_vi_pham, @noi_dung_bao_cao, @trang_thai_xu_ly, @ngay_bao_cao)";
            using var cmd = new MySqlCommand(luu_bai_vi_pham, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang_Vi_Pham.ma_bai_dang);
            cmd.Parameters.AddWithValue("@ten_nguoi_dang", bai_Dang_Vi_Pham.ten_nguoi_dang);
            cmd.Parameters.AddWithValue("@tieu_de", bai_Dang_Vi_Pham.tieu_de);
            cmd.Parameters.AddWithValue("@noi_dung", bai_Dang_Vi_Pham.noi_dung);
            cmd.Parameters.AddWithValue("@ma_nguoi_bao_cao", bai_Dang_Vi_Pham.ma_nguoi_bao_cao);
            cmd.Parameters.AddWithValue("@loai_vi_pham", bai_Dang_Vi_Pham.loai_vi_pham);
            cmd.Parameters.AddWithValue("@noi_dung_bao_cao", bai_Dang_Vi_Pham.noi_dung_bao_cao);
            cmd.Parameters.AddWithValue("@trang_thai_xu_ly", bai_Dang_Vi_Pham.trang_thai_xu_ly.ToString());
            cmd.Parameters.AddWithValue("@ngay_bao_cao", bai_Dang_Vi_Pham.ngay_bao_cao);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }
    }
}