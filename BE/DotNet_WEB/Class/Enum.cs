using System;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Sau Đại Học")]
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

    public enum TrangThaiViecLam
    {
        None = 0,
        [Display(Name = "Đang Hiển Thị")]
        dang_Hien_Thi = 1,
        [Display(Name = "Tạm Ẩn")]
        tam_An = 2,
        [Display(Name = "Đã Hết Hạn")]
        da_Het_Han = 3,
        [Display(Name = "Chờ Duyệt")]
        cho_Duyet = 4
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
        [Display(Name = "Hồ Sơ")]
        ho_So = 1,
        [Display(Name = "Chia Sẻ")]
        chia_Se = 2,
        [Display(Name = "Hỗ Trợ")]
        ho_Tro = 3
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
        Admin = 1,
        [Display(Name = "Điều Hành Viên")]
        Moderator = 2
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
}
