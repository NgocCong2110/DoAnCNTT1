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
        public int? ma_bai_dang { get; set; }
        [ForeignKey(nameof(ma_bai_dang))]
        public bai_dang? bai_Dang { get; set; }
        public string? ten_nguoi_dang {get; set;}

        public string? tieu_de {get; set;}

        public string? noi_dung {get; set;}

        public int ma_nguoi_bao_cao {get; set;}
        [ForeignKey(nameof(ma_nguoi_bao_cao))]
        public nguoi_tim_viec? nguoi_Tim_Viec { get; set; }
        public string? loai_vi_pham { get; set; }

        public string? noi_dung_bao_cao { get; set; }
        public TrangThaiXuLy trang_thai_xu_ly { get; set; } = TrangThaiXuLy.chua_Xu_Ly;

        public DateTime ngay_bao_cao {get; set;} = DateTime.Now;
    }
}