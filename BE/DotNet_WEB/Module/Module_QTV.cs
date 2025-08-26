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
    public class Module_QTV
    {
        private readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";


        public List<nguoi_dung> truyVanToanBoND()
        {
            List<nguoi_dung> danhSach_NguoiDung = new List<nguoi_dung>();
            using var conn = new MySqlConnection(chuoi_KetNoi);
            conn.Open();
            string truyVan_ToanBo = "select ten_nguoi_dung, email, ngay_sinh, ngay_tao from nguoi_dung";
            using var cmd = new MySqlCommand(truyVan_ToanBo, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var nguoidung = new nguoi_dung
                {
                    ma_nguoi_dung = reader.GetInt32("ma_nguoi_dung"),
                    loai_nguoi_dung = (LoaiNguoiDung)Enum.Parse(typeof(LoaiNguoiDung), reader.GetString("loai_nguoi_dung")),
                    ten_dang_nhap = reader.GetString("ten_dang_nhap"),
                    email = reader.GetString("email"),
                    ngay_tao = reader.GetDateTime("ngay_tao"),
                    ngay_cap_nhat = reader.GetDateTime("ngay_cap_nhat")
                };
                danhSach_NguoiDung.Add(nguoidung);
            }
            return danhSach_NguoiDung;
        }

        
        
    }
}