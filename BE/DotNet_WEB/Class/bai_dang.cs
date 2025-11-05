using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class bai_dang
    {
        [Key]
        public int ma_bai_dang { get; set; }

        public int ma_nguoi_dang { get; set; }
        [ForeignKey(nameof(ma_nguoi_dang))]
        public cong_ty? cong_Ty { get; set; }

        public string? ten_nguoi_dang { get; set; }

        public string? tieu_de { get; set; }

        public string? noi_dung { get; set; }

        public LoaiBai loai_bai { get; set; } = LoaiBai.tuyen_Dung;

        public TrangThaiBai trang_thai { get; set; } = TrangThaiBai.cong_Khai;

        public DateTime ngay_tao { get; set; } = DateTime.Now;

        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;   
    }
}