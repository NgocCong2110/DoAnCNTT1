using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DotNet_WEB.Class
{
    public class so_luong_nganh_nghe
    {
        public string? nganh_nghe { get; set;}
        public int so_luong { get; set; }
    }
}