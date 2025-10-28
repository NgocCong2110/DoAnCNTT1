using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class phuc_loi_viec_lam
    {
        public int ma_viec { get; set; }
        [ForeignKey(nameof(ma_viec))]
        public viec_lam? viec_Lam { get; set; }
        
        public int ma_phuc_loi { get; set; }
        [ForeignKey(nameof(ma_phuc_loi))]
        public phuc_loi? phuc_Loi { get; set; }
    }
}