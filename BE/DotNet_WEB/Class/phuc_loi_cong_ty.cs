using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class phuc_loi_cong_ty
    {
        [Key]
        public int ma_phuc_loi_cty { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public string? ten_phuc_loi { get; set; }
        public string? mo_ta { get; set; }
    }
}