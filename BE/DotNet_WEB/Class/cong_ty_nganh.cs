using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class cong_ty_nganh
    {
        public int ma_cong_ty { get; set; }
        [ForeignKey(nameof(ma_cong_ty))]
        public cong_ty? cong_ty { get; set; }
        public int ma_nganh { get; set; }
        [ForeignKey(nameof(ma_nganh))]
        public nganh_nghe? nganh { get; set; }
    }
}