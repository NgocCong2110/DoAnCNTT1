using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_thong_bao
{
    public class chuc_nang_thong_bao_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool guiThongBaoToiServer(thong_bao thongBao)
        {
            var danhSachNguoiNhan = LayDanhSachNguoiNhan(
                LoaiThongBao.toan_Server,
                thongBao.ma_cong_ty,
                thongBao.ma_nguoi_tim_viec
            );

            if (danhSachNguoiNhan.Count == 0)
                return false;

            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            using var transaction = coon.BeginTransaction();

            try
            {
                string sqlThongBao = @"
            INSERT INTO thong_bao (
                tieu_de, noi_dung, loai_thong_bao, 
                ma_quan_tri, ma_cong_ty, ma_nguoi_tim_viec,
                ngay_tao, ngay_cap_nhat
            )
            VALUES (
                @tieu_de, @noi_dung, @loai_thong_bao,
                @ma_quan_tri, @ma_cong_ty, @ma_nguoi_tim_viec,
                NOW(), NOW()
            );
            SELECT LAST_INSERT_ID();
        ";

                int maThongBao;

                using (var cmd = new MySqlCommand(sqlThongBao, coon, transaction))
                {
                    cmd.Parameters.AddWithValue("@tieu_de", thongBao.tieu_de);
                    cmd.Parameters.AddWithValue("@noi_dung", thongBao.noi_dung);
                    cmd.Parameters.AddWithValue("@loai_thong_bao", thongBao.loai_thong_bao.ToString());
                    cmd.Parameters.AddWithValue("@ma_quan_tri", thongBao.ma_quan_tri ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma_cong_ty", thongBao.ma_cong_ty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", thongBao.ma_nguoi_tim_viec ?? (object)DBNull.Value);

                    maThongBao = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string sqlTrangThai = @"
            INSERT INTO trang_thai_thong_bao (
                ma_nguoi_nhan, loai_nguoi_nhan, 
                ma_thong_bao, trang_thai_doc, trang_thai_an, 
                ngay_tao, ngay_cap_nhat
            )
            VALUES (
                @ma_nguoi_nhan, @loai_nguoi_nhan,
                @ma_thong_bao, false, false,
                NOW(), NOW()
            )
        ";

                using var cmdTrangThai = new MySqlCommand(sqlTrangThai, coon, transaction);
                cmdTrangThai.Parameters.Add("@ma_nguoi_nhan", MySqlDbType.Int32);
                cmdTrangThai.Parameters.Add("@loai_nguoi_nhan", MySqlDbType.VarChar);
                cmdTrangThai.Parameters.Add("@ma_thong_bao", MySqlDbType.Int32).Value = maThongBao;

                foreach (var user in danhSachNguoiNhan)
                {
                    cmdTrangThai.Parameters["@ma_nguoi_nhan"].Value = user.maNguoiNhan;
                    cmdTrangThai.Parameters["@loai_nguoi_nhan"].Value = user.loaiNguoiNhan;
                    cmdTrangThai.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public static List<thong_bao> thongBaoQuanTriRieng(int ma_quan_tri)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select tieu_de, noi_dung, ngay_tao from thong_bao where ma_quan_tri = @ma_quan_tri";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_quan_tri", ma_quan_tri);
            using var reader = cmd.ExecuteReader();
            var danhSach = new List<thong_bao>();
            while (reader.Read())
            {
                var thongBao = new thong_bao
                {
                    ma_quan_tri = ma_quan_tri,
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? null : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? null : reader.GetString("noi_dung"),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao")
                };
                danhSach.Add(thongBao);
            }
            return danhSach;
        }


        public static List<(int maNguoiNhan, string loaiNguoiNhan)> LayDanhSachNguoiNhan(LoaiThongBao loaiThongBao, int? maCongTy = null, int? maNguoiTimViec = null)
        {
            var dsNguoiNhan = new List<(int, string)>();
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "";

            switch (loaiThongBao)
            {
                case LoaiThongBao.toan_Server:
                    sql = @"SELECT ma_nguoi_dung as ma, 'nguoi_Dung ' as loai 
                            FROM nguoi_dung
                            UNION
                            SELECT ma_quan_tri as ma, 'quan_Tri' as loai 
                            FROM quan_tri";
                    break;
            }

            using var cmd = new MySqlCommand(sql, coon);

            if (maCongTy.HasValue)
                cmd.Parameters.AddWithValue("@ma_cong_ty", maCongTy.Value);
            if (maNguoiTimViec.HasValue)
                cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", maNguoiTimViec.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dsNguoiNhan.Add((reader.GetInt32("ma"), reader.GetString("loai")));
            }

            return dsNguoiNhan;
        }

        public static bool xoaThongBaoQuanTri(thong_bao thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "delete from thong_bao where ma_thong_bao = @ma_thong_bao and ma_quan_tri = @ma_quan_tri";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_thong_bao", thong_Bao.ma_thong_bao);
            cmd.Parameters.AddWithValue("@ma_quan_tri", thong_Bao.ma_quan_tri);
            return cmd.ExecuteNonQuery() > 0;
        }

    }
}