using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class danh_gia
    {
        [Key]
        public int ma_danh_gia { get; set; }
        public int ma_nguoi_danh_gia { get; set; }
        [ForeignKey(nameof(ma_nguoi_danh_gia))]
        public nguoi_dung? ma_nguoi_dung { get; set; } 
        public string? ten_nguoi_danh_gia { get; set; }
        public int so_diem_danh_gia { get; set; }
        public string? noi_dung_danh_gia { get; set;}
        public TrangThaiDanhGia trang_thai_danh_gia { get; set; } = TrangThaiDanhGia.chua_Hien_Thi;
        public DateTime ngay_tao { get; set; } = DateTime.Now;
    }
}