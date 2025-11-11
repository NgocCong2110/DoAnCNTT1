using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using DotNet_WEB.Module;
using DotNet_WEB.Class;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_tai_khoan_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_tramg_nguoi_tim_viec.chuc_nang_tai_khoan;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_phuc_loi_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_cong_ty.chuc_nang_mang_xa_hoi_cong_ty;
using DotNet_WEB.Module.chuc_nang.chuc_nang_trang_quan_tri.chuc_nang_tai_khoan;

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
            if (nguoi_Dung.email == null)
            {
                return Ok(new { success = false, message = "Email không được để trống." });
            }
            bool ket_Qua = await XacThuc_ND.xacThucGmail(new MailAddress(nguoi_Dung.email));
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Email hợp lệ." });
            }
            return Ok(new { success = false, message = "Email không hợp lệ." });
        }


        [HttpPost("xacThucMaSoThue")]
        public async Task<IActionResult> kiemTraXacThucMaSoThue([FromBody] cong_ty cong_Ty)
        {
            if (string.IsNullOrWhiteSpace(cong_Ty.ma_so_thue))
            {
                return Ok(new { success = false, message = "Mã số thuế không được để trống" });
            }
            bool ketQua = await XacThuc_ND.kiemTraMaSoThue(cong_Ty.ma_so_thue);
            if (ketQua == true)
            {
                return Ok(new { success = true, message = "Mã số thuế hợp lệ." });
            }
            return Ok(new { success = false, message = "Mã số thuế không hợp lệ." });
        }

        [HttpPost("xacThucNguoiDung")]
        public IActionResult xacThucNguoiDung([FromBody] nguoi_dung nguoi_Dung)
        {
            if (nguoi_Dung.mat_khau == null || nguoi_Dung.email == null)
            {
                return Ok(new { success = false, message = "Mật khẩu và email không được để trống." });
            }
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(nguoi_Dung.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucNguoiDung(nguoi_Dung.email, matKhauDaMaHoa);
            var thong_tin = ChucNang_WEB.thongTinNguoiDungBangEmail(nguoi_Dung.email);
            if (ket_Qua)
            {
                return Ok(new { success = true, email = nguoi_Dung.email, thong_tin });
            }
            return Ok(new { success = false, message = "Đăng nhập thất bại." });
        }
        [HttpPost("xacThucQuanTriVien")]
        public IActionResult xacThucQuanTriVien([FromBody] quan_tri quan_Tri)
        {
            if (quan_Tri.mat_khau == null || quan_Tri.email == null)
            {
                return Ok(new { success = false, message = "Mật khẩu và email không được để trống." });
            }
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(quan_Tri.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucQuanTriVien(quan_Tri.email, matKhauDaMaHoa);
            var thong_tin = ChucNang_WEB.layThongTinQuanTri(quan_Tri.email);
            if (ket_Qua)
            {
                return Ok(new
                {
                    success = true,
                    ma_quan_tri = thong_tin[0].ma_quan_tri,
                    ten_dang_nhap = thong_tin[0].ten_dang_nhap ?? "",
                    ho_ten = thong_tin[0].ho_ten,
                    email = thong_tin[0].email,
                    duong_dan_anh_dai_dien = thong_tin[0].duong_dan_anh_dai_dien
                });
            }
            return Ok(new { success = false, message = "Đăng nhập thất bại." });
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
            return Ok(new { success = false, message = "Thêm thông tin công ty thất bại." });
        }

        [HttpPost("themThongTinNguoiTimViec")]
        public IActionResult themThongTinNguoiTimViec([FromBody] nguoi_tim_viec nguoi_Tim_Viec)
        {
            bool ket_Qua = ChucNang_WEB.themNguoiTimViec(nguoi_Tim_Viec);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Thêm thông tin người tìm việc thành công." });
            }
            return Ok(new { success = false, message = "Thêm thông tin người tìm việc thất bại." });
        }

        [HttpPost("themThongTinQuanTriVien")]
        public IActionResult themThongTinQuanTriVien([FromBody] quan_tri quan_Tri)
        {
            bool ket_Qua = Module_QTV.themQuanTriVien(quan_Tri);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Thêm thông tin quản trị viên thành công." });
            }
            return Ok(new { success = false, message = "Thêm thông tin quản trị viên thất bại." });
        }













        // Module Quan Tri Vien
        // In danh sach nguoi dung
        [HttpPatch("capNhatThongTinQuanTri")]
        public IActionResult capNhatThongTinQuanTri([FromBody] thong_tin_truong_du_lieu_cap_nhat_quan_tri thong_tin)
        {
            bool ket_Qua = Module_QTV.capNhatThongTinQuanTri(thong_tin);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Cập nhật thông tin thất bại " });
        }

        [HttpPost("capNhatAnhDaiDienQuanTriVien")]
        public async Task<IActionResult> capNhatAnhDaiDienQuanTriVien([FromForm] IFormFile anh_dai_dien, [FromForm] int ma_quan_tri)
        {
            string url = await Module_QTV.capNhatAnhDaiDienQuanTriVien(anh_dai_dien, ma_quan_tri);
            if (!string.IsNullOrEmpty(url))
            {
                return Ok(new { success = true, url });
            }
            return Ok(new { success = false, message = "Cập nhật thất bại " });
        }

        [HttpPost("layDanhSachNguoiTimViec")]
        public IActionResult layDanhSachNguoiTimViec()
        {
            var danh_Sach = Module_QTV.layDanhSachNguoiTimViec();
            if (danh_Sach != null)
            {
                return Ok(new { success = true, danh_Sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách người tìm việc thất bại." });
        }

        [HttpPost("layDanhSachCongTy")]
        public IActionResult layDanhSachCongTy()
        {
            var danh_sach = Module_QTV.layDanhSachCongTy();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách công ty thất bại." });
        }

        [HttpPost("layToanBoDanhSachDanhGia")]
        public IActionResult layToanBoDanhSachDanhGia()
        {
            var danh_sach = Module_QTV.layToanBoDanhSachDanhGia();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách đánh giá thất bại." });
        }

        [HttpPost("capNhatTrangThaiDanhGia")]
        public IActionResult capNhatTrangThaiDanhGia([FromBody] danh_gia danh_Gia)
        {
            bool ket_Qua = Module_QTV.capNhatTrangThaiDanhGia(danh_Gia);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Cập nhật trạng thái thành công " });
            }
            return Ok(new { success = false, message = "Cập nhật trạng thái thất bại " });
        }

        [HttpPost("laySoLuongNguoiDung")]
        public IActionResult laySoLuongNguoiDung()
        {
            var so_luong = Module_QTV.laySoLuongNguoiDung();
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy số lượng thất bại." });
        }

        [HttpPost("laySoLuongCongTyVaNguoiTimViec")]
        public IActionResult laySoLuongCongTyVaNguoiTimViec()
        {
            var so_luong = Module_QTV.laySoLuongCongTyVaNguoiTimViec();
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy số lượng thất bại." });
        }

        [HttpPost("layDanhSachDangKyMoi")]
        public IActionResult layDanhSachDangKyMoi()
        {
            var danh_sach = Module_QTV.layDanhSachDangKyMoi();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thất bại." });
        }

        [HttpPost("laySoLuongTinTuyenDung")]
        public IActionResult laySoLuongTinTuyenDung()
        {
            var so_luong = Module_QTV.laySoLuongTinTuyenDung();
            if (so_luong != null && so_luong.Any())
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy danh sách thất bại." });
        }

        [HttpPost("layDanhSachTinTuyenDungMoi")]
        public IActionResult layDanhSachTinTuyenDungMoi()
        {
            var danh_sach = Module_QTV.layDanhSachTinTuyenDungMoi();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thất bại." });
        }

        [HttpPost("layDanhSachViPham")]
        public IActionResult layDanhSachViPham()
        {
            var danh_sach = Module_QTV.layDanhSachViPham();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách vi phạm thất bại." });
        }

        [HttpPost("layLichSuThanhToan")]
        public IActionResult layLichSuThanhToan()
        {
            var danh_sach = Module_QTV.layLichSuThanhToan();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thất bại." });
        }

        [HttpPost("taoDichVuMoi")]
        public IActionResult taoDichVuMoi([FromBody] dich_vu dich_Vu)
        {
            bool ket_qua = Module_QTV.taoDichVuMoi(dich_Vu);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Thêm dịch vụ mới thất bại" });
        }

        [HttpPost("xoaCongTy")]
        public IActionResult xoaCongTy([FromBody] int ma_cong_ty)
        {
            bool ket_qua = Module_QTV.xoaCongTy(ma_cong_ty);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Xóa công ty thất bại" });
        }

        [HttpPost("xoaNguoiTimViec")]
        public IActionResult xoaNguoiTimViec([FromBody] int ma_nguoi_tim_viec)
        {
            bool ket_qua = Module_QTV.xoaNguoiTimViec(ma_nguoi_tim_viec);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Xóa công ty thất bại" });
        }

        [HttpPost("guiThongBaoToiServer")]
        public IActionResult guiThongBaoToiServer([FromBody] thong_bao thong_Bao)
        {
            bool ket_qua = Module_QTV.guiThongBaoToiServer(thong_Bao);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Gửi thông báo thất bại" });
        }









        //Module cong ty
        [HttpPost("layThongTinCongTy")]
        public IActionResult layThongTinCongTy([FromBody] int ma_cong_ty)
        {
            var danh_sach = Module_CTY.layThongTinCongTy(ma_cong_ty);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false });
        }

        [HttpPost("layDanhSachViecLamCuaCongTy")]
        public IActionResult layDanhSachViecLamCuaCongTy([FromBody] int ma_cong_ty)
        {
            var danh_sach = Module_CTY.layDanhSachViecLamCuaCongTy(ma_cong_ty);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false });
        }

        [HttpPost("capNhatAnhBiaCongTy")]
        public async Task<IActionResult> capNhatAnhBiaCongTy([FromForm] IFormFile file, [FromForm] int ma_cong_ty)
        {
            if (ma_cong_ty == 0)
            {
                return Ok(new { success = false, message = "Không tồn tại công ty" });
            }
            string url = await Module_CTY.capNhatAnhBiaCongTy(file, ma_cong_ty);
            if (!string.IsNullOrEmpty(url))
            {
                return Ok(new { success = true, url });
            }
            return Ok(new { success = false, message = "Cập nhật ảnh bìa thất bại " });
        }

        [HttpPatch("capNhatThongTinCongTy")]
        public IActionResult capNhatThongTinCongTy([FromBody] thong_tin_truong_du_lieu_cap_nhat req)
        {
            bool ket_Qua = Module_CTY.capNhatThongTinCongTy(req);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("kiemTraMatKhauCongTy")]
        public IActionResult kiemTraMatKhauCongTy([FromBody] cong_ty req)
        {
            bool ket_Qua = Module_CTY.kiemTraMatKhauCongTy(req);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("capNhatMatKhauCongTy")]
        public IActionResult capNhatMatKhauCongTy([FromBody] cong_ty req)
        {
            bool ket_Qua = Module_CTY.capNhatMatKhauCongTy(req);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("layDanhSachUngVien")]
        public IActionResult layDanhSachUngVien([FromBody] ung_tuyen ung_Tuyen)
        {
            var danh_sach = Module_CTY.layDanhSachUngVien(ung_Tuyen.ma_cong_ty ?? 0);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách ứng viên thất bại." });
        }

        [HttpPost("layBaiDangTheoIDCongTy")]
        public IActionResult layBaiDangTheoIDCongTy([FromBody] cong_ty cong_Ty)
        {
            var danh_sach_bai_dang = Module_CTY.layBaiDangTheoIDCongTy(cong_Ty.ma_cong_ty);
            if (danh_sach_bai_dang != null && danh_sach_bai_dang.Any())
            {
                return Ok(new { success = true, danh_sach_bai_dang });
            }
            return Ok(new { success = false, message = "Lấy bài đăng theo ID công ty thất bại." });
        }

        [HttpPost("laySoLuongUngVien")]
        public IActionResult laySoLuongUngVien([FromBody] cong_ty cong_Ty)
        {
            var so_luong = Module_CTY.laySoLuongUngVien(cong_Ty.ma_cong_ty);
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy số lượng ứng viên thất bại." });
        }

        [HttpPost("laySoLuongBaiDangCuaCongTy")]
        public IActionResult laySoLuongBaiDangCuaCongTy([FromBody] cong_ty cong_Ty)
        {
            var so_luong = Module_CTY.laySoLuongBaiDangCuaCongTy(cong_Ty.ma_cong_ty);
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy số lượng bài đăng thất bại." });
        }

        [HttpPost("guiThuMoiPhongVan")]
        public IActionResult guiThuMoiPhongVan([FromBody] thong_tin_phong_van ttpv)
        {
            bool ket_qua = Module_CTY.guiThuMoiPhongVan(ttpv);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = " Gửi thư mời không thành công " });
        }


        [HttpPost("tuChoiUngVien")]
        public IActionResult tuChoiUngVien([FromBody] ung_tuyen ung_Tuyen)
        {
            bool ket_qua = Module_CTY.tuChoiUngVien(ung_Tuyen);
            if (ket_qua)
            {
                return Ok(new { success = true, message = "Từ chối ứng viên thành công" });
            }
            return Ok(new { success = false, message = "Có lỗi trong quá trình từ chối ứng viên" });
        }

        [HttpPost("xoaUngVien")]
        public IActionResult xoaUngVien([FromBody] ung_tuyen ung_Tuyen)
        {
            bool ket_qua = Module_CTY.xoaUngVien(ung_Tuyen);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = " Có lỗi trong quá trình " });
        }

        [HttpPost("layDanhSachDichVu")]
        public IActionResult layDanhSachDichVu()
        {
            var danh_sach = Module_CTY.layDanhSachDichVu();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = " Không lấy được danh sách dịch vụ " });
        }

        [HttpPost("capNhatLogoCongTy")]
        public async Task<IActionResult> capNhatLogoCongTy([FromForm] IFormFile logo, [FromForm] int ma_cong_ty)
        {
            if (logo == null || logo.Length == 0)
            {
                return Ok(new { success = false, message = "Không thấy đường dẫn file" });
            }

            string url = await Module_CTY.capNhatLogoCongTy(logo, ma_cong_ty);

            if (url != null)
            {
                return Ok(new { success = true, url });
            }
            return Ok(new { success = false });
        }

        [HttpPost("capNhatPhucLoiCongTy")]
        public IActionResult capNhatPhucLoiCongTy([FromBody] phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            if (phuc_Loi_Cong_Ty.ma_cong_ty == 0)
            {
                return Ok(new { success = false, message = "Không tìm thấy công ty " });
            }
            bool ket_Qua = Module_CTY.capNhatPhucLoiCongTy(phuc_Loi_Cong_Ty);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Cập nhật phúc lợi thất bại" });
        }

        [HttpPost("xoaPhucLoiCongTy")]
        public IActionResult xoaPhucLoiCongTy([FromBody] phuc_loi_cong_ty_cap_nhat phuc_Loi_Cong_Ty)
        {
            if (phuc_Loi_Cong_Ty.ma_cong_ty == 0)
            {
                return Ok(new { success = false, message = "Không tìm thấy công ty " });
            }
            bool ket_Qua = Module_CTY.xoaPhucLoiCongTy(phuc_Loi_Cong_Ty);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Xóa phúc lợi thất bại" });
        }

        [HttpPost("capNhatLienKetMangXaHoi")]
        public IActionResult capNhatLienKetMangXaHoi([FromBody] mang_xa_hoi_cong_ty_cap_nhat mang_Xa_Hoi_Cong_Ty_Cap_Nhat)
        {
            if (mang_Xa_Hoi_Cong_Ty_Cap_Nhat.ma_cong_ty == 0)
            {
                return Ok(new { success = false, message = "Không tìm thấy công ty " });
            }
            bool ket_Qua = Module_CTY.capNhatLienKetMangXaHoi(mang_Xa_Hoi_Cong_Ty_Cap_Nhat);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Cập nhật phúc lợi thất bại" });
        }










        //Chuc nang cua web
        [HttpPost("kiemTraTaiKhoanDangKy")]
        public IActionResult kiemTraTaiKhoanDangKy([FromBody] nguoi_dung nguoi_Dung)
        {
            bool ket_qua = ChucNang_WEB.kiemTraTaiKhoanDangKy(nguoi_Dung);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Email đã tồn tại" });
        }

        [HttpPost("layNganhNgheNoiBat")]
        public IActionResult layNganhNgheNoiBat()
        {
            var danh_sach = ChucNang_WEB.layNganhNgheNoiBat();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Không lấy được danh sách ngành nghề nổi bật" });
        }

        [HttpPost("layDanhSachNganhNghe")]
        public IActionResult layDanhSachNganhNghe()
        {
            var danh_sach = ChucNang_WEB.layDanhSachNganhNghe();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Không lấy được danh sách ngành nghề" });
        }

        [HttpPost("layToaDoCongTy")]
        public async Task<IActionResult> layToaDoCongTy([FromBody] string vi_tri)
        {
            string apiKey = "pk.dee0f3e5396893fd514aca66dd4c0790";
            var url = $"https://us1.locationiq.com/v1/search?key={apiKey}&q={Uri.EscapeDataString(vi_tri)}&format=json";
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetAsync(url);
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                var json = JArray.Parse(content);

                if (json.Count > 0)
                {
                    var lat = json[0]["lat"].ToString();
                    var lng = json[0]["lon"].ToString();
                    return Ok(new { lat, lng });
                }
            }
            return NotFound(new { message = "Không tìm thấy địa chỉ" });
        }

        [HttpPost("doiMatKhauMoi")]
        public IActionResult doiMatKhauMoi([FromBody] nguoi_dung nguoi_Dung)
        {
            if (nguoi_Dung == null || string.IsNullOrEmpty(nguoi_Dung.email) || string.IsNullOrEmpty(nguoi_Dung.mat_khau))
            {
                return Ok(new { success = false, message = "Thông tin không hợp lệ" });
            }

            bool ket_qua = ChucNang_WEB.doiMatKhauMoi(nguoi_Dung.email, nguoi_Dung.mat_khau);

            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            else
            {
                bool ket_qua_2 = ChucNang_WEB.doiMatKhauQuanTri(nguoi_Dung.email, nguoi_Dung.mat_khau);
                if (ket_qua_2)
                {
                    return Ok(new { success = true });
                }
            }
            return Ok(new { success = false });
        }

        [HttpPost("layDanhSachBaiDang")]
        public IActionResult LayDanhSachBaiDang()
        {
            var danh_sach = ChucNang_WEB.layDanhSachBaiDang();
            if (danh_sach == null || !danh_sach.Any())
            {
                return Ok(new { success = false, message = "Không lấy được danh sách bài đăng" });
            }

            var ket_qua = danh_sach.Select(bd => new
            {
                bd.ma_bai_dang,
                bd.ma_nguoi_dang,
                bd.ten_nguoi_dang,
                bd.tieu_de,
                bd.noi_dung,
                bd.loai_bai,
                bd.trang_thai,
                bd.ngay_tao,
                bd.ngay_cap_nhat,
                viec_lam = ChucNang_WEB.layViecLamTheoBaiDang(bd.ma_bai_dang)
            });

            return Ok(new { success = true, danh_sach = ket_qua });
        }


        [HttpPost("themBaiDangMoi")]
        public IActionResult themBaiDangMoi([FromBody] thong_tin_bai_dang thong_Tin)
        {
            if (thong_Tin == null || thong_Tin.bai_Dang == null || thong_Tin.viec_Lam == null)
                return Ok(new { success = false, message = "Dữ liệu không hợp lệ" });
            var bai_d = thong_Tin.bai_Dang;
            var viec_l = thong_Tin.viec_Lam;
            var phuc_l = thong_Tin.phuc_Loi;
            bool luu_bai_moi = ChucNang_WEB.themBaiDangMoi(bai_d, viec_l, phuc_l);
            if (luu_bai_moi)
            {
                return Ok(new { success = true, message = "Thêm bài đăng thành công" });
            }
            return Ok(new { success = false, message = "Thêm bài đăng không thành công" });
        }

        [HttpPost("baoCaoBaiDang")]
        public IActionResult baoCaoBaiDang([FromBody] bai_dang_vi_pham bai_Dang_Vi_Pham)
        {
            bool bao_cao_bai_dang = ChucNang_WEB.luuBaiDangViPham(bai_Dang_Vi_Pham);
            if (bao_cao_bai_dang)
            {
                return Ok(new { success = true, message = "Báo cáo bài đăng thành công." });
            }
            return Ok(new { success = false, message = "Báo cáo bài đăng không thành công." });
        }

        [HttpPost("xoaBaiDang")]
        public IActionResult xoaBaiDang([FromBody] bai_dang bai_Dang)
        {
            bool xoa_bai_dang = ChucNang_WEB.xoaBaiDang(bai_Dang.ma_bai_dang);
            if (xoa_bai_dang)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("luuBaiDang")]
        public IActionResult luuBaiDang([FromBody] bai_dang_da_luu bai_Dang_Da_Luu)
        {
            bool luuBaiDang = ChucNang_WEB.luuBaiDang(bai_Dang_Da_Luu);
            if (luuBaiDang)
            {
                return Ok(new { success = true, message = "Lưu bài đăng thành công." });
            }
            return Ok(new { success = false, message = "Lưu bài đăng không thành công." });
        }

        [HttpPost("huyLuuBaiDang")]
        public IActionResult huyLuuBaiDang([FromBody] bai_dang_da_luu bai_Dang_Da_Luu)
        {
            bool ket_Qua = ChucNang_WEB.huyLuuBaiDang(bai_Dang_Da_Luu);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Hủy lưu bài đăng thành công." });
            }
            return Ok(new { success = false, message = "Hủy lưu bài đăng không thành công." });
        }

        [HttpPost("layDanhSachBaiDangDaLuu")]
        public IActionResult layDanhSachBaiDangDaLuu([FromBody] bai_dang_da_luu bai_Dang_Da_Luu)
        {
            var danh_sach = ChucNang_WEB.layDanhSachBaiDangDaLuu(bai_Dang_Da_Luu.ma_nguoi_luu);
            if (danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách bài đăng đã lưu thất bại." });
        }

        [HttpPost("kiemTraUngTuyen")]
        public IActionResult kiemTraUngTuyen([FromBody] ung_tuyen ung_Tuyen)
        {
            bool ktra = ChucNang_WEB.kiemTraUngTuyen(ung_Tuyen);
            if (ktra)
            {
                return Ok(new { success = false, message = "Đã ứng tuyển công việc rồi." });
            }
            return Ok(new { success = true });
        }

        [HttpPost("ungTuyenCongViec")]
        public IActionResult ungTuyenCongViec([FromBody] ung_tuyen ung_Tuyen)
        {
            bool ungTuyen = ChucNang_WEB.ungTuyenCongViec(ung_Tuyen);
            if (ungTuyen)
            {
                return Ok(new { success = true, message = "Ứng tuyển công việc thành công." });
            }
            return Ok(new { success = false, message = "Ứng tuyển công việc không thành công." });
        }

        [HttpPost("ungTuyenCongViecUploadCV")]
        public async Task<IActionResult> ungTuyenCongViecUploadCV([FromForm] int ma_viec, [FromForm] int ma_cong_ty, [FromForm] int ma_nguoi_tim_viec, [FromForm] IFormFile duong_dan_file_cv_upload)
        {
            if (duong_dan_file_cv_upload == null || duong_dan_file_cv_upload.Length == 0)
            {
                return BadRequest(new { success = false, message = "Chưa có file CV được tải lên." });
            }

            bool ungTuyen = await ChucNang_WEB.ungTuyenCongViecUploadCV(ma_viec, ma_cong_ty, ma_nguoi_tim_viec, duong_dan_file_cv_upload);

            if (ungTuyen)
            {
                return Ok(new { success = true, message = "Ứng tuyển công việc thành công." });
            }

            return Ok(new { success = false, message = "Ứng tuyển công việc không thành công." });
        }

        [HttpPost("layDanhSachThongBao")]
        public IActionResult layDanhSachThongBao([FromBody] thong_bao_kieu_nguoi_dung thong_Bao_Kieu_Nguoi_Dung)
        {
            var danh_sach = ChucNang_WEB.layDanhSachThongBao(thong_Bao_Kieu_Nguoi_Dung);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thông báo thất bại" });
        }

        [HttpPost("layDanhSachDanhGia")]
        public IActionResult layDanhSachDanhGia()
        {
            var danh_sach = ChucNang_WEB.layDanhSachDanhGia();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false });
        }

        [HttpPost("themDanhGia")]
        public IActionResult themDanhGia([FromBody] danh_gia danh_Gia)
        {
            bool ket_Qua = ChucNang_WEB.themDanhGiaMoi(danh_Gia);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("chonThongBaoCoDinh")]
        public IActionResult layDanhSachThongBaoChoNguoiDung([FromBody] thong_bao thong_Bao)
        {
            var danh_sach = ChucNang_WEB.chonThongBaoCoDinh(thong_Bao);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thông báo thất bại" });
        }

        [HttpPost("deXuatViecLamSelector")]
        public IActionResult deXuatViecLamSelector([FromBody] viec_lam viec_Lam)
        {
            var danh_sach = ChucNang_WEB.deXuatViecLamSelector(viec_Lam);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Không lấy được danh sách" });
        }

        [HttpPost("layDanhSachViecLamDuocQuanTam")]
        public IActionResult layDanhSachViecLamDuocQuanTam()
        {
            try
            {
                var danh_sach = ChucNang_WEB.layDanhSachViecLamDuocQuanTam();
                if (danh_sach != null && danh_sach.Any())
                {
                    return Ok(new { success = true, danh_sach });
                }
                return Ok(new { success = false, message = "Lay that bai " });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("layChiTietViecLam")]
        public IActionResult LayChiTietViecLam([FromBody] bai_dang bai_Dang)
        {
            var danh_sach_bai_dang = ChucNang_WEB.layBaiDangTheoMa(bai_Dang.ma_bai_dang);
            var bai_dang = danh_sach_bai_dang.FirstOrDefault();

            if (bai_dang == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy bài đăng" });
            }

            var viec_Lam = ChucNang_WEB.layViecLamTheoBaiDang(bai_Dang.ma_bai_dang);

            var chi_tiet = new
            {
                bai_dang.ma_bai_dang,
                bai_dang.ma_nguoi_dang,
                bai_dang.ten_nguoi_dang,
                bai_dang.tieu_de,
                bai_dang.noi_dung,
                bai_dang.loai_bai,
                bai_dang.trang_thai,
                bai_dang.ngay_tao,
                bai_dang.ngay_cap_nhat,
                viec_lam = viec_Lam
            };

            return Ok(new { success = true, chi_tiet });
        }

        [HttpPost("layDanhSachViecLam")]
        public IActionResult layDanhSachViecLam()
        {
            var danh_sach = ChucNang_WEB.layDanhSachBaiDang();
            if (danh_sach == null || !danh_sach.Any())
            {
                return Ok(new { success = false, message = "Không lấy được danh sách bài đăng" });
            }

            var ket_qua = danh_sach.Select(bd => new
            {
                bd.ma_bai_dang,
                bd.ma_nguoi_dang,
                bd.ten_nguoi_dang,
                bd.tieu_de,
                bd.noi_dung,
                bd.loai_bai,
                bd.trang_thai,
                bd.ngay_tao,
                bd.ngay_cap_nhat,
                logo = bd.cong_Ty?.logo,
                viec_Lam = ChucNang_WEB.layViecLamTheoBaiDang(bd.ma_bai_dang)
            });

            return Ok(new { success = true, danh_sach = ket_qua });
        }

        [HttpPost("duaRaDeXuat")]
        public IActionResult duaRaDeXuat([FromBody] string tu_khoa_tim_kiem)
        {
            var thong_tin = ChucNang_WEB.duaRaDanhSachDeXuat(tu_khoa_tim_kiem);
            if (thong_tin != null && thong_tin.Any())
            {
                return Ok(new { success = true, thong_tin });
            }
            return Ok(new { success = false, message = "Không có thông tin việc làm" });
        }



        [HttpPost("guiYeuCauOTP")]
        public async Task<IActionResult> guiYeuCauOTP([FromBody] string email_yeu_cau)
        {
            bool ket_qua = ChucNang_WEB.kiemTraOTPTonTai(email_yeu_cau);
            if (!ket_qua)
            {
                return Ok(new { success = false, message = "Đã tồn tại OTP hãy sử dụng OTP cũ" });

            }
            int otp = ChucNang_WEB.taoOTPMoi(email_yeu_cau);
            if (otp >= 100000 && otp <= 999999)
            {
                bool kq_gui_otp = await XacThuc_ND.guiMaOTP(otp, email_yeu_cau);
                if (kq_gui_otp)
                {
                    return Ok(new { success = true });
                }
            }
            return Ok(new { success = false, message = "Tạo otp không thành công " });
        }

        [HttpPost("xacNhanOTP")]
        public IActionResult xacNhanOTP([FromBody] ma_otp ma_Otp)
        {
            bool ket_qua = XacThuc_ND.xacNhanOTP(ma_Otp);
            if (ket_qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "OTP sai" });
        }

        [HttpPost("goiYTuKhoa")]
        public IActionResult goiYTuKhoa([FromBody] string tu_khoa)
        {
            var danh_sach = ChucNang_WEB.goiYTuKhoa(tu_khoa);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false });
        }
        [HttpPost("layDanhSachViecLamCungCongTy")]
        public IActionResult layDanhSachViecLamCungCongTy([FromBody] bai_dang bai_Dang)
        {
            if (bai_Dang.ma_bai_dang == 0 || bai_Dang.ma_nguoi_dang == 0)
            {
                return Ok(new { success = false, message = " Thiếu dữ liệu" });
            }
            var danh_sach = ChucNang_WEB.layDanhSachViecLamLienQuan(bai_Dang);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy việc làm liên quan thất bại " });
        }









        //Module nguoi tim viec
        [HttpPost("kiemTraMatKhauNguoiTimViec")]
        public IActionResult kiemTraMatKhauNguoiTimViec([FromBody] nguoi_tim_viec req)
        {
            bool ket_Qua = Module_NTV.kiemTraMatKhauNguoiTimViec(req);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }

        [HttpPost("capNhatMatKhauNguoiTimViec")]
        public IActionResult capNhatMatKhauNguoiTimViec([FromBody] nguoi_tim_viec req)
        {
            bool ket_Qua = Module_NTV.capNhatMatKhauNguoiTimViec(req);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false });
        }
        [HttpPost("layDanhSachLichSuUngTuyen")]
        public IActionResult layDanhSachLichSuUngTuyen([FromBody] ung_tuyen ung_Tuyen)
        {
            var danh_sach = Module_NTV.layDanhSachUngTuyen(ung_Tuyen.ma_nguoi_tim_viec);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy thông tin thất bại" });
        }

        [HttpPost("dangTaiCV")]
        public async Task<IActionResult> dangTaiCV([FromForm] IFormFile cv_file, [FromForm] int ma_Nguoi_Tim_Viec)
        {
            var result = await Module_NTV.dangTaiCV(cv_file, ma_Nguoi_Tim_Viec);

            if (result)
            {
                return Ok(new { success = true, message = "Upload thành công" });
            }
            return Ok(new { success = false, message = "Chưa có file CV" });
        }

        [HttpPost("luuCV")]
        public async Task<IActionResult> luuCV([FromBody] cv_online_nguoi_tim_viec cv_Online_Nguoi_Tim_Viec)
        {
            string duong_dan = await ChucNang_WEB.luuCVOnLine(cv_Online_Nguoi_Tim_Viec);
            if (duong_dan != null && !string.IsNullOrEmpty(duong_dan))
            {
                return Ok(new { success = true, message = "Lưu cv thành công", duong_dan });
            }
            return Ok(new { success = false, message = "Chưa có file CV" });
        }

        [HttpPost("layDanhSachCV")]
        public IActionResult layDanhSachCV([FromBody] cv_nguoi_tim_viec cv_ntv)
        {
            var danh_sach = Module_NTV.layDanhSachCV(cv_ntv.ma_nguoi_tim_viec);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Chưa có file CV" });
        }

        [HttpPost("layDanhSachCVOnlineNguoiTimViec")]
        public IActionResult layDanhSachCVOnlineNguoiTimViec([FromBody] int ma_nguoi_tim_viec)
        {
            var danh_sach = Module_NTV.layDanhSachCVOnlineNguoiTimViec(ma_nguoi_tim_viec);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lỗi không lấy được CV" });
        }

        [HttpPost("xoaCVNguoiTimViec")]
        public IActionResult xoaCVNguoiTimViec([FromBody] cv_online_nguoi_tim_viec cv_Online_Nguoi_Tim_Viec)
        {
            bool ket_Qua = Module_NTV.xoaCVNguoiTimViec(cv_Online_Nguoi_Tim_Viec);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Xóa CV thành công " });
            }
            return Ok(new { success = false, message = "Xóa cv không thành công " });
        }

        [HttpPatch("capNhatThongTinNguoiTimViec")]
        public IActionResult capNhatThongTinNguoiTimViec([FromBody] thong_tin_truong_du_lieu_cap_nhat_ntv req)
        {
            if (req.truong == null || req.ma_nguoi_tim_viec == 0 || req.gia_tri == null)
            {
                return Ok(new { success = false, message = "Thiếu dữ liệu" });
            }
            bool ket_Qua = Module_NTV.capNhatThongTinNguoiTimViec(req);

            if (ket_Qua)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false, message = "Cập nhật dữ liệu thất bại" });
        }

        [HttpPost("capNhatAnhDaiDienNguoiTimViec")]
        public async Task<IActionResult> capNhatAnhDaiDienNguoiTimViec([FromForm] IFormFile anh_dai_dien, [FromForm] int ma_nguoi_tim_viec)
        {
            Console.WriteLine(ma_nguoi_tim_viec);
            string url = await Module_NTV.capNhatAnhDaiDienNguoiTimViec(anh_dai_dien, ma_nguoi_tim_viec);
            if (!string.IsNullOrEmpty(url))
            {
                return Ok(new { success = true, url });
            }
            return Ok(new { success = false, message = "Cập nhật thất bại " });
        }

        [HttpPost("layDanhSachChungChi")]
        public IActionResult layDanhSachChungChi([FromBody] int ma_nguoi_tim_viec)
        {
            if (ma_nguoi_tim_viec == 0)
            {
                return Ok(new { success = false, message = "Không tìm thấy người tìm việc " });
            }
            var danh_sach = Module_NTV.layDanhSachChungChi(ma_nguoi_tim_viec);
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lấy danh sách thất bại " });
        }

        [HttpPost("dangTaiChungChi")]
        public async Task<IActionResult> dangTaiChungChi([FromForm] int ma_nguoi_tim_viec, [FromForm] string ten_chung_chi, [FromForm] string don_vi_cap, [FromForm] DateTime ngay_cap, [FromForm] DateTime ngay_het_han, [FromForm] IFormFile ten_tep)
        {
            if (ma_nguoi_tim_viec == 0 || string.IsNullOrEmpty(ten_chung_chi) || string.IsNullOrEmpty(don_vi_cap) || ten_tep == null || ten_tep.Length == 0)
            {
                return Ok(new { success = false, message = "Không đủ dữ liệu chứng chỉ " });
            }
            bool ket_Qua = await Module_NTV.dangTaiChungChi(ma_nguoi_tim_viec, ten_chung_chi, don_vi_cap, ngay_cap, ngay_het_han, ten_tep);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Đăng tải chứng chỉ thất bại " });
        }

        [HttpPost("xoaChungChi")]
        public async Task<IActionResult> xoaChungChi([FromBody] chung_chi chung_Chi)
        {
            if (chung_Chi.ma_nguoi_tim_viec == 0 || chung_Chi.ma_nguoi_tim_viec == 0)
            {
                return Ok(new { success = false, message = " Thiếu dữ liệu " });
            }
            bool ket_Qua = await Module_NTV.xoaChungChi(chung_Chi);
            if (ket_Qua)
            {
                return Ok(new { success = true });
            }
            return Ok(new { success = false, message = "Xóa chứng chỉ thất bại " });
        }
    }
}


