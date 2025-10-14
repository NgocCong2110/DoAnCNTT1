import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Route, RouterLink, Router } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  message: string;
}

@Component({
  selector: 'app-trang-doi-mat-khau',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './trang-doi-mat-khau.html',
  styleUrl: './trang-doi-mat-khau.css'
})
export class TrangDoiMatKhau {
  doiMatKhauForm: FormGroup;
  email: string | null = null;
  isSubmitting = false;

  constructor(private fb: FormBuilder, private http: HttpClient, public route: ActivatedRoute, public router: Router) {
    this.doiMatKhauForm = this.fb.group({
      mat_khau_moi: ['', [Validators.required, Validators.minLength(6)]],
      xac_nhan: ['', Validators.required]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('mat_khau_moi')?.value === form.get('xac_nhan')?.value 
      ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.doiMatKhauForm.invalid) return;

    this.isSubmitting = true;
    const body = {
      email: this.route.snapshot.paramMap.get('email'),
      mat_khau: this.doiMatKhauForm.value.mat_khau_moi
    };

    console.log(body);

    this.http.post<API_RESPONSE>("http://localhost:65001/api/API_WEB/doiMatKhauMoi", body)
      .subscribe({
        next: (data) => {
          this.isSubmitting = false;
          if(data.success){
            this.router.navigate(['/dang-nhap']);
          }
        },
        error: (err) => {
          alert("Lỗi khi đổi mật khẩu");
          this.isSubmitting = false;
        }
      });
  }
}
