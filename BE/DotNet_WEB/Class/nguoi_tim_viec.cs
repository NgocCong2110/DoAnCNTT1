using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class nguoi_tim_viec
    {
        [Key]
        public int ma_nguoi_tim_viec { get; set; }
        public string? ho_ten { get; set; }
        public string? ten_dang_nhap { get; set; }
        public string? email { get; set; }
        public string? dien_thoai { get; set; }
        public string? mat_khau { get; set; }
        public DateTime ngay_sinh { get; set; }

        public GioiTinh gioi_tinh { get; set; } = GioiTinh.None;
        public TrinhDoHocVan trinh_do_hoc_van { get; set; } = TrinhDoHocVan.khac;

        public string? dia_chi { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? quoc_tich { get; set; }
        public string? mo_ta { get; set; }

        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}