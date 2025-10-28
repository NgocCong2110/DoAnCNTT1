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
        public static bool guiThongBaoToiServer(thong_bao thong_Bao)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "insert into thong_bao (tieu_de, noi_dung, loai_thong_bao, ma_quan_tri, ma_cong_ty, ma_nguoi_nhan, ngay_tao, ngay_cap_nhat) values(@tieu_de, @noi_dung, @loai_thong_bao, @ma_quan_tri, @ma_cong_ty, @ma_nguoi_nhan, @ngay_tao, @ngay_cap_nhat)";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@tieu_de", thong_Bao.tieu_de);
            cmd.Parameters.AddWithValue("@noi_dung", thong_Bao.noi_dung);
            cmd.Parameters.AddWithValue("@loai_thong_bao", thong_Bao.loai_thong_bao);
            cmd.Parameters.AddWithValue("@ma_quan_tri", thong_Bao.ma_quan_tri);
            cmd.Parameters.AddWithValue("@ma_cong_ty", thong_Bao.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", thong_Bao.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@ngay_tao", thong_Bao.ngay_tao);
            cmd.Parameters.AddWithValue("@ngay_cap_nhat", thong_Bao.ngay_cap_nhat);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}