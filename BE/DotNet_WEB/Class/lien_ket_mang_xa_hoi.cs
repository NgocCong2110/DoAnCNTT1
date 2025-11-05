using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DotNet_WEB.Class
{
    public class lien_ket_mang_xa_hoi
    {
        [Key]
        public int ma_lien_ket { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public string? ten_mang_xa_hoi { get; set; }
        public string? duong_dan { get; set; }
    }
}