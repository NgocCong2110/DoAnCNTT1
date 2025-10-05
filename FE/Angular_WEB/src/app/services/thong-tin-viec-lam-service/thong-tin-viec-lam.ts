import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThongTinViecLam {
  private ket_qua_tim_kiem = new BehaviorSubject<any[]>([]);
  ket_qua$ = this.ket_qua_tim_kiem.asObservable();

  capNhatDuLieu(data: any){
    this.ket_qua_tim_kiem.next(data);
  }
}
