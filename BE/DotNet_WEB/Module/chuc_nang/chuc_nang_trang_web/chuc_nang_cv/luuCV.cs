using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNet_WEB.Class;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace DotNet_WEB.Module.chuc_nang.chuc_nang_trang_web.chuc_nang_cv
{
    public class luuCV
    {
        private static readonly string chuoi_KetNoi =
            "server=localhost;user=root;password=123456;database=hethong_timviec";

        public static async Task<string> luuCVOnline(cv_online_nguoi_tim_viec cv)
        {
            string duong_dan_folder = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruCVOnline");
            if (!Directory.Exists(duong_dan_folder))
                Directory.CreateDirectory(duong_dan_folder);

            string ten_file = !string.IsNullOrEmpty(cv.ten_cv)
                ? $"{cv.ten_cv.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.pdf"
                : $"{cv.ho_ten.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            string duong_dan_pdf = Path.Combine(duong_dan_folder, ten_file);
            string duong_dan_file_pdf = $"LuuTruCVOnline/{ten_file}";




            string duong_dan_folder_anh_dai_dien = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDien");
            if (!Directory.Exists(duong_dan_folder_anh_dai_dien))
            {
                Directory.CreateDirectory(duong_dan_folder_anh_dai_dien);
            }

            if (!string.IsNullOrEmpty(cv.anh_dai_dien))
            {
                var base64Data = cv.anh_dai_dien.Split(',')[1];
                var mimeType = cv.anh_dai_dien.Substring(5, cv.anh_dai_dien.IndexOf(";") - 5); 
                string extension = mimeType switch
                {
                    "image/png" => "png",
                    "image/jpeg" => "jpg",
                    "image/jpg" => "jpg",
                    _ => "jpg"
                };

                var bytes = Convert.FromBase64String(base64Data);
                string ten_file_anh = $"{cv.ten_cv.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.{extension}";
                string duong_dan_file_anh_dai_dien = Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDien", ten_file_anh);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDien")))
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "LuuTruAnhDaiDien"));

                await File.WriteAllBytesAsync(duong_dan_file_anh_dai_dien, bytes);

                cv.anh_dai_dien = $"LuuTruAnhDaiDien/{ten_file_anh}";
            }

            taoPDFMauCVMacDinh(cv, duong_dan_pdf);

            long ma_cv = 0;

            using (var conn = new MySqlConnection(chuoi_KetNoi))
            {
                await conn.OpenAsync();

                string sql = @"
                    INSERT INTO cv_online_nguoi_tim_viec
                    (ma_nguoi_tim_viec, ten_cv, anh_dai_dien, ho_ten, email, dien_thoai, ngay_sinh, gioi_tinh, dia_chi,
                     chuyen_nganh, ky_nang, du_an, muc_tieu, vi_tri_ung_tuyen, ngay_tao, duong_dan_file_pdf)
                    VALUES (@ma_nguoi_tim_viec, @ten_cv, @anh_dai_dien, @ho_ten, @email, @dien_thoai, @ngay_sinh, 
                            @gioi_tinh, @dia_chi, @chuyen_nganh, @ky_nang, @du_an, @muc_tieu, @vi_tri_ung_tuyen, NOW(), @duong_dan_file_pdf);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ma_nguoi_tim_viec", cv.ma_nguoi_tim_viec);
                    cmd.Parameters.AddWithValue("@ten_cv", cv.ten_cv);
                    cmd.Parameters.AddWithValue("@anh_dai_dien", cv.anh_dai_dien);
                    cmd.Parameters.AddWithValue("@ho_ten", cv.ho_ten);
                    cmd.Parameters.AddWithValue("@email", cv.email);
                    cmd.Parameters.AddWithValue("@dien_thoai", cv.dien_thoai);
                    cmd.Parameters.AddWithValue("@ngay_sinh", cv.ngay_sinh);
                    cmd.Parameters.AddWithValue("@gioi_tinh", cv.gioi_tinh.ToString());
                    cmd.Parameters.AddWithValue("@dia_chi", cv.dia_chi);
                    cmd.Parameters.AddWithValue("@chuyen_nganh", cv.chuyen_nganh);
                    cmd.Parameters.AddWithValue("@ky_nang", cv.ky_nang);
                    cmd.Parameters.AddWithValue("@du_an", cv.du_an);
                    cmd.Parameters.AddWithValue("@muc_tieu", cv.muc_tieu);
                    cmd.Parameters.AddWithValue("@vi_tri_ung_tuyen", cv.vi_tri_ung_tuyen);
                    cmd.Parameters.AddWithValue("@duong_dan_file_pdf", duong_dan_file_pdf);

                    ma_cv = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                }

                if (cv.hoc_Van != null)
                {
                    foreach (var hv in cv.hoc_Van)
                    {
                        string sqlHV = @"INSERT INTO hoc_van (ma_cv, thoi_gian_hoc_tap, ten_truong, nganh_hoc, mo_ta)
                                         VALUES (@ma_cv, @thoi_gian, @truong, @nganh, @mo_ta)";
                        using var cmdHV = new MySqlCommand(sqlHV, conn);
                        cmdHV.Parameters.AddWithValue("@ma_cv", ma_cv);
                        cmdHV.Parameters.AddWithValue("@thoi_gian", hv.thoi_gian_hoc_tap);
                        cmdHV.Parameters.AddWithValue("@truong", hv.ten_truong);
                        cmdHV.Parameters.AddWithValue("@nganh", hv.nganh_hoc);
                        cmdHV.Parameters.AddWithValue("@mo_ta", hv.mo_ta);
                        await cmdHV.ExecuteNonQueryAsync();
                    }
                }

                if (cv.kinh_Nghiem != null)
                {
                    foreach (var kn in cv.kinh_Nghiem)
                    {
                        string sqlKN = @"INSERT INTO kinh_nghiem (ma_cv, thoi_gian_lam_viec, ten_cong_ty, vi_tri, mo_ta)
                                         VALUES (@ma_cv, @thoi_gian, @cty, @vitri, @mota)";
                        using var cmdKN = new MySqlCommand(sqlKN, conn);
                        cmdKN.Parameters.AddWithValue("@ma_cv", ma_cv);
                        cmdKN.Parameters.AddWithValue("@thoi_gian", kn.thoi_gian_lam_viec);
                        cmdKN.Parameters.AddWithValue("@cty", kn.ten_cong_ty);
                        cmdKN.Parameters.AddWithValue("@vitri", kn.vi_tri);
                        cmdKN.Parameters.AddWithValue("@mota", kn.mo_ta);
                        await cmdKN.ExecuteNonQueryAsync();
                    }
                }
            }

            return duong_dan_file_pdf;
        }

        public static void taoPDFMauCVMacDinh(cv_online_nguoi_tim_viec cv, string duong_dan_file)
        {
            string duong_dan_file_anh_dai_dien = $"http://localhost:65001/{cv.anh_dai_dien}";
            string css = @"
.cv-container {
  max-width: 900px;
  margin: 40px auto;
  padding: 40px;
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 8px 24px rgba(0,0,0,0.08);
  font-family: 'Segoe UI', Roboto, sans-serif;
}
.cv-header {
  display: flex;
  align-items: center;
  gap: 30px;
  border-bottom: 3px solid #0d6efd;
  padding-bottom: 25px;
  margin-bottom: 25px;
}
.avatar {
  width: 140px; height: 140px; border-radius: 50%; overflow: hidden;
  box-shadow: 0 4px 8px rgba(0,0,0,0.15);
}
.avatar img { width:100%; height:100%; object-fit:cover; border-radius:50%; border:3px solid #0d6efd; }
.info h1 { font-size: 26px; margin: 0; color: #222; }
.cv-section { margin-bottom: 25px; }
.cv-section h2 { color: #0d6efd; font-size: 18px; margin-bottom: 10px; }
.cv-item { margin-bottom: 16px; border-left: 3px solid #e0e0e0; padding-left: 12px; }
";

            string html = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>{css}</style>
</head>
<body>
<div class='cv-container'>
  <div class='cv-header'>
    <div class='avatar'>
      <img src='{duong_dan_file_anh_dai_dien ?? "assets/default-avatar.png"}' alt='avatar' />
    </div>
    <div class='info'>
      <h1>{cv.ho_ten}</h1>
      <p><b>Vị trí ứng tuyển:</b> {cv.vi_tri_ung_tuyen}</p>
      <p><b>Email:</b> {cv.email}</p>
      <p><b>Điện thoại:</b> {cv.dien_thoai}</p>
      <p><b>Ngày sinh:</b> {(cv.ngay_sinh != DateTime.MinValue ? cv.ngay_sinh.ToString("dd/MM/yyyy") : "")}</p>
      <p><b>Giới tính:</b> {(cv.gioi_tinh == GioiTinh.nam ? "Nam" : cv.gioi_tinh == GioiTinh.nu ? "Nữ" : "Khác")}</p>
      <p><b>Địa chỉ:</b> {cv.dia_chi}</p>
    </div>
  </div>

  <div class='cv-section'><h2>Chuyên ngành</h2><p>{cv.chuyen_nganh}</p></div>
  <div class='cv-section'><h2>Kỹ năng</h2><p>{cv.ky_nang}</p></div>
  <div class='cv-section'><h2>Dự án</h2><p>{cv.du_an}</p></div>
  <div class='cv-section'><h2>Mục tiêu nghề nghiệp</h2><p>{cv.muc_tieu}</p></div>
  <div class='cv-section'><h2>Học vấn</h2>";

            if (cv.hoc_Van != null)
            {
                foreach (var hv in cv.hoc_Van)
                {
                    html += $@"<div class='cv-item'><p><b>{hv.thoi_gian_hoc_tap}</b> – {hv.ten_truong} ({hv.nganh_hoc})</p><p>{hv.mo_ta}</p></div>";
                }
            }

            html += "<h2>Kinh nghiệm làm việc</h2>";

            if (cv.kinh_Nghiem != null)
            {
                foreach (var kn in cv.kinh_Nghiem)
                {
                    html += $@"<div class='cv-item'><p><b>{kn.thoi_gian_lam_viec}</b> – {kn.ten_cong_ty} ({kn.vi_tri})</p><p>{kn.mo_ta}</p></div>";
                }
            }

            html += "</div></div></body></html>";

            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Out = duong_dan_file
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8", LoadImages = true, EnableJavascript = true }
                    }
                }
            };
            converter.Convert(doc);
        }
    }
}
