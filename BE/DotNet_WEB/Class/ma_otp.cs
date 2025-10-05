using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class ma_otp
    {
        [Key]
        public int? ma_otp_id { get; set; }
        public string email { get; set; }
        public int ma_otp_gui_di { get; set; }
        public DateTime het_han_luc { get; set; }
        public bool da_su_dung { get; set; }
        public int so_lan_thu { get; set; }
    }
}