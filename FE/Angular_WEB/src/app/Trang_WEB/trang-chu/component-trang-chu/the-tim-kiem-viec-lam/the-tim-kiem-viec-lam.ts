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
  chonCachTim(cach: string) {
    this.loading = cach;

    setTimeout(() => {
      if (cach === 'tu-khoa') {
        this.router.navigate(['/trang-tim-viec-theo-tu-khoa']);
      } else if (cach === 'xung-quanh') {
        this.router.navigate(['/trang-tim-viec-xung-quanh']);
      }
      this.loading = '';
    }, 1000);
  }
}
