using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class trang_thai_thong_bao
    {
        [Key]
        public int ma_trang_thai_thong_bao { get; set; }

        public int ma_nguoi_nhan { get; set; }
        
        public int ma_thong_bao { get; set; }
        [ForeignKey(nameof(ma_thong_bao))]
        public thong_bao? thong_bao { get; set; }

        public bool trang_thai_doc { get; set; } = false;   

        public bool trang_thai_an { get; set; } = false; 

        public string? loai_nguoi_nhan { get; set; }  

        public DateTime ngay_tao { get; set; } = DateTime.Now;

        public DateTime ngay_cap_nhat { get; set; } = DateTime.Now;
    }
}