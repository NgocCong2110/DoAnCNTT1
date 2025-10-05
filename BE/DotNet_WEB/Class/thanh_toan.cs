using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thanh_toan
    {
        [Key]
        public int ma_thanh_toan { get; set; }
        public int ma_don_hang { get; set; }
        [ForeignKey(nameof(ma_don_hang))]
        public don_hang? don_Hang { get; set; }
        public decimal so_tien { get; set; }
        public string? response_code { get; set; }
        public string? transaction_no { get; set; }
        public string? bank_code { get; set; }
        public DateTime ngay_thanh_toan { get; set; }
        public TrangThaiThanhToan trang_thai_thanh_toan { get; set; } = TrangThaiThanhToan.None;
        public DateTime ngay_tao { get; set; }
    }
}