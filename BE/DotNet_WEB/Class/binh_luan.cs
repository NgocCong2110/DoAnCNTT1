using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class binh_luan
    {
        [Key]
        public int ma_binh_luan { get; set; }
        public int ma_bai_dang { get; set; }
        public int ma_nguoi_binh_luan { get; set; }
        public string? noi_dung { get; set; }

        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}