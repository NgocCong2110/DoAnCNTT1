using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class cv_nguoi_tim_viec
    {
        [Key]
        public int ma_cv {get; set;}
        public int ma_nguoi_tim_viec {get; set;}
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_Tim_Viec {get; set;}
        public string? ten_file {get; set;}
        public string? duong_dan_file {get; set;}
        public DateTime ngay_tao {get; set;}
    }
}