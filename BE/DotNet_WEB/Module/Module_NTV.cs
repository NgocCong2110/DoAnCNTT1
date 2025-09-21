using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;


namespace DotNet_WEB.Module
{
    public class Module_NTV
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static List<ung_tuyen> layDanhSachUngTuyen(int ma_Nguoi_Tim_Viec)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = @"select ut.ngay_ung_tuyen, vl.nganh_nghe, vl.vi_tri, vl.kinh_nghiem, vl.muc_luong, vl.yeu_cau,
	                            ct.ten_cong_ty, ct.dia_chi
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
                    ngay_ung_tuyen = reader.GetDateTime("ngay_ung_tuyen"),
                    viec_Lam = new viec_lam
                    {
                        nganh_nghe = reader.GetString("nganh_nghe"),
                        vi_tri = reader.GetString("vi_tri"),
                        kinh_nghiem = reader.GetString("kinh_nghiem"),
                        muc_luong = reader.GetString("muc_luong"),
                        yeu_cau = reader.GetString("yeu_cau")
                    },
                    cong_Ty = new cong_ty
                    {
                        ten_cong_ty = reader.GetString("ten_cong_ty"),
                        dia_chi = reader.GetString("dia_chi")
                    }
                };

                danh_sach_lich_su.Add(ung_Tuyen);
            }
            return danh_sach_lich_su;
        }
    }
}