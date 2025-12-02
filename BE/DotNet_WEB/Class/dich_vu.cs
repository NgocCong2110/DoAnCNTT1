using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class dich_vu
    {
        [Key]
        public string? ma_dich_vu {get; set;}
        public string? ten_dich_vu {get; set;}
        public decimal so_tien { get; set; }
        public DateTime ngay_tao { get;  set; }
    }
}