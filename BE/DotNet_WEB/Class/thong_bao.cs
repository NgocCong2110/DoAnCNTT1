using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thong_bao
    {
        [Key]
        public int ma_thong_bao { get; set; }
        public string? tieu_de { get; set; }
        public string? noi_dung { get; set; }
        public LoaiThongBao loai_thong_bao { get; set; } = LoaiThongBao.None;
        public int? ma_quan_tri { get; set; }
        [ForeignKey(nameof(ma_quan_tri))]
        public quan_tri? quan_Tri { get; set; }
        public int? ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public int? ma_nguoi_tim_viec { get; set; }
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_Tim_Viec { get; set; }
        public int ma_nguoi_nhan { get; set; }
        [ForeignKey(nameof(ma_nguoi_nhan))]
        public nguoi_dung? nguoi_Dung { get; set; }
        public DateTime ngay_tao { get; set; }
        public DateTime ngay_cap_nhat { get; set; }
        public List<chi_tiet_thu_moi>? chi_tiet_thu_moi { get; set; }
    }
}