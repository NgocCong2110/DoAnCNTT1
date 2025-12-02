using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class tinh_thanh
    {
        public int ma_tinh { get; set; }
        public string? ten_tinh { get; set; }
    }
}