using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_ung_tuyen_nguoi_tim_viec
{
    public class chuc_nang_ung_tuyen_nguoi_tim_viec_web
    {   private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
         public static List<ung_tuyen> layDanhSachUngTuyen(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select ut.ngay_ung_tuyen, vl.ma_viec, vl.nganh_nghe, vl.vi_tri, vl.kinh_nghiem, vl.muc_luong, vl.yeu_cau,
	                            ct.ma_cong_ty, ct.ten_cong_ty, ct.dia_chi, ut.trang_thai
                            from ung_tuyen ut
                            join nguoi_tim_viec ntv on ut.ma_nguoi_tim_viec = ntv.ma_nguoi_tim_viec
                            join viec_lam vl on ut.ma_viec = vl.ma_viec
                            join cong_ty ct on ut.ma_cong_ty = ct.ma_cong_ty
                            where ut.ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_Nguoi_Tim_Viec);
            using var reader = cmd.ExecuteReader();
            var danh_sach_lich_su = new List<ung_tuyen>();
            while (reader.Read())
            {
                var ung_Tuyen = new ung_tuyen
                {
                    ngay_ung_tuyen = reader.IsDBNull(reader.GetOrdinal("ngay_ung_tuyen")) ? DateTime.MinValue : reader.GetDateTime("ngay_ung_tuyen"),
                    trang_thai = reader.IsDBNull(reader.GetOrdinal("trang_thai")) ? TrangThaiUngTuyen.None : (TrangThaiUngTuyen)Enum.Parse(typeof(TrangThaiUngTuyen), reader.GetString("trang_thai")),
                    viec_Lam = new viec_lam
                    {
                        ma_viec = reader.IsDBNull(reader.GetOrdinal("ma_viec")) ? 0 : reader.GetInt32("ma_viec"),
                        nganh_nghe = reader.IsDBNull(reader.GetOrdinal("nganh_nghe")) ? null : reader.GetString("nganh_nghe"),
                        vi_tri = reader.IsDBNull(reader.GetOrdinal("vi_tri")) ? null : reader.GetString("vi_tri"),
                        kinh_nghiem = reader.IsDBNull(reader.GetOrdinal("kinh_nghiem")) ? null : reader.GetString("kinh_nghiem"),
                        muc_luong = reader.IsDBNull(reader.GetOrdinal("muc_luong")) ? null : reader.GetString("muc_luong"),
                        yeu_cau = reader.IsDBNull(reader.GetOrdinal("yeu_cau")) ? null : reader.GetString("yeu_cau")
                    },
                    cong_Ty = new cong_ty
                    {
                        ma_cong_ty = reader.IsDBNull(reader.GetOrdinal("ma_cong_ty")) ? 0 : reader.GetInt32("ma_cong_ty"),
                        ten_cong_ty = reader.IsDBNull(reader.GetOrdinal("ten_cong_ty")) ? null : reader.GetString("ten_cong_ty"),
                        dia_chi = reader.IsDBNull(reader.GetOrdinal("dia_chi")) ? null : reader.GetString("dia_chi")
                    }
                };

                danh_sach_lich_su.Add(ung_Tuyen);
            }
            return danh_sach_lich_su;
        }
    }
}