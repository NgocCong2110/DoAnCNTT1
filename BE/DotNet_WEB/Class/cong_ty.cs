using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class cong_ty
    {
        [Key]
        public int ma_cong_ty { get; set; }
        public string? ten_cong_ty { get; set; }
        public string? ten_dn_cong_ty { get; set; }
        public string? mat_khau_dn_cong_ty { get; set; }
        public string? nguoi_dai_dien { get; set; }
        public string? ma_so_thue { get; set; }
        public string? dia_chi { get; set; }
        public string? dien_thoai { get; set; }
        public string? email { get; set; }
        public string? website { get; set; }
        public string? logo { get; set; }
        public string? mo_ta { get; set; }
        public LoaiHinhCongTy loai_hinh_cong_ty { get; set; } = LoaiHinhCongTy.congty_CoPhan;
        public string? quy_mo { get; set; }
        public int? nam_thanh_lap { get; set; }
        public string? anh_bia { get; set; }
        public TrangThaiCongTy trang_thai { get; set; } = TrangThaiCongTy.hoat_Dong;
        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}