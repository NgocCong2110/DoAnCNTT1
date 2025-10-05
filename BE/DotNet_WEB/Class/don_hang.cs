using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class don_hang
    {
        [Key]
        public int ma_don_hang { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public decimal tong_tien { get; set; }
        public TrangThaiDonHang trang_thai_don_hang { get; set; } = TrangThaiDonHang.cho_Thanh_Toan;
        public DateTime ngay_tao { get; set; }
    }
}