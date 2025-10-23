using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class viec_lam_ket_qua
    {
        public int ma_viec { get; set; }
        [ForeignKey(nameof(ma_viec))]
        public viec_lam? viec_Lam { get; set; }
        public string? nganh_nghe { get; set; }
        public string? vi_tri { get; set; }
        public string? kinh_nghiem { get; set; }
        public string? tieu_de { get; set; }
        public string? mo_ta { get; set; }
        public string? yeu_cau { get; set; }
        public string? muc_luong { get; set; }
        public string? dia_diem { get; set; }
        public LoaiHinhViecLam loai_hinh { get; set; } = LoaiHinhViecLam.toan_Thoi_Gian;
        public int diem_phu_hop { get; set; }
        public int ma_bai_dang { get; set; }
    }
}