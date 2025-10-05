using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class chi_tiet_don_hang
    {
        [Key]
        public int ma_chi_tiet { get; set; }
        public int ma_don_hang { get; set; }
        [ForeignKey(nameof(ma_don_hang))]
        public don_hang? don_Hang { get; set; }
        public int ma_dich_vu { get; set; }
        [ForeignKey(nameof(ma_dich_vu))]
        public dich_vu? dich_Vu { get; set; }
        public int so_luong { get; set; }
        public decimal don_gia { get; set; }
        public TrangThaiDonHang trang_thai_don_hang { get; set; } = TrangThaiDonHang.cho_Thanh_Toan;
    }
}