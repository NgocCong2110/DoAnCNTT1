using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module
{
    public class Module_CTY
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static List<ung_tuyen> layDanhSachUngVien(int ma_Cong_Ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = @"
SELECT u.ma_ung_tuyen, u.ma_viec, u.ma_nguoi_tim_viec, u.ma_cong_ty,
       u.ngay_ung_tuyen, u.trang_thai,
       v.ma_viec, v.ma_cong_ty, v.vi_tri, v.kinh_nghiem, v.tieu_de,
       v.mo_ta, v.yeu_cau, v.muc_luong, v.dia_diem, v.loai_hinh,
       v.ngay_tao, v.ngay_cap_nhat, v.ma_bai_dang,
       n.ho_ten, n.email
FROM ung_tuyen u
INNER JOIN viec_lam v ON u.ma_viec = v.ma_viec
INNER JOIN nguoi_tim_viec n ON u.ma_nguoi_tim_viec = n.ma_nguoi_tim_viec
WHERE u.ma_cong_ty = @ma_Cong_Ty;";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_Cong_Ty);

            using var reader = cmd.ExecuteReader();
            var danh_sach_ung_tuyen = new List<ung_tuyen>();

            while (reader.Read())
            {
                var ntv = new nguoi_tim_viec
                {
                    ma_nguoi_tim_viec = reader.GetInt32("ma_nguoi_tim_viec"),
                    ho_ten = reader.IsDBNull(reader.GetOrdinal("ho_ten")) ? null : reader.GetString("ho_ten"),
                    email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                };

                var ungTuyen = new ung_tuyen
                {
                    ma_ung_tuyen = reader.GetInt32("ma_ung_tuyen"),
                    ma_viec = reader.GetInt32("ma_viec"),
                    ma_nguoi_tim_viec = reader.GetInt32("ma_nguoi_tim_viec"),
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty"))
                                ? null : reader.GetInt32("ma_cong_ty"),
                    trang_thai = Enum.Parse<TrangThaiUngTuyen>(reader.GetString("trang_thai")),
                    ngay_ung_tuyen = reader.GetDateTime("ngay_ung_tuyen"),
                    viec_Lam = new viec_lam
                    {
                        ma_viec = reader.GetInt32("ma_viec"),
                        ma_cong_ty = reader.GetInt32("ma_cong_ty"),
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
                    ma_bai_dang = reader.GetInt32("ma_bai_dang"),
                    ten_nguoi_dang = reader.IsDBNull(reader.GetOrdinal("ten_nguoi_dang")) ? null : reader.GetString("ten_nguoi_dang"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
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
    }
}
public class thong_ke_ung_vien
{
    public int so_luong_ung_vien { get; set; }
    public int thang { get; set; }
}
