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
  // Les variables email et token de la classe ne sont plus nécessaires,
  // car nous les récupérons directement depuis l'URL dans la méthode onSubmit.
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

    // Nous récupérons l'email et le token directement depuis l'URL ici,
    // pour nous assurer d'avoir les valeurs les plus récentes au moment de la soumission.
    this.route.queryParams.subscribe(params => {
      const email = params['email'] || '';
      const token = params['token'] || '';

      console.log("Token envoyé au backend :", token);
      console.log("Email envoyé au backend :", email);
      console.log("Nouveau mot de passe :", this.resetForm.value.newPassword);

      this.authService.resetPassword(email, token, this.resetForm.value.newPassword)
        .subscribe({
          next: () => {
            this.message = 'Mot de passe réinitialisé avec succès';
            setTimeout(() => this.router.navigate(['/login']), 2000);
          },
          error: (error) => {
            this.message = 'Erreur lors de la réinitialisation';
            this.loading = false;
            // Pour le débogage, il est utile de voir l'erreur complète de l'API.
            console.error("Erreur de l'API:", error);
          }
        });
    });
  }
}