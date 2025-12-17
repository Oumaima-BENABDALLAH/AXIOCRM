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

  // Generate an invoice based on orderId
  generate(orderId: number): Observable<InvoiceDto> {
    return this.http.post<InvoiceDto>(`${this.apiUrl}/generate/${orderId}`, {});
  }

  // Get invoice by id
  getById(id: number): Observable<InvoiceDto> {
    return this.http.get<InvoiceDto>(`${this.apiUrl}/${id}`);
  }

  // Get all invoices
  getAll(): Observable<InvoiceDto[]> {
    return this.http.get<InvoiceDto[]>(`${this.apiUrl}`);
  }
}
