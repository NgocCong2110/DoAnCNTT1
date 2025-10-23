import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-the-viec-lam-duoc-quan-tam',
  imports: [CommonModule, FormsModule],
  templateUrl: './the-viec-lam-duoc-quan-tam.html',
  styleUrl: './the-viec-lam-duoc-quan-tam.css'
})
export class TheViecLamDuocQuanTam {
  jobs = [
    {
      nganh_nghe: 'cong_nghe_thong_tin',
      ten_cong_ty: 'FPT Corporation',
      tieu_de: 'Tuyển dụng lập trình viên .NET',
      mo_ta: 'Chúng tôi cần tuyển lập trình viên .NET có kinh nghiệm từ 2 năm trở lên.',
      muc_luong: '25 triệu',
      dia_diem: 'Hà Nội',
      so_luong: '1'
    },
    {
      nganh_nghe: 'cong_nghe_thong_tin',
      ten_cong_ty: 'FPT Corporation',
      tieu_de: 'Tuyển dụng thiết kế UI/UX',
      mo_ta: 'Chúng tôi cần tuyển 2 designer UI/UX cho dự án web và mobile app.',
      muc_luong: '18 triệu',
      dia_diem: 'Hà Nội',
      so_luong: '1'
    },
    {
      nganh_nghe: 'cong_nghe_thong_tin',
      ten_cong_ty: 'Viettel',
      tieu_de: 'Tuyển dụng kỹ sư mạng',
      mo_ta: 'Viettel tuyển dụng kỹ sư mạng với kinh nghiệm 3 năm trở lên.',
      muc_luong: '30 triệu',
      dia_diem: 'Hà Nội',
      so_luong: '1'
    },
    {
      nganh_nghe: 'marketing',
      ten_cong_ty: 'Vingroup',
      tieu_de: 'Tuyển dụng nhân viên marketing',
      mo_ta: 'Vingroup cần tuyển nhân viên marketing cho dự án mới tại Cần Thơ.',
      muc_luong: '22 triệu',
      dia_diem: 'Cần Thơ',
      so_luong: '1'
    }
  ];
}
