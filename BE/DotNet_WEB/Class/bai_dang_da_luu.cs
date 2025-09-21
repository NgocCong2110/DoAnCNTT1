using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class bai_dang_da_luu
    {
        public int ma_bai_dang {get; set;}
        [ForeignKey(nameof(ma_bai_dang))]
        public bai_dang? bai_Dang { get; set; }
        public int ma_nguoi_luu {get; set;}
        [ForeignKey(nameof(ma_nguoi_luu))]
        public nguoi_dung? nguoi_Dung { get; set; }

    }
}