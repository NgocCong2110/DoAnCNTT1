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

namespace DotNet_WEB.Module
{
    public class ChucNang_WEB
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
            catch
            {
                transaction.Rollback();
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
                    loai_nguoi_dung = (LoaiNguoiDung)Enum.Parse(typeof(LoaiNguoiDung), reader.GetString("loai_nguoi_dung")),
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? null : reader.GetInt32("ma_cong_ty"),
                    ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? null : reader.GetInt32("ma_nguoi_tim_viec"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap")
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
                    email = reader.GetString("email"),
                    vai_tro = (VaiTro)Enum.Parse(typeof(VaiTro), reader.GetString("vai_tro"))
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
                    ma_cong_ty = reader.GetInt32("ma_cong_ty"),
                    ten_dn_cong_ty = reader.GetString("ten_dn_cong_ty"),
                    ten_cong_ty = reader.GetString("ten_cong_ty"),
                    nguoi_dai_dien = reader.GetString("nguoi_dai_dien"),
                    ma_so_thue = reader.GetString("ma_so_thue"),
                    dia_chi = reader.GetString("dia_chi"),
                    dien_thoai = reader.GetString("dien_thoai"),
                    email = reader.GetString("email"),
                    website = reader.GetString("website"),
                    logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString("logo"),
                    mo_ta = reader.IsDBNull(reader.GetOrdinal("mo_ta")) ? null : reader.GetString("mo_ta"),
                    loai_hinh_cong_ty = reader.IsDBNull(reader.GetOrdinal("loai_hinh_cong_ty"))
                    ? LoaiHinhCongTy.None
                    : (LoaiHinhCongTy)Enum.Parse(typeof(LoaiHinhCongTy), reader.GetString("loai_hinh_cong_ty")),
                    quy_mo = reader.GetString("quy_mo"),
                    nam_thanh_lap = reader.IsDBNull(reader.GetOrdinal("nam_thanh_lap"))
                    ? null
                    : (int?)reader.GetInt16("nam_thanh_lap"),
                    anh_bia = reader.IsDBNull(reader.GetOrdinal("anh_bia")) ? null : reader.GetString("anh_bia"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai"))
                    ? TrangThaiCongTy.None
                    : (TrangThaiCongTy)Enum.Parse(typeof(TrangThaiCongTy), reader.GetString("trang_thai")),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
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
                    ma_nguoi_tim_viec = reader.GetInt32("ma_nguoi_tim_viec"),
                    ho_ten = reader.GetString("ho_ten"),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    email = reader.GetString("email"),
                    dien_thoai = reader.GetString("dien_thoai"),
                    mat_khau = reader.GetString("mat_khau"),
                    ngay_sinh = reader.GetDateTime("ngay_sinh"),
                    gioi_tinh = (GioiTinh)Enum.Parse(typeof(GioiTinh), reader.GetString("gioi_tinh")),
                    trinh_do_hoc_van = (TrinhDoHocVan)Enum.Parse(typeof(TrinhDoHocVan), reader.GetString("trinh_do_hoc_van")),
                    dia_chi = reader.GetString("dia_chi"),
                    anh_dai_dien = reader.GetString("anh_dai_dien"),
                    quoc_tich = reader.GetString("quoc_tich"),
                    mo_ta = reader.GetString("mo_ta"),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }


        // public static List<nguoi_dung> layThongTinNguoiDung(string email)
        // {
        //     using var conn = new MySqlConnection(chuoi_KetNoi);
        //     conn.Open();
        //     string lay_thong_tin = "select * from nguoi_dung where email = @email";
        //     using var cmd = new MySqlCommand(lay_thong_tin, conn);
        //     cmd.Parameters.AddWithValue("@email", email);
        //     using var reader = cmd.ExecuteReader();
        //     var danh_sach_nguoi_dung = new List<nguoi_dung>();
        //     while (reader.Read())
        //     {
        //         var nguoiDung = new nguoi_dung
        //         {
        //             loai_nguoi_dung = Enum.Parse<LoaiNguoiDung>(reader.GetString("loai_nguoi_dung")),
        //             email = reader.GetString("email"),
        //             ten_dang_nhap = reader.GetString("ten_dang_nhap"),
        //         };
        //         danh_sach_nguoi_dung.Add(nguoiDung);
        //     }
        //     return danh_sach_nguoi_dung;
        // }

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
            luot_thich,
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
                    ma_bai_dang = reader.GetInt32("ma_bai_dang"),
                    ma_nguoi_dang = reader.GetInt32("ma_nguoi_dang"),
                    ten_nguoi_dang = reader.GetString("ten_nguoi_dang"),
                    tieu_de = reader.GetString("tieu_de"),
                    noi_dung = reader.GetString("noi_dung"),
                    loai_bai = (LoaiBai)Enum.Parse(typeof(LoaiBai), reader.GetString("loai_bai")),
                    trang_thai = (TrangThaiBai)Enum.Parse(typeof(TrangThaiBai), reader.GetString("trang_thai")),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat"),

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
                    ma_viec = reader.GetInt32("ma_viec"),
                    ma_cong_ty = reader.GetInt32("ma_cong_ty"),
                    nganh_nghe = reader.GetString("nganh_nghe"),
                    vi_tri = reader.GetString("vi_tri"),
                    kinh_nghiem = reader.GetString("kinh_nghiem"),
                    tieu_de = reader.GetString("tieu_de"),
                    mo_ta = reader.GetString("mo_ta"),
                    yeu_cau = reader.GetString("yeu_cau"),
                    muc_luong = reader.GetString("muc_luong"),
                    dia_diem = reader.GetString("dia_diem"),
                    loai_hinh = (LoaiHinhViecLam)Enum.Parse(typeof(LoaiHinhViecLam), reader.GetString("loai_hinh")),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };
            }
            return null;
        }


        public static bool themBaiDangMoi(bai_dang bai_Dang, viec_lam viec_Lam)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            using var trans = coon.BeginTransaction();
            try
            {
                string sqlBaiDang = @"INSERT INTO bai_dang(ma_nguoi_dang, ten_nguoi_dang, tieu_de, noi_dung, luot_thich, loai_bai, trang_thai, ngay_tao, ngay_cap_nhat) 
                              VALUES(@ma_nguoi_dang, @ten_nguoi_dang, @tieu_de, @noi_dung, @luot_thich, @loai_bai, @trang_thai, @ngay_tao, @ngay_cap_nhat)";
                using var cmdBaiDang = new MySqlCommand(sqlBaiDang, coon, trans);
                cmdBaiDang.Parameters.AddWithValue("@ma_nguoi_dang", bai_Dang.ma_nguoi_dang);
                cmdBaiDang.Parameters.AddWithValue("@ten_nguoi_dang", bai_Dang.ten_nguoi_dang);
                cmdBaiDang.Parameters.AddWithValue("@tieu_de", bai_Dang.tieu_de);
                cmdBaiDang.Parameters.AddWithValue("@noi_dung", bai_Dang.noi_dung);
                cmdBaiDang.Parameters.AddWithValue("@luot_thich", bai_Dang.luot_thich);
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
                    ma_bai_dang = reader.GetInt32("ma_bai_dang"),
                    ma_nguoi_dang = reader.GetInt32("ma_nguoi_dang"),
                    ten_nguoi_dang = reader["ten_nguoi_dang"]?.ToString(),
                    tieu_de = reader["tieu_de"]?.ToString(),
                    noi_dung = reader["noi_dung"]?.ToString(),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };
                danh_sach_da_luu.Add(bai_Dang);
            }
            return danh_sach_da_luu;
        }

        public static bool ungTuyenCongViec(int ma_Viec, int ma_Cong_Ty, int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string ung_tuyen = "INSERT INTO ung_tuyen (ma_viec, ma_cong_ty, ma_nguoi_tim_viec) VALUES (@ma_Viec, @ma_Cong_Ty, @ma_Nguoi_Tim_Viec)";
            using var cmd = new MySqlCommand(ung_tuyen, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ma_Viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_Nguoi_Tim_Viec);
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static List<thong_bao> layDanhSachThongBao()
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, qt.ho_ten,
                                tb.ma_quan_tri, tb.ma_cong_ty, ct.ten_cong_ty, tb.ngay_tao
                            from thong_bao tb 
                            LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                            LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                            ORDER BY tb.ma_thong_bao ASC";
            using var cmd = new MySqlCommand(sql, coon);
            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao = new List<thong_bao>();
            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.GetString("tieu_de"),
                    noi_dung = reader.GetString("noi_dung"),
                    loai_thong_bao = (LoaiThongBao)Enum.Parse(typeof(LoaiThongBao), reader.GetString("loai_thong_bao")),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                };
                if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.GetString("ho_ten")
                    };
                }
                if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.GetString("ten_cong_ty")
                    };
                }
                danh_sach_thong_bao.Add(danh_sach);
            }
            return danh_sach_thong_bao;
        }

        public static List<thong_bao> chonThongBaoCoDinh(LoaiThongBao loai_Thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
        select tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao, 
               qt.ho_ten, ct.ten_cong_ty, tb.ngay_tao
        from thong_bao tb 
        left join quan_tri qt on tb.ma_quan_tri = qt.ma_quan_tri
        left join cong_Ty ct on tb.ma_cong_ty = ct.ma_cong_ty 
        where tb.loai_thong_bao = @loai_Thong_Bao";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@loai_Thong_Bao", loai_Thong_Bao);

            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao_co_dinh = new List<thong_bao>();

            while (reader.Read())
            {
                var danh_sach = new thong_bao
                {
                    ma_thong_bao = reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.GetString("tieu_de"),
                    noi_dung = reader.GetString("noi_dung"),
                    ngay_tao = reader.GetDateTime("ngay_tao")
                };

                if (!reader.IsDBNull(reader.GetOrdinal("ho_ten")))
                {
                    danh_sach.quan_Tri = new quan_tri
                    {
                        ho_ten = reader.GetString("ho_ten")
                    };
                }

                if (!reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")))
                {
                    danh_sach.cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.GetString("ten_cong_ty")
                    };
                }

                danh_sach_thong_bao_co_dinh.Add(danh_sach);
            }

            return danh_sach_thong_bao_co_dinh;
        }
    }
}