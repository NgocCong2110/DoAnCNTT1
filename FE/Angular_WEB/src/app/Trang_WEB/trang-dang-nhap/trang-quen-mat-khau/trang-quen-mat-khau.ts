import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, QueryList, ViewChildren, ElementRef, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';

interface API_RESPONSE {
  success: boolean;
  message: string;
}

@Component({
  selector: 'app-trang-quen-mat-khau',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './trang-quen-mat-khau.html',
  styleUrl: './trang-quen-mat-khau.css'
})
export class TrangQuenMatKhau {
  buoc: number = 1;
  emailForm: FormGroup;
  otpForm: FormGroup;

  @ViewChildren('otpInput') otpInputs!: QueryList<ElementRef>;

  constructor(
    private fb: FormBuilder,
    private httpclient: HttpClient,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.otpForm = this.fb.group({
      d1: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
      d2: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
      d3: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
      d4: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
      d5: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
      d6: ['', [Validators.required, Validators.pattern(/^[0-9]$/)]],
    });
  }

  guiOtp() {
    if (this.emailForm.invalid) return;

    const email = this.emailForm.value.email;
    this.httpclient.post<API_RESPONSE>('http://localhost:65001/api/API_WEB/guiYeuCauOTP', email)
      .subscribe({
        next: (data) => {
          if (data.success) {

          }
        },
        error: (err) => {
          console.log("otp gui that bai");
          return;
        }
      })
    this.buoc = 2;

    this.cdr.detectChanges();
    const first = this.otpInputs.first;
    if (first) {
      first.nativeElement.focus();
    }
  }

  chuyenO(event: any, index: number) {
    const value = event.target.value;
    const inputs = this.otpInputs.toArray();

    if (value && index < inputs.length - 1) {
      inputs[index + 1]?.nativeElement.focus();
    }

    if (!value && index > 0 && event.inputType === 'deleteContentBackward') {
      inputs[index - 1]?.nativeElement.focus();
    }
  }

  isVerifying = false;

  xacThucOtp() {
    if (this.otpForm.invalid || this.isVerifying) return;

    this.isVerifying = true;
    const otp = Object.values(this.otpForm.value).join('');

    const thong_tin = {
      email: this.emailForm.value.email,
      ma_otp_gui_di: otp
    };


    this.httpclient.post<API_RESPONSE>("http://localhost:65001/api/API_WEB/xacNhanOTP", thong_tin)
      .subscribe({
        next: (data) => {
          if (data.success) {
            this.chuyenFormDoiMatKhau(this.emailForm.value.email);
          }
        },
        error: (err) => {
          console.error("OTP sai hoặc lỗi", err);
          this.isVerifying = false;
        }
      });
  }

  chuyenFormDoiMatKhau(email: string){
    this.router.navigate(["/trang-doi-mat-khau", email])
  }
}
