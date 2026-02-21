import { Injectable } from '@angular/core';
import { HttpClient , HttpHeaders } from '@angular/common/http';
import { from, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

import { KeycloakService } from 'keycloak-angular';
@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'https://localhost:7063/api/Auth';

  constructor(private http: HttpClient , private router : Router ) {}

  login(email: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
  }

  register(email: string, password: string, role: string) {
    return this.http.post(`${this.apiUrl}/register`, { email, password, role });
  }

 logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
   
  }
  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
  resetPassword(email:string, token : string , newPassword : string):Observable<any>{
    const body = { email, token, newPassword };
    return this.http.post(`${this.apiUrl}/reset-password`, body,{ responseType: 'text' });

  }

 forgotPassword(email: string) {
  return this.http.post(`${this.apiUrl}/forgot-password`, { email });
  } 
  googleLogin(idToken: string) {
    return this.http.post<any>(`${this.apiUrl}/google-login`, { idToken }).pipe(
      tap(user => this.saveUserData(user))
    );
  }

  saveUserData(user: any) {
  localStorage.setItem('token', user.token);
  localStorage.setItem('fullName', user.fullName);
  localStorage.setItem('profilePictureUrl', user.profilePictureUrl);
}

getProfilePicture() {
  return localStorage.getItem('profilePictureUrl') || 'assets/default-avatar.png';
}
getCommercials() {
  return this.http.get<any[]>(`${this.apiUrl}/commercials`);
}
}
