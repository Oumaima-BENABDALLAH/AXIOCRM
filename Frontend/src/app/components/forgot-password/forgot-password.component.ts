import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent {
  form: FormGroup;
  message = '';
  error = '';

  constructor(private fb: FormBuilder, private authService: AuthService,private router:Router) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.authService.forgotPassword(this.form.value.email).subscribe({
      next: () => {
        this.message = 'Un email de réinitialisation a été envoyé si l’adresse existe.';
        this.error = '';
        setTimeout(() => {
          this.router.navigate(['/reset-password'], {
            queryParams: { email: this.form.value.email }
          });
        }, 2000);
      },
      error: err => {
        this.error = err.error || 'Erreur lors de l’envoi du lien.';
        this.message = '';
      }
    });
  }
}
