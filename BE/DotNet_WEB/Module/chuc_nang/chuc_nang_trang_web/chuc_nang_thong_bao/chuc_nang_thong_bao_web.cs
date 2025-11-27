using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_thong_bao
{
    public class chuc_nang_thong_bao_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static List<thong_bao> layDanhSachThongBao(thong_bao_kieu_nguoi_dung tb_knd)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            bool isNguoiTimViec = tb_knd.kieu_nguoi_dung == "nguoi_Tim_Viec";
            bool isCongTy = tb_knd.kieu_nguoi_dung == "cong_Ty";
            bool isQuanTri = tb_knd.kieu_nguoi_dung == "quan_Tri_Vien";

            if (isNguoiTimViec)
            {
                sql = @"
                    SELECT 
                        tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
                        tb.ma_quan_tri, tb.ma_cong_ty, tb.ma_nguoi_tim_viec, tb.ma_bai_dang, 
                        tb.ngay_tao,
                        bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang,
                        qt.ho_ten, ct.ten_cong_ty,
                        cttm.dia_diem, cttm.thoi_gian, tttb.ma_thong_bao, tttb.trang_thai_an
                    FROM thong_bao tb
                    LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                    LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                    LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
                    LEFT JOIN chi_tiet_thu_moi cttm ON tb.ma_thong_bao = cttm.ma_thong_bao
                    LEFT JOIN trang_thai_thong_bao tttb
                        ON tb.ma_thong_bao = tttb.ma_thong_bao
                        AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
                    WHERE 
                    (
                        tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                        OR (tb.loai_thong_bao = 'thu_Moi_Phong_Van' AND tb.ma_nguoi_tim_viec = @ma_nguoi_tim_viec)
                    )
                    AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 0)
                    ORDER BY tb.ma_thong_bao DESC";
            }
            else if (isCongTy)
            {
                sql = @"
                    SELECT 
                        tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
                        tb.ma_quan_tri, tb.ma_cong_ty, tb.ngay_tao,
                        qt.ho_ten, ct.ten_cong_ty,
                        tb.ma_bai_dang, tttb.ma_thong_bao, tttb.trang_thai_an,
                        bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang,
                        cttm.dia_diem, cttm.thoi_gian
                    FROM thong_bao tb
                    LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                    LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                    LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
                    LEFT JOIN chi_tiet_thu_moi cttm ON cttm.ma_thong_bao = tb.ma_thong_bao AND tb.loai_thong_bao = 'thu_Moi_Phong_Van' 
                    LEFT JOIN trang_thai_thong_bao tttb
                        ON tb.ma_thong_bao = tttb.ma_thong_bao
                        AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
                        AND tttb.loai_nguoi_nhan = 'nguoi_Dung'
                    WHERE ((tb.loai_thong_bao = 'viec_Lam_Moi' AND tb.ma_cong_ty = @ma_cong_ty)
                            OR tb.loai_thong_bao != 'viec_Lam_Moi')
                            AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 0)
                            ORDER BY tb.ma_thong_bao DESC";
            }

            else if (isQuanTri)
            {
                sql = @"
                    SELECT 
                        tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
                        tb.ma_quan_tri, tb.ma_cong_ty, tb.ngay_tao,
                        qt.ho_ten, ct.ten_cong_ty,
                        tb.ma_bai_dang, tttb.ma_thong_bao, tttb.trang_thai_an,
                        bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang
                    FROM thong_bao tb
                    LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                    LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                    LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
                    LEFT JOIN trang_thai_thong_bao tttb
                        ON tb.ma_thong_bao = tttb.ma_thong_bao
                        AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
                        AND tttb.loai_nguoi_nhan = 'quan_Tri'
                    WHERE 
                        tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                        AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 0) AND tb.loai_thong_bao != 'viec_Lam_Moi'
                    ORDER BY tb.ma_thong_bao DESC";
            }
            else
            {
                Console.WriteLine("LOI: kieu_nguoi_dung KHONG HOP LE: " + tb_knd.kieu_nguoi_dung);
                throw new Exception("kieu_nguoi_dung không hợp lệ, không thể tạo SQL");
            }

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", tb_knd.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", tb_knd.ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@ma_cong_ty", tb_knd.ma_cong_ty);
            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao = new List<thong_bao>();

            while (reader.Read())
            {
                var tb = new thong_bao
                {
                    ma_thong_bao = reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    loai_thong_bao = Enum.TryParse<LoaiThongBao>(
                        reader.IsDBNull(reader.GetOrdinal("loai_thong_bao")) ? "None" : reader.GetString("loai_thong_bao"),
                        out var loai) ? loai : LoaiThongBao.None,
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_tao"),
                    trang_Thai_Thong_Bao = new trang_thai_thong_bao
                    {
                        ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),

                        trang_thai_an = reader.IsDBNull(reader.GetOrdinal("trang_thai_an")) ? false : reader.GetBoolean("trang_thai_an"),
                    }
                };

                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                    {
                        tb.ma_quan_tri = reader.GetInt32("ma_quan_tri");
                        tb.quan_Tri = new quan_tri { ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten") };
                    }
                }
                catch { }

                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                    {
                        tb.ma_cong_ty = reader.GetInt32("ma_cong_ty");
                        tb.cong_Ty = new cong_ty { ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty") };
                    }
                }
                catch { }

                try
                {
                    var idxMaBaiDang = reader.GetOrdinal("ma_bai_dang");
                    if (!reader.IsDBNull(idxMaBaiDang))
                    {
                        tb.ma_bai_dang = reader.GetInt32(idxMaBaiDang);
                        var idxTieuDeBD = reader.GetOrdinal("tieu_de_bai_dang");
                        if (!reader.IsDBNull(idxTieuDeBD))
                        {
                            tb.bai_Dang = new bai_dang
                            {
                                tieu_de = reader.GetString(idxTieuDeBD),
                                ngay_tao = reader.GetDateTime("ngay_tao_bai_dang")
                            };
                        }
                    }
                }
                catch { }

                if (tb.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {
                    try
                    {
                        var idxDiaDiem = reader.GetOrdinal("dia_diem");
                        if (!reader.IsDBNull(idxDiaDiem))
                        {
                            tb.chi_tiet_thu_moi = new List<chi_tiet_thu_moi>
                            {
                                new chi_tiet_thu_moi
                                {
                                    dia_diem = reader.GetString("dia_diem"),
                                    thoi_gian = reader.GetDateTime("thoi_gian")
                                }
                            };
                        }
                    }
                    catch { }
                }

                if (isNguoiTimViec)
                {
                    try { tb.ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? (int?)null : reader.GetInt32("ma_nguoi_tim_viec"); }
                    catch { }
                }

                danh_sach_thong_bao.Add(tb);
            }

            return danh_sach_thong_bao;
        }

        public static List<thong_bao> layDanhSachThongBaoDaAn(thong_bao_kieu_nguoi_dung tb_knd)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            bool isNguoiTimViec = tb_knd.kieu_nguoi_dung == "nguoi_Tim_Viec";
            bool isCongTy = tb_knd.kieu_nguoi_dung == "cong_Ty";
            bool isQuanTri = tb_knd.kieu_nguoi_dung == "quan_Tri_Vien";
            if (isNguoiTimViec)
            {
                sql = @"
                    SELECT 
                        tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
                        tb.ma_quan_tri, tb.ma_cong_ty, tb.ma_nguoi_tim_viec, tb.ma_bai_dang, 
                        tb.ngay_tao,
                        bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang,
                        qt.ho_ten, ct.ten_cong_ty, tttb.ma_thong_bao, tttb.trang_thai_an,
                        cttm.dia_diem, cttm.thoi_gian
                    FROM thong_bao tb
                    LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                    LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                    LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
                    LEFT JOIN chi_tiet_thu_moi cttm ON tb.ma_thong_bao = cttm.ma_thong_bao
                    LEFT JOIN trang_thai_thong_bao tttb
                        ON tb.ma_thong_bao = tttb.ma_thong_bao
                        AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
                    WHERE 
                    (
                        tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                        OR (tb.loai_thong_bao = 'thu_Moi_Phong_Van' AND tb.ma_nguoi_tim_viec = @ma_nguoi_tim_viec)
                        AND tttb.loai_nguoi_nhan = 'nguoi_Dung'
                    )
                    AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 1)
                    ORDER BY tb.ma_thong_bao DESC";
            }
            if (isCongTy)
            {
                sql = @"
                    SELECT 
            tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
            tb.ma_quan_tri, tb.ma_cong_ty, tb.ngay_tao,
            qt.ho_ten, ct.ten_cong_ty,
            tb.ma_bai_dang, tttb.ma_thong_bao, tttb.trang_thai_an,
            bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang
        FROM thong_bao tb
        LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
        LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
        LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
        LEFT JOIN trang_thai_thong_bao tttb
            ON tb.ma_thong_bao = tttb.ma_thong_bao
            AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
            AND tttb.loai_nguoi_nhan = 'nguoi_Dung'
        WHERE tb.loai_thong_bao != 'thu_Moi_Phong_Van'
            AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 1)
            AND tb.loai_thong_bao != 'viec_Lam_Moi'
        ORDER BY tb.ma_thong_bao DESC";
            }

            if (isQuanTri)
            {
                sql = @"
                    SELECT 
                        tb.ma_thong_bao, tb.tieu_de, tb.noi_dung, tb.loai_thong_bao,
                        tb.ma_quan_tri, tb.ma_cong_ty, tb.ngay_tao,
                        qt.ho_ten, ct.ten_cong_ty,
                        tb.ma_bai_dang, tttb.ma_thong_bao, tttb.trang_thai_an,
                        bd.tieu_de AS tieu_de_bai_dang, bd.ngay_tao AS ngay_tao_bai_dang
                    FROM thong_bao tb
                    LEFT JOIN quan_tri qt ON tb.ma_quan_tri = qt.ma_quan_tri
                    LEFT JOIN cong_ty ct ON tb.ma_cong_ty = ct.ma_cong_ty
                    LEFT JOIN bai_dang bd ON tb.ma_bai_dang = bd.ma_bai_dang
                    LEFT JOIN trang_thai_thong_bao tttb
                        ON tb.ma_thong_bao = tttb.ma_thong_bao
                        AND tttb.ma_nguoi_nhan = @ma_nguoi_nhan
                        AND tttb.loai_nguoi_nhan = 'quan_Tri'
                    WHERE 
                        tb.loai_thong_bao != 'thu_Moi_Phong_Van'
                        AND (tttb.trang_thai_an IS NULL OR tttb.trang_thai_an = 1) AND tb.loai_thong_bao != 'viec_Lam_Moi'
                    ORDER BY tb.ma_thong_bao DESC";
            }

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", tb_knd.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", tb_knd.ma_nguoi_tim_viec);

            using var reader = cmd.ExecuteReader();
            var danh_sach_thong_bao = new List<thong_bao>();

            while (reader.Read())
            {
                var tb = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    loai_thong_bao = Enum.TryParse<LoaiThongBao>(
                        reader.IsDBNull(reader.GetOrdinal("loai_thong_bao")) ? "None" : reader.GetString("loai_thong_bao"),
                        out var loai) ? loai : LoaiThongBao.None,
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_tao"),
                    trang_Thai_Thong_Bao = new trang_thai_thong_bao
                    {
                        ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),

                        trang_thai_an = reader.IsDBNull(reader.GetOrdinal("trang_thai_an")) ? false : reader.GetBoolean("trang_thai_an"),
                    }
                };

                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ma_quan_tri")))
                    {
                        tb.ma_quan_tri = reader.GetInt32("ma_quan_tri");
                        tb.quan_Tri = new quan_tri { ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten") };
                    }
                }
                catch { }

                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")))
                    {
                        tb.ma_cong_ty = reader.GetInt32("ma_cong_ty");
                        tb.cong_Ty = new cong_ty { ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty") };
                    }
                }
                catch { }

                try
                {
                    var idxMaBaiDang = reader.GetOrdinal("ma_bai_dang");
                    if (!reader.IsDBNull(idxMaBaiDang))
                    {
                        tb.ma_bai_dang = reader.GetInt32(idxMaBaiDang);
                        var idxTieuDeBD = reader.GetOrdinal("tieu_de_bai_dang");
                        if (!reader.IsDBNull(idxTieuDeBD))
                        {
                            tb.bai_Dang = new bai_dang
                            {
                                tieu_de = reader.GetString(idxTieuDeBD),
                                ngay_tao = reader.GetDateTime("ngay_tao_bai_dang")
                            };
                        }
                    }
                }
                catch { }

                if (tb.loai_thong_bao == LoaiThongBao.thu_Moi_Phong_Van)
                {
                    try
                    {
                        var idxDiaDiem = reader.GetOrdinal("dia_diem");
                        if (!reader.IsDBNull(idxDiaDiem))
                        {
                            tb.chi_tiet_thu_moi = new List<chi_tiet_thu_moi>
                            {
                                new chi_tiet_thu_moi
                                {
                                    dia_diem = reader.GetString("dia_diem"),
                                    thoi_gian = reader.GetDateTime("thoi_gian")
                                }
                            };
                        }
                    }
                    catch { }
                }

                if (isNguoiTimViec)
                {
                    try { tb.ma_nguoi_tim_viec = reader.IsDBNull(reader.GetOrdinal("ma_nguoi_tim_viec")) ? (int?)null : reader.GetInt32("ma_nguoi_tim_viec"); }
                    catch { }
                }

                danh_sach_thong_bao.Add(tb);
            }

            return danh_sach_thong_bao;
        }

        public static bool anThongBao(trang_thai_thong_bao trang_Thai_Thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            string loai_nguoi_nhan = "";
            if (trang_Thai_Thong_Bao.loai_nguoi_nhan == "nguoi_Tim_Viec" || trang_Thai_Thong_Bao.loai_nguoi_nhan == "cong_Ty")
                loai_nguoi_nhan = "nguoi_Dung";
            else if (trang_Thai_Thong_Bao.loai_nguoi_nhan == "quan_Tri_Vien")
                loai_nguoi_nhan = "quan_Tri";
            else
                return false;
            sql = @"UPDATE trang_thai_thong_bao 
                   SET trang_thai_an = @trang_thai_an 
                   WHERE ma_thong_bao = @ma_thong_bao 
                   AND ma_nguoi_nhan = @ma_nguoi_nhan 
                   AND loai_nguoi_nhan = @loai_nguoi_nhan";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@trang_thai_an", 1);
            cmd.Parameters.AddWithValue("@ma_thong_bao", trang_Thai_Thong_Bao.ma_thong_bao);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", trang_Thai_Thong_Bao.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@loai_nguoi_nhan", loai_nguoi_nhan);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public static bool boAnThongBao(trang_thai_thong_bao trang_Thai_Thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "";
            string loai_nguoi_nhan = "";
            if (trang_Thai_Thong_Bao.loai_nguoi_nhan == "nguoi_Tim_Viec" || trang_Thai_Thong_Bao.loai_nguoi_nhan == "cong_Ty")
                loai_nguoi_nhan = "nguoi_Dung";
            else if (trang_Thai_Thong_Bao.loai_nguoi_nhan == "quan_Tri_Vien")
                loai_nguoi_nhan = "quan_Tri";
            else
                return false;

            sql = @"UPDATE trang_thai_thong_bao 
                   SET trang_thai_an = @trang_thai_an 
                   WHERE ma_thong_bao = @ma_thong_bao 
                   AND ma_nguoi_nhan = @ma_nguoi_nhan 
                   AND loai_nguoi_nhan = @loai_nguoi_nhan";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@trang_thai_an", 0);
            cmd.Parameters.AddWithValue("@ma_thong_bao", trang_Thai_Thong_Bao.ma_thong_bao);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", trang_Thai_Thong_Bao.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@loai_nguoi_nhan", loai_nguoi_nhan);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }
}