using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class quan_tri
    {
        [Key]
        public int ma_quan_tri { get; set; }
        public string? ten_dang_nhap { get; set; }
        public string? mat_khau { get; set; }
        public string? ho_ten { get; set; }
        public string? email { get; set; }
        public string? dien_thoai { get; set; }
        public string? duong_dan_anh_dai_dien { get; set; }
        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}