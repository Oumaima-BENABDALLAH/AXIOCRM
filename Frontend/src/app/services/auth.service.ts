import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:7063/api/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
  }

  register(email: string, password: string, role: string) {
    return this.http.post(`${this.apiUrl}/register`, { email, password, role });
  }

  logout() {
    localStorage.removeItem('token');
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


}
