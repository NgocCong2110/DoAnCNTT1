using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class hoc_van
    {
        [Key]
        public int ma_hoc_van { get; set; }
        public int ma_cv { get; set; }
        [ForeignKey(nameof(ma_cv))]
        public cv_online_nguoi_tim_viec? cv_online_nguoi_tim_viec { get; set; }
        public string? thoi_gian_hoc_tap { get; set; }
        public string? ten_truong { get; set; }
        public string? nganh_hoc { get; set; }
        public string? mo_ta { get; set; }
    }
}