import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface ChungChi {
  file: File;
  preview?: string;
}

@Component({
  selector: 'app-trang-danh-sach-chung-chi',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './trang-danh-sach-chung-chi.html',
  styleUrls: ['./trang-danh-sach-chung-chi.css']
})
export class TrangDanhSachChungChi {
  danh_sach_chung_chi: ChungChi[] = [];

  chonFile(event: any) {
    const files: FileList = event.target.files;
    for (let i = 0; i < files.length; i++) {
      const file = files[i];

      if (!['application/pdf', 'image/jpeg', 'image/png'].includes(file.type)) {
        alert('Chỉ được upload PDF, JPG hoặc PNG!');
        continue;
      }

      const chung_chi: ChungChi = { file };

      if (file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = () => {
          chung_chi.preview = reader.result as string;
        };
        reader.readAsDataURL(file);
      }

      this.danh_sach_chung_chi.push(chung_chi);
    }
  }

  xoaChungChi(index: number) {
    this.danh_sach_chung_chi.splice(index, 1);
  }

  taiLen() {
    if (this.danh_sach_chung_chi.length === 0) {
      alert('Vui lòng chọn ít nhất 1 chứng chỉ');
      return;
    }

    const form_data = new FormData();
    this.danh_sach_chung_chi.forEach((cc, idx) => {
      form_data.append('certificates', cc.file, cc.file.name);
    });

    console.log('FormData ready:', form_data);
    alert('Upload thành công (demo)');
  }
}
