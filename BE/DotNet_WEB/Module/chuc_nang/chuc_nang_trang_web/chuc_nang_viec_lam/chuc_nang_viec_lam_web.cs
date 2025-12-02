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
        JOIN bai_dang bd ON vl.ma_bai_dang = bd.ma_bai_dang
        WHERE vl.ma_bai_dang = @ma_bai_dang and bd.trang_thai = 'cong_Khai'
        LIMIT 1";

            using var cmdViec = new MySqlCommand(sqlViec, conn);
            cmdViec.Parameters.AddWithValue("@ma_bai_dang", ma_bai_dang);

            using var reader = cmdViec.ExecuteReader();

            viec_lam? viec_Lam = null;

            if (reader.Read())
            {
                viec_Lam = new viec_lam
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

            if (viec_Lam == null)
            {
                return null;
            }

            if (viec_Lam.ma_cong_ty > 0)
            {
                viec_Lam.danh_sach_phuc_loi = new List<phuc_loi>();

                string sqlPhucLoi = @"
            SELECT pl.ma_phuc_loi, pl.ten_phuc_loi
            FROM phuc_loi pl
            JOIN phuc_loi_viec_lam plvl ON plvl.ma_phuc_loi = pl.ma_phuc_loi
            WHERE plvl.ma_viec = @ma_viec";

                using var cmdPhuc = new MySqlCommand(sqlPhucLoi, conn);
                cmdPhuc.Parameters.AddWithValue("@ma_viec", viec_Lam.ma_viec);

                using var readerPL = cmdPhuc.ExecuteReader();
                while (readerPL.Read())
                {
                    viec_Lam.danh_sach_phuc_loi.Add(new phuc_loi
                    {
                        ma_phuc_loi = readerPL.GetInt32("ma_phuc_loi"),
                        ten_phuc_loi = readerPL.GetString("ten_phuc_loi")
                    });
                }
                readerPL.Close();
            }

            if (viec_Lam.ma_cong_ty > 0)
            {
                viec_Lam.tinh_Thanh = new List<tinh_thanh>();

                string sqlTinhThanh = @"
            SELECT tt.ma_tinh, tt.ten_tinh
            FROM tinh_thanh tt
            JOIN viec_lam_tinh_thanh vltt ON vltt.ma_tinh = tt.ma_tinh
            WHERE vltt.ma_viec = @ma_viec";

                using var cmdTinh = new MySqlCommand(sqlTinhThanh, conn);
                cmdTinh.Parameters.AddWithValue("@ma_viec", viec_Lam.ma_viec);

                using var readerTT = cmdTinh.ExecuteReader();
                while (readerTT.Read())
                {
                    viec_Lam.tinh_Thanh.Add(new tinh_thanh
                    {
                        ma_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ma_tinh")) ? 0 : readerTT.GetInt32("ma_tinh"),
                        ten_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ten_tinh")) ? null : readerTT.GetString("ten_tinh")
                    });
                }
                readerTT.Close();
            }

            return viec_Lam;
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
                        WHERE bd.trang_thai = 'cong_Khai'
                        group by vl.ma_viec, bd.ma_bai_dang, vl.nganh_nghe, ct.ten_cong_ty, vl.tieu_de, vl.mo_ta, vl.muc_luong, vl.dia_diem, ct.logo 
                        order by so_luong desc
                        limit 16";
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
            catch (Exception err)
            {
                Console.WriteLine(err);
                return new List<so_luong_ung_vien_viec_lam>();
            }
        }

        public static List<so_luong_nganh_nghe> layNganhNgheNoiBat()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select vl.nganh_nghe, count(*) as so_luong 
            from viec_lam  vl
            JOIN bai_dang bd ON vl.ma_bai_dang = bd.ma_bai_dang
            WHERE bd.trang_thai = 'cong_Khai'
            group by vl.nganh_nghe order by so_luong desc limit 14";
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

        public static List<nganh_nghe> layDanhSachNganhNghe()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select * from nganh_nghe order by thu_tu";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach = new List<nganh_nghe>();
            while (reader.Read())
            {
                var sl = new nganh_nghe
                {
                    ma_nganh_nghe = reader.IsDBNull(reader.GetOrdinal("ma_nganh_nghe")) ? null : reader.GetString("ma_nganh_nghe"),
                    ten_nganh_nghe = reader.IsDBNull(reader.GetOrdinal("ten_nganh_nghe")) ? null : reader.GetString("ten_nganh_nghe"),
                    thu_tu = reader.IsDBNull(reader.GetOrdinal("thu_tu")) ? 0 : reader.GetInt32("thu_tu"),
                };
                danh_sach.Add(sl);
            }
            return danh_sach;
        }

        public static bool themNganhNgheMoi(nganh_nghe nganh_Nghe)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"insert into nganh_nghe values(@ma_nganh_nghe, @ten_nganh_nghe, @thu_tu)";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nganh_nghe", nganh_Nghe.ma_nganh_nghe);
            cmd.Parameters.AddWithValue("@ten_nganh_nghe", nganh_Nghe.ten_nganh_nghe);
            cmd.Parameters.AddWithValue("@thu_tu", nganh_Nghe.thu_tu);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static bool xoaNganhNghe(nganh_nghe nganh_Nghe)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"delete from nganh_nghe where ma_nganh_nghe = @ma_nganh_nghe";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nganh_nghe", nganh_Nghe.ma_nganh_nghe);
            return cmd.ExecuteNonQuery() > 0;
        }

        public static List<viec_lam_ket_qua> duaRaDanhSachDeXuat(de_xuat_tuong_ung de_Xuat_Tuong_Ung)
        {
            var chuoiYeuCau = Normalize(de_Xuat_Tuong_Ung.tu_khoa);

            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "laptrinhvien", "cong_nghe_thong_tin" },
                { "laptrinh", "cong_nghe_thong_tin" },
                { "developer", "cong_nghe_thong_tin" },
                { "dev", "cong_nghe_thong_tin" },
                { "coder", "cong_nghe_thong_tin" },
                { "it", "cong_nghe_thong_tin" },
                { "itsupport", "cong_nghe_thong_tin" },
                { "helpdesk", "cong_nghe_thong_tin" },
                { "tester", "cong_nghe_thong_tin" },
                { "qa", "cong_nghe_thong_tin" },
                { "qc", "cong_nghe_thong_tin" },
                { "softwareengineer", "cong_nghe_thong_tin" },
                { "frontend", "cong_nghe_thong_tin" },
                { "backend", "cong_nghe_thong_tin" },
                { "fullstack", "cong_nghe_thong_tin" },
                { "data", "cong_nghe_thong_tin" },
                { "dataanalyst", "cong_nghe_thong_tin" },
                { "ai", "cong_nghe_thong_tin" },
                { "machinelearning", "cong_nghe_thong_tin" },
                { "cybersecurity", "cong_nghe_thong_tin" },

                { "ketoan", "tai_chinh" },
                { "accountant", "tai_chinh" },
                { "accounting", "tai_chinh" },
                { "finance", "tai_chinh" },
                { "financial", "tai_chinh" },
                { "nganhang", "tai_chinh" },
                { "banking", "tai_chinh" },
                { "thungan", "tai_chinh" },
                { "thuquy", "tai_chinh" },

                { "giaovien", "giao_duc" },
                { "giangvien", "giao_duc" },
                { "teacher", "giao_duc" },
                { "giasu", "giao_duc" },
                { "daotao", "giao_duc" },
                { "training", "giao_duc" },

                { "sales", "sales" },
                { "sale", "sales" },
                { "banhang", "sales" },
                { "telesales", "sales" },
                { "kinhdoanh", "sales" },
                { "nhanvienkinhdoanh", "sales" },
                { "salesman", "sales" },

                { "marketing", "marketing" },
                { "marketer", "marketing" },
                { "content", "marketing" },
                { "contentcreator", "marketing" },
                { "seo", "marketing" },
                { "sem", "marketing" },
                { "facebookads", "marketing" },
                { "googleads", "marketing" },
                { "chayquangcao", "marketing" },

                { "hanhchinh", "hanh_chinh" },
                { "vanphong", "hanh_chinh" },
                { "admin", "hanh_chinh" },
                { "adminoffice", "hanh_chinh" },
                { "letan", "hanh_chinh" },
                { "receptionist", "hanh_chinh" },

                { "hr", "nhan_su" },
                { "humanresource", "nhan_su" },
                { "tuyendung", "nhan_su" },
                { "recruiter", "nhan_su" },

                { "designer", "thiet_ke" },
                { "graphic", "thiet_ke" },
                { "graphicdesign", "thiet_ke" },
                { "uiux", "thiet_ke" },
                { "uiuxdesigner", "thiet_ke" },
                { "uxdesigner", "thiet_ke" },
                { "uidesigner", "thiet_ke" },

                { "xaydung", "xay_dung" },
                { "kysuxaydung", "xay_dung" },
                { "construction", "xay_dung" },
                { "civilengineer", "xay_dung" },

                { "bacsi", "y_te" },
                { "doctor", "y_te" },
                { "dieuduong", "y_te" },
                { "nurse", "y_te" },
                { "duoc", "y_te" },
                { "duocsi", "y_te" },
                { "pharmacist", "y_te" },
                { "batdongsan", "bat_dong_san" },
                { "bdst", "bat_dong_san" },
                { "nhadat", "bat_dong_san" },
                { "moi gioi", "bat_dong_san" },
                { "moigioi", "bat_dong_san" },
                { "salesbatdongsan", "bat_dong_san" },

                { "chamsockhachhang", "cham_soc_khach_hang" },
                { "cskh", "cham_soc_khach_hang" },
                { "callcenter", "cham_soc_khach_hang" },
                { "support", "cham_soc_khach_hang" },

                { "cokhi", "co_khi_dien_dien_tu" },
                { "dien", "co_khi_dien_dien_tu" },
                { "dienlanh", "co_khi_dien_dien_tu" },
                { "diencongnghiep", "co_khi_dien_dien_tu" },
                { "kythuatdien", "co_khi_dien_dien_tu" },
                { "baotri", "co_khi_dien_dien_tu" },

                { "congtacxahoi", "cong_tac_xa_hoi" },
                { "philoinhuan", "cong_tac_xa_hoi" },
                { "nongovernment", "cong_tac_xa_hoi" },
                { "ngoc", "cong_tac_xa_hoi" },

                { "dulich", "du_lich" },
                { "nhahang", "du_lich" },
                { "khachsan", "du_lich" },
                { "phucvu", "du_lich" },
                { "phucvuong", "du_lich" },
                { "phache", "du_lich" },
                { "buongphong", "du_lich" },
                { "housekeeping", "du_lich" },
                { "hotel", "du_lich" },
                { "restaurant", "du_lich" },

                { "luat", "luat" },
                { "law", "luat" },
                { "lawyer", "luat" },
                { "tuvanluat", "luat" },

                { "nongnghiep", "nong_lam_ngu_nghiep" },
                { "nonglamngu", "nong_lam_ngu_nghiep" },
                { "lamnghiep", "nong_lam_ngu_nghiep" },
                { "nungnghiep", "nong_lam_ngu_nghiep" },
                { "channuoi", "nong_lam_ngu_nghiep" },
                { "thuyhai", "nong_lam_ngu_nghiep" },

                { "sanxuat", "san_xuat" },
                { "congnghiep", "san_xuat" },
                { "congnhan", "san_xuat" },
                { "vanhanhmay", "san_xuat" },
                { "kcs", "san_xuat" },
                { "kho", "san_xuat" },
                { "quanlychuyeng", "san_xuat" },

                { "truyenthong", "truyen_thong" },
                { "quangcao", "truyen_thong" },
                { "pr", "truyen_thong" },
                { "publicrelation", "truyen_thong" },
                { "copywriter", "truyen_thong" },
                { "truyenthongbaochi", "truyen_thong" },

                { "vantai", "van_tai" },
                { "giaohang", "van_tai" },
                { "shipper", "van_tai" },
                { "logistics", "van_tai" },
                { "khovan", "van_tai" },
                { "taixe", "van_tai" },
                { "tx", "van_tai" }
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

            var ketQua = new List<viec_lam_ket_qua>();
            bool loc_theo_tinh = de_Xuat_Tuong_Ung.ma_tinh > 0;
            bool loc_theo_nghe = !string.IsNullOrEmpty(chuoiYeuCau) && !string.IsNullOrEmpty(mappedNganh);
            using (var coon = new MySqlConnection(chuoi_KetNoi))
            {
                coon.Open();
                string sql = @"
        SELECT vl.*, ct.logo, ct.ten_cong_ty 
        FROM viec_lam vl 
        JOIN cong_ty ct ON vl.ma_cong_ty = ct.ma_cong_ty
        JOIN bai_dang bd ON vl.ma_bai_dang = bd.ma_bai_dang";

                if (loc_theo_tinh)
                {
                    sql += " JOIN viec_lam_tinh_thanh vltt ON vl.ma_viec = vltt.ma_viec ";
                }

                List<string> dieuKien = new List<string>();

                dieuKien.Add("bd.trang_thai = 'cong_Khai'");

                if (loc_theo_nghe)
                {
                    dieuKien.Add("vl.nganh_nghe = @nganh");
                }

                if (loc_theo_tinh)
                {
                    dieuKien.Add("vltt.ma_tinh = @ma_tinh");
                }

                if (dieuKien.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", dieuKien);
                }

                using var cmd = new MySqlCommand(sql, coon);

                if (loc_theo_nghe)
                {
                    cmd.Parameters.AddWithValue("@nganh", mappedNganh);
                }

                if (loc_theo_tinh)
                {
                    cmd.Parameters.AddWithValue("@ma_tinh", de_Xuat_Tuong_Ung.ma_tinh);
                }

                using var reader = cmd.ExecuteReader();
                List<viec_lam_ket_qua> listKetQuaTam = new List<viec_lam_ket_qua>();

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
                    listKetQuaTam.Add(vl);
                }
                reader.Close();

                string sqlTinhThanh = @"
        SELECT tt.ma_tinh, tt.ten_tinh
        FROM tinh_thanh tt
        JOIN viec_lam_tinh_thanh vltt ON vltt.ma_tinh = tt.ma_tinh
        WHERE vltt.ma_viec = @ma_viec";

                foreach (var jobResult in listKetQuaTam)
                {
                    int maViec = jobResult.ma_viec;
                    if (maViec > 0 && jobResult.viec_Lam != null)
                    {
                        jobResult.viec_Lam.tinh_Thanh = new List<tinh_thanh>();

                        using var cmdTinh = new MySqlCommand(sqlTinhThanh, coon);
                        cmdTinh.Parameters.AddWithValue("@ma_viec", maViec);

                        using var readerTT = cmdTinh.ExecuteReader();
                        while (readerTT.Read())
                        {
                            jobResult.viec_Lam.tinh_Thanh.Add(new tinh_thanh
                            {
                                ma_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ma_tinh")) ? 0 : readerTT.GetInt32("ma_tinh"),
                                ten_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ten_tinh")) ? null : readerTT.GetString("ten_tinh")
                            });
                        }
                        readerTT.Close();
                    }
                }

                ketQua.AddRange(listKetQuaTam);
            }

            return ketQua;
        }

        public static List<viec_lam> layDanhSachViecLamLienQuan(bai_dang bai_Dang)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select vl.*, ct.ten_cong_ty, ct.logo 
            from viec_lam vl 
                join cong_ty ct on vl.ma_cong_ty = ct.ma_cong_ty 
                join bai_dang bd on bd.ma_bai_dang = vl.ma_bai_dang 
            where bd.ma_bai_dang <> @ma_bai_dang and ct.ma_cong_ty = @ma_cong_ty and bd.trang_thai = 'cong_Khai'";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_bai_dang", bai_Dang.ma_bai_dang);
            cmd.Parameters.AddWithValue("@ma_cong_ty", bai_Dang.ma_nguoi_dang);
            var danh_sach = new List<viec_lam>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var viec_Lam = new viec_lam
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
                danh_sach.Add(viec_Lam);
            }
            return danh_sach;
        }

        public static List<viec_lam_ket_qua> deXuatViecLamSelector(viec_lam viec_Lam)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"SELECT v.*, c.logo, c.ten_cong_ty 
                   FROM viec_lam v
                   INNER JOIN cong_ty c ON v.ma_cong_ty = c.ma_cong_ty
                   INNER JOIN bai_dang bd ON v.ma_bai_dang = bd.ma_bai_dang
                   WHERE bd.trang_thai = 'cong_Khai'";

            using var cmd = new MySqlCommand(sql, coon);

            var danh_sach = new List<viec_lam_ket_qua>();

            using (var reader = cmd.ExecuteReader())
            {
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

                    var req_nganh_nghe = string.IsNullOrWhiteSpace(viec_Lam.nganh_nghe) ? null : Normalize(viec_Lam.nganh_nghe);
                    var req_vi_tri = string.IsNullOrWhiteSpace(viec_Lam.vi_tri) ? null : Normalize(viec_Lam.vi_tri);
                    var req_dia_diem = string.IsNullOrWhiteSpace(viec_Lam.dia_diem) ? null : Normalize(viec_Lam.dia_diem);
                    var req_muc_luong = string.IsNullOrWhiteSpace(viec_Lam.muc_luong) ? null : Normalize(viec_Lam.muc_luong);
                    var req_kinh_nghiem = string.IsNullOrWhiteSpace(viec_Lam.kinh_nghiem) ? null : Normalize(viec_Lam.kinh_nghiem);
                    var req_loai_hinh = string.IsNullOrWhiteSpace(viec_Lam.loai_hinh.ToString()) ? null : Normalize(viec_Lam.loai_hinh.ToString());

                    if (!string.IsNullOrEmpty(req_dia_diem) && !string.IsNullOrEmpty(vl_dia_diem) && vl_dia_diem.Contains(req_dia_diem))
                    {
                        vl.diem_phu_hop += 4;
                    }

                    if (!string.IsNullOrEmpty(req_nganh_nghe) && !string.IsNullOrEmpty(vl_nganh_nghe) && vl_nganh_nghe == req_nganh_nghe)
                    {
                        vl.diem_phu_hop += 6;
                    }

                    if (!string.IsNullOrEmpty(req_vi_tri) && !string.IsNullOrEmpty(vl_vi_tri) && vl_vi_tri.Contains(req_vi_tri))
                    {
                        vl.diem_phu_hop += 2;
                    }
                    if (!string.IsNullOrEmpty(req_muc_luong))
                    {
                        decimal reqMin = 0;
                        decimal reqMax = 0;
                        bool thoa_thuan = false;
                        switch (req_muc_luong)
                        {
                            case "10":
                                reqMin = 0; reqMax = 10; break;
                            case "11":
                                reqMin = 10; reqMax = 20; break;
                            case "21":
                                reqMin = 20; reqMax = 30; break;
                            case "30":
                                reqMin = 30; reqMax = decimal.MaxValue; break;
                            case "thoathuan":
                                thoa_thuan = true; break;
                        }
                        bool coGiaoNhau = false;
                        decimal? luongThap = vl.viec_Lam.muc_luong_thap_nhat;
                        decimal? luongCao = vl.viec_Lam.muc_luong_cao_nhat;
                        if (thoa_thuan)
                        {
                            if (!luongThap.HasValue && !luongCao.HasValue)
                                coGiaoNhau = true;
                        }
                        else if (luongThap.HasValue && luongCao.HasValue)
                        {
                            coGiaoNhau = !(luongCao < reqMin || luongThap > reqMax);
                        }
                        if (coGiaoNhau)
                            vl.diem_phu_hop += 3;
                    }
                    if (!string.IsNullOrEmpty(req_kinh_nghiem) && !string.IsNullOrEmpty(vl_kinh_nghiem) && vl_kinh_nghiem.Contains(req_kinh_nghiem))
                    {
                        vl.diem_phu_hop += 2;
                    }
                    if (!string.IsNullOrEmpty(req_loai_hinh) && !string.IsNullOrEmpty(vl_loai_hinh) && vl_loai_hinh.Contains(req_loai_hinh))
                    {
                        vl.diem_phu_hop += 2;
                    }

                    if (vl.diem_phu_hop > 1)
                        danh_sach.Add(vl);
                }
            }

            string sqlTinhThanh = @"
        SELECT tt.ma_tinh, tt.ten_tinh
        FROM tinh_thanh tt
        JOIN viec_lam_tinh_thanh vltt ON vltt.ma_tinh = tt.ma_tinh
        WHERE vltt.ma_viec = @ma_viec";

            foreach (var jobResult in danh_sach)
            {
                int maViec = jobResult.ma_viec;
                if (maViec > 0 && jobResult.viec_Lam != null)
                {
                    jobResult.viec_Lam.tinh_Thanh = new List<tinh_thanh>();

                    using var cmdTinh = new MySqlCommand(sqlTinhThanh, coon);
                    cmdTinh.Parameters.AddWithValue("@ma_viec", maViec);

                    using var readerTT = cmdTinh.ExecuteReader();
                    while (readerTT.Read())
                    {
                        jobResult.viec_Lam.tinh_Thanh.Add(new tinh_thanh
                        {
                            ma_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ma_tinh")) ? 0 : readerTT.GetInt32("ma_tinh"),
                            ten_tinh = readerTT.IsDBNull(readerTT.GetOrdinal("ten_tinh")) ? null : readerTT.GetString("ten_tinh")
                        });
                    }
                    readerTT.Close();
                }
            }

            return danh_sach.OrderByDescending(j => j.diem_phu_hop).ToList();
        }


        public static string Normalize(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            result = Regex.Replace(result, @"[\s_\-.,/]+", "");

            return string.IsNullOrWhiteSpace(result) ? "" : result;
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

    public class de_xuat_tuong_ung
    {
        public string? tu_khoa { get; set; }
        public int ma_tinh { get; set; }
    }
}