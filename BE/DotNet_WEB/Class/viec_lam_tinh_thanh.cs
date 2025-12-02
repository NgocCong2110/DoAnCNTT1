using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class viec_lam_tinh_thanh
    {
        public int ma_viec { get; set; }
        [ForeignKey(nameof(ma_viec))]
        public viec_lam? viec_Lam { get; set; }
        public int ma_tinh { get; set; }
        [ForeignKey(nameof(ma_tinh))]
        public tinh_thanh? tinh_Thanh { get; set; }
    }
}