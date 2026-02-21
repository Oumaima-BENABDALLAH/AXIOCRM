import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChurnDashboard } from '../models/churn-dashboard.model';

@Injectable({ providedIn: 'root' })
export class ChurnService {
  private baseUrl = 'https://localhost:7063/api/ai/churn';;

  constructor(private http: HttpClient) {}

  getDashboard(): Observable<ChurnDashboard> {
    return this.http.get<ChurnDashboard>(`${this.baseUrl}/dashboard`);
  }
}