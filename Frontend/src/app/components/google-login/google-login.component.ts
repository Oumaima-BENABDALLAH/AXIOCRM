import { Component, AfterViewInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
declare const google: any;

@Component({
  selector: 'app-google-login',
  template: `<div id="googleButton"></div>`
})
export class GoogleLoginComponent implements AfterViewInit {
  constructor(private authService: AuthService, private router: Router) {}

ngAfterViewInit(): void {
  const interval = setInterval(() => {
    if (typeof google !== 'undefined' && google.accounts) {
      clearInterval(interval);

      google.accounts.id.initialize({
        client_id: '',
        callback: (response: any) => this.handleGoogleResponse(response)
      });

      google.accounts.id.renderButton(
        document.getElementById('googleButton'),
        { theme: 'outline', size: 'large' }
      );
    }
  }, 100);
}
  handleGoogleResponse(response: any) {
    const idToken = response.credential
    this.authService.googleLogin(idToken).subscribe({
      next: (response) => {
        
        localStorage.setItem('token', response.token);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Erreur Google Login', err);
      }
    });
  }

}
