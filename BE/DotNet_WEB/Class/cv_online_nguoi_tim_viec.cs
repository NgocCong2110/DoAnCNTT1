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
        public nguoi_tim_viec? nguoi_Tim_Viec {get; set;}
        public string? ho_ten {get; set;}
        public string? email {get; set;}
        public string? dien_thoai {get; set;}
        public DateTime ngay_sinh {get; set;}
        public string? dia_chi {get; set;}
        public string? truong_hoc {get; set;}
        public string? chuyen_nganh {get; set;}
        public string? kinh_nghiem {get; set;}
        public string? ky_nang {get; set;}
        public string? du_an {get; set;}
        public string? muc_tieu {get; set;}
        public DateTime ngay_tao {get; set;}
    }
}