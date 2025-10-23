using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thong_tin_phong_van
    {
        public int ma_viec { get; set; }
        public int ma_cong_ty { get; set; }
        public int ma_nguoi_tim_viec { get; set; }
        public DateTime thoi_gian { get; set; }
        public string dia_diem { get; set; } = "";
        public string noi_dung { get; set; } = "";
        public string trang_thai { get; set; } = "";
    }
}