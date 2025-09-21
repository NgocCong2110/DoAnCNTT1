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
            if (nguoi_Dung.email == null)
            {
                return BadRequest(new { success = false, message = "Email không được để trống." });
            }
            bool ket_Qua = await XacThuc_ND.xacThucGmail(new MailAddress(nguoi_Dung.email));
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Email hợp lệ." });
            }
            return BadRequest(new { success = false, message = "Email không hợp lệ." });
        }


        [HttpPost("xacThucMaSoThue")]
        public async Task<IActionResult> kiemTraXacThucMaSoThue([FromBody] cong_ty cong_Ty)
        {
            if (string.IsNullOrWhiteSpace(cong_Ty.ma_so_thue))
            {
                return BadRequest(new { success = false, message = "Mã số thuế không được để trống" });
            }
            bool ketQua = await XacThuc_ND.kiemTraMaSoThue(cong_Ty.ma_so_thue);
            if (ketQua == true)
            {
                return Ok(new { success = true, message = "Mã số thuế hợp lệ." });
            }
            return BadRequest(new { success = false, message = "Mã số thuế không hợp lệ." });
        }

        [HttpPost("xacThucNguoiDung")]
        public IActionResult xacThucNguoiDung([FromBody] nguoi_dung nguoi_Dung)
        {
            if (nguoi_Dung.mat_khau == null || nguoi_Dung.email == null)
            {
                return BadRequest(new { success = false, message = "Mật khẩu và email không được để trống." });
            }
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(nguoi_Dung.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucNguoiDung(nguoi_Dung.email, matKhauDaMaHoa);
            var thong_tin = ChucNang_WEB.thongTinNguoiDungBangEmail(nguoi_Dung.email);
            if (ket_Qua)
            {
                return Ok(new { success = true, email = nguoi_Dung.email, thong_tin });
            }
            return Unauthorized(new { success = false, message = "Đăng nhập thất bại." });
        }
        [HttpPost("xacThucQuanTriVien")]
        public IActionResult xacThucQuanTriVien([FromBody] quan_tri quan_Tri)
        {
            if (quan_Tri.mat_khau == null || quan_Tri.email == null)
            {
                return BadRequest(new { success = false, message = "Mật khẩu và email không được để trống." });
            }
            string matKhauDaMaHoa = XacThuc_ND.maHoaMatKhau(quan_Tri.mat_khau);
            bool ket_Qua = XacThuc_ND.xacThucQuanTriVien(quan_Tri.email, matKhauDaMaHoa);
            var thong_tin = ChucNang_WEB.layDanhSachQuanTri(quan_Tri.email);
            if (ket_Qua)
            {
                return Ok(new
                {
                    success = true,
                    ma_quan_tri = thong_tin[0].ma_quan_tri,
                    ten_dang_nhap = thong_tin[0].ten_dang_nhap ?? "",
                    ho_ten = thong_tin[0].ho_ten,
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

        [HttpPost("themThongTinQuanTriVien")]
        public IActionResult themThongTinQuanTriVien([FromBody] quan_tri quan_Tri)
        {
            bool ket_Qua = Module_QTV.themQuanTriVien(quan_Tri);
            if (ket_Qua)
            {
                return Ok(new { success = true, message = "Thêm thông tin quản trị viên thành công." });
            }
            return BadRequest(new { success = false, message = "Thêm thông tin quản trị viên thất bại." });
        }






        // Module Quan Tri Vien
        // In danh sach nguoi dung
        [HttpPost("layDanhSachNguoiTimViec")]
        public IActionResult layDanhSachNguoiTimViec()
        {
            var danh_Sach = Module_QTV.layDanhSachNguoiTimViec();
            if (danh_Sach != null)
            {
                return Ok(new { success = true, danh_Sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách người tìm việc thất bại." });
        }

        [HttpPost("layDanhSachCongTy")]
        public IActionResult layDanhSachCongTy()
        {
            var danh_sach = Module_QTV.layDanhSachCongTy();
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách công ty thất bại." });
        }

        [HttpPost("laySoLuongNguoiDung")]
        public IActionResult laySoLuongNguoiDung()
        {
            var so_luong = Module_QTV.laySoLuongNguoiDung();
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return BadRequest(new { success = false, message = "Lấy số lượng người dùng thất bại." });
        }

        [HttpPost("layDanhSachViPham")]
        public IActionResult layDanhSachViPham()
        {
            var danh_sach = Module_QTV.layDanhSachViPham();
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách vi phạm thất bại." });
        }






        //Module cong ty
        [HttpPost("layDanhSachUngVien")]
        public IActionResult layDanhSachUngVien([FromBody] ung_tuyen ung_Tuyen)
        {
            var danh_sach = Module_CTY.layDanhSachUngVien(ung_Tuyen.ma_cong_ty ?? 0);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách ứng viên thất bại." });
        }

        [HttpPost("layBaiDangTheoIDCongTy")]
        public IActionResult layBaiDangTheoIDCongTy([FromBody] cong_ty cong_Ty)
        {
            var danh_sach_bai_dang = Module_CTY.layBaiDangTheoIDCongTy(cong_Ty.ma_cong_ty);
            if (danh_sach_bai_dang != null)
            {
                return Ok(new { success = true, danh_sach_bai_dang });
            }
            return BadRequest(new { success = false, message = "Lấy bài đăng theo ID công ty thất bại." });
        }

        [HttpPost("laySoLuongUngVien")]
        public IActionResult laySoLuongUngVien([FromBody] cong_ty cong_Ty)
        {
            var so_luong = Module_CTY.laySoLuongUngVien(cong_Ty.ma_cong_ty);
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return BadRequest(new { success = false, message = "Lấy số lượng ứng viên thất bại." });
        }








        //Chuc nang cua web
        [HttpPost("layDanhSachBaiDang")]
        public IActionResult LayDanhSachBaiDang()
        {
            var danh_sach_bai_dang = ChucNang_WEB.layDanhSachBaiDang();
            if (danh_sach_bai_dang == null || !danh_sach_bai_dang.Any())
            {
                return BadRequest(new { success = false, message = "Không lấy được danh sách bài đăng" });
            }

            var ket_qua = danh_sach_bai_dang.Select(bd => new
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

            return Ok(new { success = true, danh_sach_bai_dang = ket_qua });
        }


        [HttpPost("themBaiDangMoi")]
        public IActionResult themBaiDangMoi([FromBody] thong_tin_bai_dang thong_Tin)
        {
            if (thong_Tin == null || thong_Tin.bai_Dang == null || thong_Tin.viec_Lam == null)
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            var bai_d = thong_Tin.bai_Dang;
            var viec_l = thong_Tin.viec_Lam;
            bai_d.loai_bai = LoaiBai.tuyen_Dung;
            bai_d.trang_thai = TrangThaiBai.cong_Khai;
            bai_d.ngay_tao = DateTime.Now;
            bai_d.ngay_cap_nhat = DateTime.Now;
            bool luu_bai_moi = ChucNang_WEB.themBaiDangMoi(bai_d, viec_l);
            if (luu_bai_moi)
            {
                return Ok(new { success = true, message = "Thêm bài đăng thành công" });
            }
            return BadRequest(new { success = false, message = "Thêm bài đăng không thành công" });
        }

        [HttpPost("baoCaoBaiDang")]
        public IActionResult baoCaoBaiDang([FromBody] bai_dang_vi_pham bai_Dang_Vi_Pham)
        {
            bool baoCaoBaiDang = ChucNang_WEB.luuBaiDangViPham(bai_Dang_Vi_Pham);
            if (baoCaoBaiDang)
            {
                return Ok(new { success = true, message = "Báo cáo bài đăng thành công." });
            }
            return BadRequest(new { success = false, message = "Báo cáo bài đăng không thành công." });
        }

        [HttpPost("luuBaiDang")]
        public IActionResult luuBaiDang([FromBody] bai_dang_da_luu bai_Dang_Da_Luu)
        {
            bool luuBaiDang = ChucNang_WEB.luuBaiDang(bai_Dang_Da_Luu);
            if (luuBaiDang)
            {
                return Ok(new { success = true, message = "Lưu bài đăng thành công." });
            }
            return BadRequest(new { success = false, message = "Lưu bài đăng không thành công." });
        }

        [HttpPost("layDanhSachBaiDangDaLuu")]
        public IActionResult layDanhSachBaiDangDaLuu([FromBody] bai_dang_da_luu bai_Dang_Da_Luu)
        {
            var danh_sach = ChucNang_WEB.layDanhSachBaiDangDaLuu(bai_Dang_Da_Luu.ma_nguoi_luu);
            if (danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách bài đăng đã lưu thất bại." });
        }

        [HttpPost("ungTuyenCongViec")]
        public IActionResult ungTuyenCongViec([FromBody] ung_tuyen ung_Tuyen)
        {
            bool ungTuyen = ChucNang_WEB.ungTuyenCongViec(ung_Tuyen.ma_viec, ung_Tuyen.ma_cong_ty ?? 0, ung_Tuyen.ma_nguoi_tim_viec);
            if (ungTuyen)
            {
                return Ok(new { success = true, message = "Ứng tuyển công việc thành công." });
            }
            return BadRequest(new { success = false, message = "Ứng tuyển công việc không thành công." });
        }

        [HttpPost("layDanhSachThongBao")]
        public IActionResult layDanhSachThongBao([FromBody] thong_bao thong_Bao)
        {
            var danh_sach = ChucNang_WEB.layDanhSachThongBao();
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách thông báo thất bại" });
        }

        [HttpPost("chonThongBaoCoDinh")]
        public IActionResult layDanhSachThongBaoChoNguoiDung([FromBody] thong_bao thong_Bao)
        {
            var danh_sach = ChucNang_WEB.chonThongBaoCoDinh(Enum.Parse<LoaiThongBao>(thong_Bao.loai_thong_bao.ToString(), true));
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy danh sách thông báo thất bại" });
        }






        //Module nguoi tim viec
        [HttpPost("layDanhSachLichSuUngTuyen")]
        public IActionResult layDanhSachLichSuUngTuyen([FromBody] ung_tuyen ung_Tuyen)
        {
            var danh_sach = Module_NTV.layDanhSachUngTuyen(ung_Tuyen.ma_nguoi_tim_viec);
            if (danh_sach != null)
            {
                return Ok(new { success = true, danh_sach });
            }
            return BadRequest(new { success = false, message = "Lấy thông tin thất bại" });
        }
    }
}