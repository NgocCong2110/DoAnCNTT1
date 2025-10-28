using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNet_WEB.Class
{
    public class thong_tin_bai_dang
    {
        public bai_dang? bai_Dang {get; set;}
        public viec_lam? viec_Lam { get; set; }
        public List<int> phuc_Loi { get; set; }
    }
}