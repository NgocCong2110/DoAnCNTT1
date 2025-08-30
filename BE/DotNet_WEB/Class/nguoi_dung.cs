using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class nguoi_dung
    {
        [Key]
        public int ma_nguoi_dung { get; set; }

        public LoaiNguoiDung loai_nguoi_dung { get; set; } = LoaiNguoiDung.None;

        public int? ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_ty { get; set; }

        public int? ma_nguoi_tim_viec { get; set; }
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_tim_viec { get; set; }

        public string? ten_dang_nhap { get; set; }
        public string? mat_khau { get; set; }
        public string? email { get; set; }

        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;

    }
}