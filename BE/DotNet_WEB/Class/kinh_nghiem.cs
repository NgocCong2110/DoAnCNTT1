using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class kinh_nghiem
    {
        [Key]
        public int ma_kinh_nghiem { get; set; }
        public int ma_cv { get; set; }
        [ForeignKey(nameof(ma_cv))]
        public cv_online_nguoi_tim_viec? cv_online_nguoi_tim_viec { get; set; }
        public string? thoi_gian_lam_viec { get; set; }
        public string? ten_cong_ty { get; set; }
        public string? vi_tri { get; set; }
        public string? mo_ta { get; set; }
    }
}