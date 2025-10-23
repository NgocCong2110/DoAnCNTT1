using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace DotNet_WEB.Class
{
    public enum LoaiNguoiDung
    {
        None = 0,
        [Display(Name = "Người Tìm Việc")]
        nguoi_Tim_Viec = 1,
        [Display(Name = "Công Ty")]
        cong_Ty = 2,
    }

    public enum GioiTinh
    {
        None = 0,
        [Display(Name = "Nam")]
        nam = 1,
        [Display(Name = "Nữ")]
        nu = 2
    }

    public enum TrinhDoHocVan
    {
        None = 0,
        [Display(Name = "Trung Học")]
        trung_Hoc = 1,
        [Display(Name = "Cao Đẳng")]
        cao_Dang = 2,
        [Display(Name = "Đại Học")]
        dai_Hoc = 3,
        [Display(Name = "Tốt nghiệp")]
        tot_Nghiep = 4,
        [Display(Name = "Khác")]
        khac = 5
    }

    public enum LoaiHinhCongTy
    {
        None = 0,
        [Display(Name = "Công Ty TNHH")]
        congty_TNHH = 1,
        [Display(Name = "Công Ty Cổ Phần")]
        congty_CoPhan = 2,
        [Display(Name = "Doanh Nghiệp Tư Nhân")]
        doanhnghiep_TuNhan = 3,
        [Display(Name = "Công Ty Hợp Danh")]
        congty_HopDanh = 4,
        [Display(Name = "Khác")]
        Khac = 5
    }

    public enum LoaiHinhViecLam
    {
        None = 0,
        [Display(Name = "Toàn Thời Gian")]
        toan_Thoi_Gian = 1,
        [Display(Name = "Bán Thời Gian")]
        ban_Thoi_Gian = 2,
        [Display(Name = "Thực Tập")]
        thuc_Tap = 3,
        [Display(Name = "Tự Do")]
        tu_Do = 4
    }

    public enum TrangThaiDanhGia
    {
        None = 0,
        [Display(Name = "Đang hiển thị")]
        dang_Hien_Thi = 1,
        [Display(Name = "Chưa hiển thị")]
        chua_Hien_Thi = 2
    }

    public enum TrangThaiUngTuyen
    {
        None = 0,
        [Display(Name = "Đang Chờ")]
        dang_Cho = 1,
        [Display(Name = "Chấp Nhận")]
        chap_Nhan = 2,
        [Display(Name = "Từ Chối")]
        tu_Choi = 3
    }

    public enum LoaiBai
    {
        None = 0,
        [Display(Name = "Tuyển Dụng")]
        tuyen_Dung = 1,
        [Display(Name = "Tìm Việc")]
        tim_Viec = 2
    }

    public enum TrangThaiBai
    {
        None = 0,
        [Display(Name = "Công Khai")]
        cong_Khai = 1,
        [Display(Name = "Riêng Tư")]
        rieng_Tu = 2,
        [Display(Name = "Đã Đóng")]
        da_Dong = 3
    }

    public enum VaiTro
    {
        None = 0,
        [Display(Name = "Quản Trị Viên")]
        quan_Tri_Vien = 1,
        [Display(Name = "Điều Hành Viên")]
        dieu_Hanh_Vien = 2
    }

    public enum TrangThaiCongTy
    {
        None = 0,
        [Display(Name = "Hoạt Động")]
        hoat_Dong = 1,
        [Display(Name = "Ngừng Hoạt Động")]
        tam_Ngung = 2,
        [Display(Name = "Giải Thể")]
        giai_The = 3,
        [Display(Name = "Chờ Duyệt")]
        cho_Duyet = 4
    }

    public enum LoaiThongBao
    {
        None = 0,
        [Display(Name = "Toàn server")]
        toan_Server = 1,
        [Display(Name = "Việc làm mới")]
        viec_Lam_Moi = 2,
        [Display(Name = "Thư mời phỏng vấn")]
        thu_Moi_Phong_Van = 3
    }

    public enum TrangThaiDoc
    {
        None = 0,
        [Display(Name = "Đã đọc")]
        da_Doc = 1,
        [Display(Name = "Chưa đọc")]
        chua_Doc = 2
    }

    public enum TrangThaiDonHang
    {
        None = 0,
        [Display(Name = "Chờ thanh toán")]
        cho_Thanh_Toan = 1,
        [Display(Name = "Đã thanh toán")]
        da_Thanh_Toan = 2,
        [Display(Name = "Thanh toán thất bại")]
        that_Bai = 3,
        [Display(Name = "Đã hủy")]
        da_Huy = 4
    }   

    public enum TrangThaiThanhToan
    {
        None = 0,
        [Display(Name = "Thanh toán thành công")]
        thanh_Cong = 1,
        [Display(Name = "Thanh toán thất bại")]
        that_Bai = 2
    }
}
