//email-logs
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {EmailHistoryGroup} from '../models/email-history.model'


@Injectable({
  providedIn: 'root'
})
export class EmailHistoryService {
   private apiUrl = 'https://localhost:7063/api/email-logs';
 

  constructor(private http: HttpClient) {}

  getEmailHistory(): Observable<EmailHistoryGroup[]> {
    return this.http.get<EmailHistoryGroup[]>(this.apiUrl);
  }
}
