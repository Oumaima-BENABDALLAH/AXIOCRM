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
        client_id: '1018179724908-d7l8b3kcqgubkg6vnco23jq0g9igrh23.apps.googleusercontent.com',
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
        // 1️⃣ Sauvegarde du token interne de ton API
        localStorage.setItem('token', response.token);

        // 2️⃣ Redirection vers le Dashboard
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Erreur Google Login', err);
      }
    });
  }
  /*handleCredentialResponse(response: any) {
    console.log('Encoded JWT ID token: ' + response.credential);
    // Appelle ton backend avec ce token
    this.authService.googleLogin(response.credential).subscribe({
      next: (res) => {
        console.log('Backend response:', res);
        this.authService.saveToken(res.token);
        // Redirige vers dashboard ou page souhaitée
      },
      error: (err) => {
        console.error('Erreur login Google:', err);
      }
    });
  }*/
}
