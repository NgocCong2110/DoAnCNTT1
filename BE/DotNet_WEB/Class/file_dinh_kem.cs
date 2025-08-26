using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class file_dinh_kem
    {
        [Key]
        public int ma_file { get; set; }
        public int ma_bai_dang { get; set; }
        [ForeignKey(nameof(ma_bai_dang))]
        public bai_dang? bai_dang { get; set; }
        public string? ten_file { get; set; }
        public string? duong_dan { get; set; }
        public string? loai_file { get; set; }
        public DateTime ngay_tao { get; set; } = DateTime.Now;
    }
}