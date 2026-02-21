import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InvoiceDto } from '../models/invoice.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private apiUrl = 'https://localhost:7063/api/invoice';
  constructor(private http: HttpClient) {}
  generate(orderId: number): Observable<InvoiceDto> {
    return this.http.post<InvoiceDto>(`${this.apiUrl}/generate/${orderId}`, {});
  }

  getById(id: number): Observable<InvoiceDto> {
    return this.http.get<InvoiceDto>(`${this.apiUrl}/${id}`);
  }
  getAll(): Observable<InvoiceDto[]> {
    return this.http.get<InvoiceDto[]>(`${this.apiUrl}`);
  }
}
