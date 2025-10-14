using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class vi_tri
    {
        [Key]
        public int ma_vi_tri { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public int ma_viec { get; set; }
        [ForeignKey(nameof(ma_viec))]
        public viec_lam? viec_Lam { get; set; }
        public string? dia_chi { get; set; }
        public string? thanh_pho { get; set; }
        public decimal kinh_do { get; set; }
        public decimal vi_do { get; set; }
    }
}