using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class cv_online_nguoi_tim_viec
    {
        [Key]
        public int ma_cv {get; set;}
        public int ma_nguoi_tim_viec {get; set;}
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_Tim_Viec { get; set; }
        public string? ten_cv { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? ho_ten {get; set;}
        public string? email {get; set;}
        public string? dien_thoai {get; set;}
        public DateTime ngay_sinh { get; set; }
        public GioiTinh gioi_tinh { get; set; } = GioiTinh.None;
        public string? dia_chi {get; set;}
        public string? chuyen_nganh {get; set;}
        public string? ky_nang {get; set;}
        public string? du_an {get; set;}
        public string? muc_tieu {get; set;}
        public string? vi_tri_ung_tuyen { get; set; }
        public string? duong_dan_file_pdf { get; set; }
        public DateTime ngay_tao { get; set; }
        public List<hoc_van>? hoc_Van { get; set; }
        public List<kinh_nghiem>? kinh_Nghiem { get; set; }
        public int mau_cv { get; set; }
    }
}