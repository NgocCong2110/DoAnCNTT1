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

        [HttpPost("laySoLuongNguoiDung")]
        public IActionResult laySoLuongNguoiDung()
        {
            var so_luong = Module_QTV.laySoLuongNguoiDung();
            if (so_luong != null)
            {
                return Ok(new { success = true, so_luong });
            }
            return Ok(new { success = false, message = "Lấy số lượng người dùng thất bại." });
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
            if (danh_sach_bai_dang != null)
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

            if (url != null )
            {
                return Ok(new { success = true, url });
            }
            return Ok(new { success = false });
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
            bool ungTuyen = ChucNang_WEB.ungTuyenCongViec(ung_Tuyen.ma_viec, ung_Tuyen.ma_cong_ty ?? 0, ung_Tuyen.ma_nguoi_tim_viec);
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
            var danh_sach = ChucNang_WEB.layDanhSachViecLamDuocQuanTam();
            if (danh_sach != null && danh_sach.Any())
            {
                return Ok(new { success = true, danh_sach });
            }
            return Ok(new { success = false, message = "Lay that bai " });
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
                viec_lam = ChucNang_WEB.layViecLamTheoBaiDang(bd.ma_bai_dang)
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









        //Module nguoi tim viec
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
                return Ok(new { success = true, message = "Lưu cv thành công",  duong_dan});
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

        [HttpPost("capNhatThongTinNguoiTimViec")]
        public IActionResult capNhatThongTinNguoiTimViec([FromBody] JObject du_lieu_ntv)
        {
            int ma_Nguoi_Tim_Viec = du_lieu_ntv["ma_nguoi_tim_viec"]?.Value<int>() ?? 0;
            if (ma_Nguoi_Tim_Viec == 0)
            {
                return Ok(new { success = false, message = "Không có mã người tìm việc" });
            }
            var field = du_lieu_ntv.Properties().FirstOrDefault(p => p.Name != "ma_nguoi_tim_viec");
            if (field == null)
            {
                return Ok(new { success = false, message = "Không có trường dữ liệu cần cập nhật" });
            }
            try
            {
                bool ket_Qua = Module_NTV.capNhatThongTinNguoiTimViec(ma_Nguoi_Tim_Viec, field.Name, field.Value.ToString());
                if (ket_Qua)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, message = "Cập nhật thông tin không thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}


