using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class ung_tuyen
    {
        [Key]
        public int ma_ung_tuyen { get; set; }
        public int ma_viec { get; set; }
        [ForeignKey(nameof(ma_viec))]
        public viec_lam? viec_Lam { get; set; }
        public int ma_nguoi_tim_viec { get; set; }
        [ForeignKey(nameof(ma_nguoi_tim_viec))]
        public nguoi_tim_viec? nguoi_tim_viec { get; set; }
        public int? ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_Ty { get; set; }
        public int ma_cv { get; set; }
        [ForeignKey(nameof(ma_cv))]
        public cv_online_nguoi_tim_viec? cv_Online_Nguoi_Tim_Viec { get; set; } 
        public string? duong_dan_file_cv_upload { get; set; }
        public TrangThaiUngTuyen trang_thai { get; set; } = TrangThaiUngTuyen.dang_Cho;
        public TrangThaiDuyetUngTuyen trang_thai_duyet { get; set; } = TrangThaiDuyetUngTuyen.chua_Duyet;
        public DateTime ngay_ung_tuyen { get; set; } = DateTime.Now;
    }
}