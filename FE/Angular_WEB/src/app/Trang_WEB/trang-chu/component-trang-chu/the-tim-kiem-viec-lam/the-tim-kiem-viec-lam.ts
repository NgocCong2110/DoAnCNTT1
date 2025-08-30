import { Component } from '@angular/core';
import { RouterLink, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-the-tim-kiem-viec-lam',
  imports: [RouterLink, RouterModule, CommonModule, FormsModule],
  templateUrl: './the-tim-kiem-viec-lam.html',
  styleUrls: ['./the-tim-kiem-viec-lam.css']
})
export class TheTimKiemViecLam {
  constructor(private router: Router) { }
  loading: string = '';
  chonCachTim(cachTim: string) {
    this.loading = cachTim;
    if(cachTim === 'xung-quanh') {
      this.router.navigate(['/trang-tim-viec-xung-quanh']);
    }
    else if(cachTim === 'tu-khoa') {
      this.router.navigate(['/trang-tim-viec-theo-tu-khoa']);
    }
  }
}
