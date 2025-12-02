using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thanh_toan_dich_vu
    {
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public string? ma_dich_vu { get; set; }
        [ForeignKey(nameof(ma_dich_vu))]
        public dich_vu? dich_Vu { get; set; }
        public TrangThaiThanhToan trang_thai_thanh_toan { get; set; } = TrangThaiThanhToan.chua_Thanh_Toan;
    }
}