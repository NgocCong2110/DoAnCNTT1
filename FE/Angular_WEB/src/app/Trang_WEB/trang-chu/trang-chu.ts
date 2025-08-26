import { Component } from '@angular/core';
import { RouterLink, RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Auth } from '../../services/auth';
import { HeaderWEB } from '../Component/header-web/header-web';

@Component({
  selector: 'app-trang-chu',
  imports: [RouterModule, RouterLink, FormsModule, HeaderWEB],
  templateUrl: './trang-chu.html',
  styleUrls: ['./trang-chu.css']
})
export class TrangChu {
  
}
