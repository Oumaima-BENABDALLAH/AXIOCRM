import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OrderDto } from 'src/app/models/order.model';

interface OrderApiResponse {
  $values: OrderDto[];
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://localhost:7063/api/Order';

  constructor(private http: HttpClient) {}

getOrders(): Observable<OrderDto[]> {
  return this.http.get<{ $id: string, $values: OrderDto[] }>(`${this.apiUrl}`)
    .pipe(map(response => response.$values || []));
}

  createOrder(order: OrderDto): Observable<OrderDto> {
    return this.http.post<OrderDto>(this.apiUrl, order);
  }

  // Ajoute aussi si tu prévois de mettre à jour ou supprimer :
  updateOrder(order: OrderDto): Observable<OrderDto> {
    return this.http.put<OrderDto>(`${this.apiUrl}/${order.id}`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  getDashboardStats() {
  return this.http.get<any>(this.apiUrl + '/dashboard');
}
}
