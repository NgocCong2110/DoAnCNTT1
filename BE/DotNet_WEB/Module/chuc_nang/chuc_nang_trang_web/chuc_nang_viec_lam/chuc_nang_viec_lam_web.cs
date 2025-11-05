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

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_viec_lam
{
    public class chuc_nang_viec_lam_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static viec_lam? layViecLamTheoBaiDang(int ma_bai_dang)
        {
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();

            string sqlViec = @"
        SELECT vl.*, ct.logo, ct.ten_cong_ty
        FROM viec_lam vl
        JOIN cong_ty ct ON vl.ma_cong_ty = ct.ma_cong_ty
        WHERE vl.ma_bai_dang = @ma_bai_dang
        LIMIT 1";

            using var cmdViec = new MySqlCommand(sqlViec, conn);
            cmdViec.Parameters.AddWithValue("@ma_bai_dang", ma_bai_dang);

            using var reader = cmdViec.ExecuteReader();

            viec_lam? viec = null;

            if (reader.Read())
            {
                viec = new viec_lam
                {
                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                    cong_Ty = new cong_ty
                    {
                        ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                        logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                    },
                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                    vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),
                    kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                    yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),
                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                    muc_luong_thap_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_thap_nhat")) ? null : reader.GetDecimal("muc_luong_thap_nhat"),
                    muc_luong_cao_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_cao_nhat")) ? null : reader.GetDecimal("muc_luong_cao_nhat"),
                    quyen_loi_cong_viec = reader.IsDBNull(reader.GetOrdinal("quyen_loi_cong_viec")) ? null : reader.GetString("quyen_loi_cong_viec"),
                    trinh_do_hoc_van_yeu_cau = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van_yeu_cau"))
                        ? TrinhDoHocVan.khong_Yeu_Cau
                        : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van_yeu_cau")),
                    thoi_gian_lam_viec = reader.IsDBNull(reader.GetOrdinal("thoi_gian_lam_viec")) ? null : reader.GetString("thoi_gian_lam_viec"),
                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),
                    thoi_han_nop_cv = reader.IsDBNull(reader.GetOrdinal("thoi_han_nop_cv")) ? DateTime.MinValue : reader.GetDateTime("thoi_han_nop_cv"),
                    loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh"))
                        ? LoaiHinhViecLam.None
                        : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                };
            }

            reader.Close();

            if (viec == null)
            {
                return null;
            }

            if (viec.ma_cong_ty > 0)
            {
                viec.danh_sach_phuc_loi = new List<phuc_loi>();

                string sqlPhucLoi = @"
            SELECT pl.ma_phuc_loi, pl.ten_phuc_loi
            FROM phuc_loi pl
            JOIN phuc_loi_viec_lam plvl ON plvl.ma_phuc_loi = pl.ma_phuc_loi
            WHERE plvl.ma_viec = @ma_viec";

                using var cmdPhuc = new MySqlCommand(sqlPhucLoi, conn);
                cmdPhuc.Parameters.AddWithValue("@ma_viec", viec.ma_viec);

                using var readerPL = cmdPhuc.ExecuteReader();
                while (readerPL.Read())
                {
                    viec.danh_sach_phuc_loi.Add(new phuc_loi
                    {
                        ma_phuc_loi = readerPL.GetInt32("ma_phuc_loi"),
                        ten_phuc_loi = readerPL.GetString("ten_phuc_loi")
                    });
                }
                readerPL.Close();
            }

            return viec;
        }
        public static List<so_luong_ung_vien_viec_lam> layDanhSachViecLamDuocQuanTam()
        {
            try
            {
                using var coon = new MySqlConnection(chuoi_KetNoi);
                coon.Open();
                string sql = @"select vl.ma_viec, bd.ma_bai_dang, vl.nganh_nghe, ct.ten_cong_ty, vl.tieu_de , vl.mo_ta, vl.muc_luong, vl.dia_diem, ct.logo, count(*) as so_luong 
                            from ung_tuyen ut
                            join viec_lam vl on ut.ma_viec = vl.ma_viec
                            join bai_dang bd on vl.ma_bai_dang = bd.ma_bai_dang
	                        join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty
                        group by vl.ma_viec, bd.ma_bai_dang, vl.nganh_nghe, ct.ten_cong_ty, vl.tieu_de, vl.mo_ta, vl.muc_luong, vl.dia_diem, ct.logo";
                using var cmd = new MySqlCommand(sql, coon);
                using var reader = cmd.ExecuteReader();
                var danh_sach = new List<so_luong_ung_vien_viec_lam>();
                while (reader.Read())
                {
                    var vl = new viec_lam
                    {
                        ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                        bai_dang = new bai_dang
                        {
                            ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang")
                        },

                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                        cong_Ty = new cong_ty
                        {
                            ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),

                            logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                        },

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                        dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),
                    };

                    var uv_vl = new so_luong_ung_vien_viec_lam
                    {
                        viec_Lam = vl,

                        so_luong = reader.IsDBNull(reader.GetOrdinal("so_luong")) ? 0 : reader.GetInt32("so_luong")
                    };

                    danh_sach.Add(uv_vl);
                }
                return danh_sach;
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
                return new List<so_luong_ung_vien_viec_lam>();
            }
        }

        public static List<so_luong_nganh_nghe> layNganhNgheNoiBat()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select nganh_nghe, count(*) as so_luong from viec_lam 
            group by nganh_nghe order by so_luong desc";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<so_luong_nganh_nghe>();
            while (reader.Read())
            {
                var sl = new so_luong_nganh_nghe
                {
                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                    so_luong = reader.IsDBNull(reader.GetOrdinal("so_luong")) ? 0 : reader.GetInt32("so_luong")
                };
                danh_sach.Add(sl);
            }
            return danh_sach;
        }

        public static List<viec_lam> duaRaDanhSachDeXuat(string chuoi_yeu_cau)
        {
            var chuoiYeuCau = Normalize(chuoi_yeu_cau);

            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "laptrinhvien", "cong_nghe_thong_tin" },
                { "developer", "cong_nghe_thong_tin" },
                { "coder", "cong_nghe_thong_tin" },
                { "tester", "cong_nghe_thong_tin" },
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
                string sql = @"SELECT vl.*, ct.logo, ct.ten_cong_ty FROM viec_lam vl join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty
                 WHERE nganh_nghe = @nganh ";
                using var cmd = new MySqlCommand(sql, coon);
                cmd.Parameters.AddWithValue("@nganh", mappedNganh);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var vl = new viec_lam
                    {
                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),

                        cong_Ty = new cong_ty
                        {
                            logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                            ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                        },

                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),

                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                        yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),

                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                        muc_luong_cao_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_cao_nhat")) ? null : reader.GetDecimal("muc_luong_cao_nhat"),

                        muc_luong_thap_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_thap_nhat")) ? null : reader.GetDecimal("muc_luong_thap_nhat"),

                        quyen_loi_cong_viec = reader.IsDBNull(reader.GetOrdinal("quyen_loi_cong_viec")) ? null : reader.GetString("quyen_loi_cong_viec"),

                        trinh_do_hoc_van_yeu_cau = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van_yeu_cau")) ? TrinhDoHocVan.khong_Yeu_Cau : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van_yeu_cau")),

                        thoi_gian_lam_viec = reader.IsDBNull(reader.GetOrdinal("thoi_gian_lam_viec")) ? null : reader.GetString("thoi_gian_lam_viec"),

                        dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),

                        thoi_han_nop_cv = reader.IsDBNull(reader.GetOrdinal("thoi_han_nop_cv")) ? DateTime.MinValue : reader.GetDateTime("thoi_han_nop_cv"),

                        loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),

                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                        ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                    };
                    ketQua.Add(vl);
                }
            }

            return ketQua;
        }

        public static List<viec_lam> layDanhSachViecLamLienQuan(bai_dang bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select vl.*, ct.ten_cong_ty, ct.logo from viec_lam vl join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty join bai_dang bd on bd.ma_bai_dang = vl.ma_bai_dang where bd.ma_bai_dang <> @ma_bai_dang and ct.ma_cong_ty = @ma_cong_ty";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang.ma_bai_dang);
            cmd.Parameters.AddWithValue("@ma_cong_ty", bai_Dang.ma_nguoi_dang);
            var danh_sach = new List<viec_lam>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vl = new viec_lam
                {
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                    bai_dang = new bai_dang
                    {
                        ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang")
                    },

                    cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),

                        logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo")
                    }
                };
                danh_sach.Add(vl);
            }
            return danh_sach;
        }

        public static List<viec_lam_ket_qua> deXuatViecLamSelector(viec_lam viec_Lam)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"SELECT v.*, c.logo, c.ten_cong_ty 
               FROM viec_lam v
               INNER JOIN cong_ty c ON v.ma_cong_ty = c.ma_cong_ty";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<viec_lam_ket_qua>();
            while (reader.Read())
            {
                var vl = new viec_lam_ket_qua
                {
                    ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),
                    viec_Lam = new viec_lam
                    {
                        ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),

                        cong_Ty = new cong_ty
                        {
                            logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),

                            ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty")
                        },

                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),

                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),

                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),

                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),

                        mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),

                        yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau"),

                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),

                        muc_luong_cao_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_cao_nhat")) ? null : reader.GetDecimal("muc_luong_cao_nhat"),

                        muc_luong_thap_nhat = reader.IsDBNull(reader.GetOrdinal("muc_luong_thap_nhat")) ? null : reader.GetDecimal("muc_luong_thap_nhat"),

                        trinh_do_hoc_van_yeu_cau = reader.IsDBNull(reader.GetOrdinal("trinh_do_hoc_van_yeu_cau")) ? TrinhDoHocVan.khong_Yeu_Cau : (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van_yeu_cau")),

                        thoi_gian_lam_viec = reader.IsDBNull(reader.GetOrdinal("thoi_gian_lam_viec")) ? null : reader.GetString("thoi_gian_lam_viec"),

                        dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),

                        thoi_han_nop_cv = reader.IsDBNull(reader.GetOrdinal("thoi_han_nop_cv")) ? DateTime.MinValue : reader.GetDateTime("thoi_han_nop_cv"),

                        loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),

                        ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),

                        ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat")
                    },
                    nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                    vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),
                    dia_diem = reader.IsDBNull(reader.GetOrdinal("dia_diem")) ? null : reader.GetString("dia_diem"),
                    muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                    kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),
                    loai_hinh = reader.IsDBNull(reader.GetOrdinal("loai_hinh")) ? LoaiHinhViecLam.None : (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? 0 : reader.GetInt32("ma_bai_dang"),
                    diem_phu_hop = 0
                };
                var vl_nganh_nghe = Normalize(vl.nganh_nghe);
                var vl_vi_tri = Normalize(vl.vi_tri);
                var vl_dia_diem = Normalize(vl.dia_diem);
                var vl_muc_luong = Normalize(vl.muc_luong);
                var vl_kinh_nghiem = Normalize(vl.kinh_nghiem);
                var vl_loai_hinh = Normalize(vl.loai_hinh.ToString());

                var req_vi_tri = Normalize(viec_Lam.vi_tri);
                var req_nganh_nghe = Normalize(viec_Lam.nganh_nghe);
                var req_dia_diem = Normalize(viec_Lam.dia_diem);
                var req_muc_luong = Normalize(viec_Lam.muc_luong);
                var req_kinh_nghiem = Normalize(viec_Lam.kinh_nghiem);
                var req_loai_hinh = Normalize(viec_Lam.loai_hinh.ToString());

                if (!string.IsNullOrEmpty(req_dia_diem) && !string.IsNullOrEmpty(vl_dia_diem) && vl_dia_diem.Contains(req_dia_diem))
                {
                    vl.diem_phu_hop += 4;
                }

                if (!string.IsNullOrEmpty(req_nganh_nghe) && !string.IsNullOrEmpty(vl_nganh_nghe) && vl_nganh_nghe.Contains(req_nganh_nghe))
                {
                    vl.diem_phu_hop += 6;
                }

                if (!string.IsNullOrEmpty(req_vi_tri) && !string.IsNullOrEmpty(vl_vi_tri) && vl_vi_tri.Contains(req_vi_tri))
                {
                    vl.diem_phu_hop += 2;
                }

                if (!string.IsNullOrEmpty(req_muc_luong) && !string.IsNullOrEmpty(vl_muc_luong) && vl_muc_luong.Contains(req_muc_luong))
                {
                    vl.diem_phu_hop += 3;
                }

                if (!string.IsNullOrEmpty(req_kinh_nghiem) && !string.IsNullOrEmpty(vl_kinh_nghiem) && vl_kinh_nghiem.Contains(req_kinh_nghiem))
                {
                    vl.diem_phu_hop += 2;
                }

                if (!string.IsNullOrEmpty(req_loai_hinh) && !string.IsNullOrEmpty(vl_loai_hinh) && vl_loai_hinh.Contains(req_loai_hinh))
                {
                    vl.diem_phu_hop += 1;
                }

                if (vl.diem_phu_hop > 1)
                    danh_sach.Add(vl);
            }
            return danh_sach.OrderByDescending(j => j.diem_phu_hop).ToList();
        }


        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            result = Regex.Replace(result, @"[\s_\-.,/]+", "");

            return string.IsNullOrWhiteSpace(result) ? null : result;
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

    public class so_luong_ung_vien_viec_lam
    {
        public viec_lam? viec_Lam { get; set; }
        public int so_luong { get; set; }
    }
}