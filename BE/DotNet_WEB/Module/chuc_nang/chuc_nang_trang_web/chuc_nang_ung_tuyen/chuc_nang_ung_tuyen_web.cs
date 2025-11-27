using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_ung_tuyen
{
    public class chuc_nang_ung_tuyen_web
    {
        private static readonly string chuoi_KetNoi = "server=localhost;user=root;password=123456;database=hethong_timviec";
        public static bool kiemTraUngTuyen(ung_tuyen ung_Tuyen)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string sql = "select * from ung_tuyen where ma_viec = @ma_Viec and ma_cong_ty = @ma_Cong_Ty and ma_nguoi_tim_viec = @ma_Nguoi_Tim_Viec";
            using var cmd = new MySqlCommand(sql, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ung_Tuyen.ma_viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ung_Tuyen.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ung_Tuyen.ma_nguoi_tim_viec);
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }
        public static bool ungTuyenCongViec(ung_tuyen ung_Tuyen)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();
            string ut = "INSERT INTO ung_tuyen (ma_viec, ma_cong_ty, ma_nguoi_tim_viec, ma_nguoi_nhan, ma_cv, trang_thai, trang_thai_duyet) VALUES (@ma_Viec, @ma_Cong_Ty, @ma_Nguoi_Tim_Viec, @ma_nguoi_nhan, @ma_CV, @trang_Thai, @trang_Thai_Duyet)";
            using var cmd = new MySqlCommand(ut, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ung_Tuyen.ma_viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ung_Tuyen.ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ung_Tuyen.ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", ung_Tuyen.ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@ma_CV", ung_Tuyen.ma_cv);
            cmd.Parameters.AddWithValue("@trang_Thai", "dang_Cho");
            cmd.Parameters.AddWithValue("@trang_Thai_Duyet", "chua_Duyet");
            if (cmd.ExecuteNonQuery() > 0)
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> ungTuyenCongViecUploadCV(int ma_viec, int ma_cong_ty, int ma_nguoi_tim_viec, int ma_nguoi_nhan, IFormFile duong_dan_file_cv_upload)
        {
            using var coon = new MySqlConnection(chuoi_KetNoi);
            coon.Open();

            var duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCVUngTuyen");
            if (!Directory.Exists(duong_dan_folder))
            {
                Directory.CreateDirectory(duong_dan_folder);
            }

            string ten_file = $"{Guid.NewGuid()}_{duong_dan_file_cv_upload.FileName}";
            var duong_dan_file = Path.Combine(duong_dan_folder, ten_file);
            using (var stream = new FileStream(duong_dan_file, FileMode.Create))
            {
                await duong_dan_file_cv_upload.CopyToAsync(stream);
            }

            var duong_dan = $"LuuTruCVUngTuyen/{ten_file}";

            string ung_tuyen = "INSERT INTO ung_tuyen (ma_viec, ma_cong_ty, ma_nguoi_tim_viec, ma_nguoi_nhan, duong_dan_file_cv_upload, trang_thai, trang_thai_duyet) VALUES (@ma_Viec, @ma_Cong_Ty, @ma_Nguoi_Tim_Viec, @ma_nguoi_nhan, @duong_dan_file_cv_upload, @trang_Thai, @trang_Thai_Duyet)";
            using var cmd = new MySqlCommand(ung_tuyen, coon);
            cmd.Parameters.AddWithValue("@ma_Viec", ma_viec);
            cmd.Parameters.AddWithValue("@ma_Cong_Ty", ma_cong_ty);
            cmd.Parameters.AddWithValue("@ma_Nguoi_Tim_Viec", ma_nguoi_tim_viec);
            cmd.Parameters.AddWithValue("@ma_nguoi_nhan", ma_nguoi_nhan);
            cmd.Parameters.AddWithValue("@duong_dan_file_cv_upload", duong_dan);
            cmd.Parameters.AddWithValue("@trang_Thai", "dang_Cho");
            cmd.Parameters.AddWithValue("@trang_Thai_Duyet", "chua_Duyet");
            if (await cmd.ExecuteNonQueryAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}