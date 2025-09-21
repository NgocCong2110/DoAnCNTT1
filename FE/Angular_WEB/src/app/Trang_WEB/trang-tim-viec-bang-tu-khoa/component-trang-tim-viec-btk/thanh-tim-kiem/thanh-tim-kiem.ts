import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-thanh-tim-kiem',
  imports: [FormsModule],
  templateUrl: './thanh-tim-kiem.html',
  styleUrl: './thanh-tim-kiem.css'
})
export class ThanhTimKiem {
  tu_khoa_tim_kiem = "";
  async thongTinTim(){
    const response = await fetch("http://localhost:65001/api/API_WEB/",{
      method: "POST",
      headers: {
        "Content-Type" : "application/json"
      },
      body: JSON.stringify(this.tu_khoa_tim_kiem)
    });
    const data = await response.json();
    if(data.success){
      //tutu tinh sau
    }
  }
}
