using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class nganh_nghe
    {
        [Key]
        public int ma_nganh { get; set; }
        public string? ten_nganh { get; set; }
        public string? mo_ta { get; set; }
    }
}