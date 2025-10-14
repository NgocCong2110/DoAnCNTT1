using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thong_bao_kieu_nguoi_dung
    {
        public string? kieu_nguoi_dung { get; set; }
        public int? ma_nguoi_tim_viec { get; set; }
    }
}