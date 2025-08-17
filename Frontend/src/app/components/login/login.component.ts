import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']

})
export class LoginComponent {
  email: string = '';
  password: string = '';
  loginError: string = '';
  passwordType: string = 'password';

  constructor(private authService : AuthService, private router : Router)
  {
    
  }

  onSubmit(): void {
    this.authService.login(this.email, this.password).subscribe({
      next: (res: any) => {
        this.authService.saveToken(res.token);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loginError = 'Identifiants invalides';
        console.error(err);
      }
    });
  }
   togglePasswordVisibility(): void {
    this.passwordType = this.passwordType === 'password' ? 'text' : 'password';
  }



}
