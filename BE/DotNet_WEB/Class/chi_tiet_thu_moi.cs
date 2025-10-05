using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class chi_tiet_thu_moi
    {
        [Key]
        public int ma_thu_moi { get; set; }

        public int ma_thong_bao { get; set; }
        [ForeignKey(nameof(ma_thong_bao))]
        public thong_bao? thong_Bao { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }

        public DateTime thoi_gian { get; set; }
        public string? dia_diem { get; set; }
        public string? noi_dung { get; set; }

        public int? ma_nguoi_tim_viec { get; set; }
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_Tim_Viec { get; set; }
    }
}