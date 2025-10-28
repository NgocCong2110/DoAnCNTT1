using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class phuc_loi
    {
        [Key]
        public int ma_phuc_loi { get; set; }
        public string? ten_phuc_loi { get; set; }
    }
}