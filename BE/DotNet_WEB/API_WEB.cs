using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using DotNet_WEB.Module;
using DotNet_WEB.Class;

namespace DotNet_WEB
{
    [ApiController]
    [Route("api/[controller]")]
    public class API_WEB : ControllerBase
    {

        //Xac Thuc Nguoi Dung

        [HttpPost("xacThucGmail")]
        public async Task<IActionResult> kiemTraXacThuc([FromBody] nguoi_dung nguoi_Dung)
        {
            bool ket_Qua = await XacThuc_ND.xacThucGmail(new MailAddress(nguoi_Dung.email));
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Email hợp lệ." });
            }
            return BadRequest(new { success = false, message = "Email không hợp lệ." });
        }


        [HttpPost("xacThucMaSoThue")]
        public IActionResult kiemTraXacThucMaSoThue([FromBody] string maSoThue)
        {
            bool ketQua = XacThuc_ND.kiemTraMaSoThue(maSoThue);
            if (ketQua == true)
            {
                return Ok(new { success = true, message = "Mã số thuế hợp lệ." });
            }
            return BadRequest(new { success = false, message = "Mã số thuế không hợp lệ." });
        }

        [HttpPost("xacThucNguoiDung")]
        public IActionResult xacThucNguoiDung([FromBody] nguoi_dung nguoi_Dung)
        {
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(nguoi_Dung.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucNguoiDung(nguoi_Dung.email, matKhauDaMaHoa);
            if (ket_Qua)
            {
                return Ok(new { success = true, email = nguoi_Dung.email, vai_tro = nguoi_Dung.loai_nguoi_dung, ten_dang_nhap = nguoi_Dung.ten_dang_nhap });
            }
            return Unauthorized(new { success = false, message = "Đăng nhập thất bại." });
        }
        [HttpPost("xacThucQuanTriVien")]
        public IActionResult xacThucQuanTriVien([FromBody] quan_tri quan_Tri)
        {
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(quan_Tri.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucQuanTriVien(quan_Tri.email, matKhauDaMaHoa);
            var thong_tin = ChucNang_WEB.layThongTinQuanTri(quan_Tri.email);
            if (ket_Qua)
            {
                return Ok(new
                {
                    success = true,
                    ten_dang_nhap = thong_tin[0].ten_dang_nhap ?? "",
                    email = thong_tin[0].email,
                    vai_tro = thong_tin[0].vai_tro
                });
            }
            return Unauthorized(new { success = false, message = "Đăng nhập thất bại." });
        }

        //Them Thong Tin Nguoi Dung

        [HttpPost("themThongTinCongTy")]
        public IActionResult themThongTinCongTy([FromBody] cong_ty cong_Ty)
        {
            bool ket_Qua = ChucNang_WEB.themCongTy(cong_Ty);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Thêm thông tin công ty thành công." });
            }
            return BadRequest(new { success = false, message = "Thêm thông tin công ty thất bại." });
        }
        [HttpPost("themThongTinNguoiTimViec")]
        public IActionResult themThongTinNguoiTimViec([FromBody] nguoi_tim_viec nguoi_Tim_Viec)
        {
            bool ket_Qua = ChucNang_WEB.themNguoiTimViec(nguoi_Tim_Viec);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Thêm thông tin người tìm việc thành công." });
            }
            return BadRequest(new { success = false, message = "Thêm thông tin người tìm việc thất bại." });
        }
    }
}