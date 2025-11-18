using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using System.Text.Json.Nodes;
using Mysqlx.Crud;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_thong_bao_cong_ty
{
    public class chuc_nang_thong_bao_cong_ty_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool guiThongBaoViecLamMoi(thong_bao thong_Bao)
        {


            var danhSachNguoiNhan = LayDanhSachNguoiNhan(LoaiThongBao.viec_Lam_Moi, thong_Bao.ma_cong_ty, thong_Bao.ma_nguoi_tim_viec);

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
            ma_quan_tri, ma_cong_ty, ma_nguoi_tim_viec, ma_bai_dang,
            ngay_tao, ngay_cap_nhat
        )
        VALUES (
            @tieu_de, @noi_dung, @loai_thong_bao,
            @ma_quan_tri, @ma_cong_ty, @ma_nguoi_tim_viec, @ma_bai_dang,
            NOW(), NOW()
        );
        SELECT LAST_INSERT_ID();
    ";

                int maThongBao;

                using (var cmd = new MySqlCommand(sqlThongBao, coon, transaction))
                {
                    cmd.Parameters.AddWithValue("@tieu_de", thong_Bao.tieu_de);
                    cmd.Parameters.AddWithValue("@noi_dung", thong_Bao.noi_dung);
                    cmd.Parameters.AddWithValue("@loai_thong_bao", thong_Bao.loai_thong_bao.ToString());
                    cmd.Parameters.AddWithValue("@ma_quan_tri", thong_Bao.ma_quan_tri ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma_cong_ty", thong_Bao.ma_cong_ty ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", thong_Bao.ma_nguoi_tim_viec ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ma_bai_dang", thong_Bao.ma_bai_dang ?? (object)DBNull.Value);

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
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static List<(int maNguoiNhan, string loaiNguoiNhan)> LayDanhSachNguoiNhan(LoaiThongBao loaiThongBao, int? maCongTy = null, int? maNguoiTimViec = null)
        {

            var dsNguoiNhan =
            new List<(int, string)>();
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "";

            switch (loaiThongBao)
            {

                case LoaiThongBao.viec_Lam_Moi:
                    sql = @"
                SELECT ma_nguoi_dung as ma, 'nguoi_Dung' as loai FROM nguoi_dung WHERE loai_nguoi_dung='nguoi_Tim_Viec'
                UNION
                SELECT ma_nguoi_dung as ma, 'nguoi_Dung' as loai FROM nguoi_dung WHERE loai_nguoi_dung='cong_Ty' AND ma_cong_ty=@ma_cong_ty
                ";
                    break;

                case LoaiThongBao.thu_Moi_Phong_Van:
                    sql = @"
                SELECT ma_nguoi_dung as ma, 'nguoi_Dung' as loai FROM nguoi_dung WHERE ma_nguoi_dung=@ma_nguoi_tim_viec
                UNION
                SELECT ma_nguoi_dung as ma, 'nguoi_Dung' as loai FROM nguoi_dung WHERE loai_nguoi_dung='cong_Ty' AND ma_cong_ty= @ma_cong_ty AND ma_nguoi_tim_viec = @ma_nguoi_tim_viec";
                    break;

                default:
                    return dsNguoiNhan;
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
        public static bool xoaThongBaoCongTy(thong_bao thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "DELETE FROM thong_bao WHERE ma_thong_bao = @ma_thong_bao AND ma_cong_ty = @ma_cong_ty";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_thong_bao", thong_Bao.ma_thong_bao);
            cmd.Parameters.AddWithValue("@ma_cong_ty", thong_Bao.ma_cong_ty);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public static List<thong_bao> thongBaoCongTyRieng(int ma_cong_ty)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            string sql = "SELECT * FROM thong_bao WHERE ma_cong_ty = @ma_cong_ty";

            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_cong_ty", ma_cong_ty);

            using var reader = cmd.ExecuteReader();
            var danhSachThongBao = new List<thong_bao>();

            while (reader.Read())
            {
                var thongBao = new thong_bao
                {
                    ma_thong_bao = reader.IsDBNull(reader.GetOrdinal("ma_thong_bao")) ? 0 : reader.GetInt32("ma_thong_bao"),
                    ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                    tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? "" : reader.GetString("tieu_de"),
                    noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? "" : reader.GetString("noi_dung"),
                    ngay_tao = reader.IsDBNull(reader.GetOrdinal("ngay_tao")) ? DateTime.MinValue : reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.IsDBNull(reader.GetOrdinal("ngay_cap_nhat")) ? DateTime.MinValue : reader.GetDateTime("ngay_cap_nhat"),
                    ma_bai_dang = reader.IsDBNull(reader.GetOrdinal("ma_bai_dang")) ? null : (int?)reader.GetInt32("ma_bai_dang"),
                    bai_Dang = new bai_dang
                    {
                        tieu_de = reader.IsDBNull(reader.GetOrdinal("tieu_de")) ? "" : reader.GetString("tieu_de"),
                        noi_dung = reader.IsDBNull(reader.GetOrdinal("noi_dung")) ? "" : reader.GetString("noi_dung")
                    }
                };
                danhSachThongBao.Add(thongBao);
            }

            return danhSachThongBao;
        }
    }
}