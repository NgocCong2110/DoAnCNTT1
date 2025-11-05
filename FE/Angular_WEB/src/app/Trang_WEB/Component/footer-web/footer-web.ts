import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Route } from '@angular/router';

@Component({
  selector: 'app-footer-web',
  imports: [RouterLink],
  templateUrl: './footer-web.html',
  styleUrl: './footer-web.css'
})
export class FooterWeb {
  constructor(private router: Router) {}
  trangChu(){
    this.router.navigate(['/trang-chu'])
  }
  chuyenHuongToiNganhNghe(nganh_nghe: string){
    this.router.navigate(['trang-tim-viec-theo-tu-khoa'], { queryParams: { nganh: nganh_nghe } })
  }
}
