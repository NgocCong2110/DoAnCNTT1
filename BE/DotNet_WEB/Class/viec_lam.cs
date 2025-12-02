using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class viec_lam
    {
        [Key]
        public int ma_viec { get; set; }
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public string? nganh_nghe { get; set; }
        public string? vi_tri { get; set; }
        public string? kinh_nghiem { get; set; }
        public string? tieu_de { get; set; }
        public string? mo_ta { get; set; }
        public string? yeu_cau { get; set; }
        public string? muc_luong { get; set; }
        public decimal? muc_luong_thap_nhat { get; set; }
        public decimal? muc_luong_cao_nhat { get; set; }
        public string? quyen_loi_cong_viec { get; set; }
        public TrinhDoHocVan trinh_do_hoc_van_yeu_cau { get; set; } = TrinhDoHocVan.None;
        public string? thoi_gian_lam_viec { get; set; }
        public string? dia_diem { get; set; }
        public DateTime thoi_han_nop_cv { get; set; }
        public LoaiHinhViecLam loai_hinh { get; set; } = LoaiHinhViecLam.toan_Thoi_Gian;
        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
        public int ma_bai_dang { get; set; }
        [ForeignKey(nameof(ma_bai_dang))]
        public bai_dang? bai_dang { get; set; }
        public List<phuc_loi>? danh_sach_phuc_loi { get; set; }
        public List<tinh_thanh>? tinh_Thanh { get; set; }
    }
}