import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent implements OnInit {
  resetForm!: FormGroup;
  message = '';
  loading = false;
  newPasswordType: string = 'password';
  confirmPasswordType: string = 'password';
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.resetForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
     
  }
 togglePasswordVisibility(field: string): void {
    if (field === 'new') {
      this.newPasswordType = this.newPasswordType === 'password' ? 'text' : 'password';
    } else if (field === 'confirm') {
      this.confirmPasswordType = this.confirmPasswordType === 'password' ? 'text' : 'password';
    }
  }
  onSubmit() {
    if (this.resetForm.invalid) {
      return;
    }

    if (this.resetForm.value.newPassword !== this.resetForm.value.confirmPassword) {
      this.message = "Les mots de passe ne correspondent pas.";
      return;
    }

    this.loading = true;
    this.route.queryParams.subscribe(params => {
      const email = params['email'] || '';
      const token = params['token'] || '';

      console.log("Token sent to the backend :", token);
      console.log("Email sent to the backend :", email);
      console.log("New password :", this.resetForm.value.newPassword);

      this.authService.resetPassword(email, token, this.resetForm.value.newPassword)
        .subscribe({
          next: () => {
            this.message = 'Password reset successfully';
            setTimeout(() => this.router.navigate(['/login']), 2000);
          },
          error: (error) => {
            this.message = 'Error resetting';
            this.loading = false;
            console.error("API error:", error);
          }
        });
    });
  }
}