export interface ViecLam {
  ma_viec: number;
  ma_cong_ty: number;
  cong_ty?: string | null;
  nganh_nghe: string;
  vi_tri: string;
  kinh_nghiem: string;
  tieu_de: string;
  mo_ta: string;
  yeu_cau: string;
  muc_luong: string;
  dia_diem: string;
  loai_hinh: number;
  ngay_tao?: string | Date;
  ngay_cap_nhat?: string | Date;
  ma_bai_dang?: number;
}

export interface BaiDangComponent {
  ma_bai_dang: number;
  ma_nguoi_dang?: number;
  ten_nguoi_dang?: string;
  tieu_de: string;
  noi_dung: string;
  luot_thich?: number;
  loai_bai?: string; 
  trang_thai?: string; 
  ngay_tao?: string | Date;
  viec_lam?: ViecLam;
}
