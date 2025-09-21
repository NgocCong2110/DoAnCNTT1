using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class bai_dang_vi_pham
    {
        public int ma_bai_vi_pham {get; set;}
        [ForeignKey(nameof(ma_bai_vi_pham))]
        public bai_dang? bai_Dang {get; set;}

        public string? ten_nguoi_dang {get; set;}

        public string? tieu_de {get; set;}

        public string? noi_dung {get; set;}

        public int ma_nguoi_bao_cao {get; set;}
        [ForeignKey(nameof(ma_nguoi_bao_cao))]
        public nguoi_dung? nguoi_Dung {get; set;}

        public string? noi_dung_bao_cao {get; set;}

        public DateTime ngay_bao_cao {get; set;} = DateTime.Now;
    }
}