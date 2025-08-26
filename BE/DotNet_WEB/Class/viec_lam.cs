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
        public cong_ty? cong_ty { get; set; }
        public string? tieu_de { get; set; }
        public string? mo_ta { get; set; }
        public string? yeu_cau { get; set; }
        public string? muc_luong { get; set; }
        public string? dia_diem { get; set; }
        public LoaiHinhViecLam loai_hinh { get; set; } = LoaiHinhViecLam.toan_Thoi_Gian;
        public TrangThaiViecLam trang_thai { get; set; } = TrangThaiViecLam.dang_Hien_Thi;

        public DateTime ngay_tao { get; set; } = DateTime.Now;
        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}