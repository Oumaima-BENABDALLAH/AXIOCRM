import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderDto } from '../models/order.model';
import { map } from 'rxjs/operators';


interface OrderApiResponse {
  $values: OrderDto[];
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://localhost:7063/api/Order';

  constructor(private http: HttpClient) {}
 /* getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(this.apiUrl);
  } */
 getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderApiResponse>(this.apiUrl).pipe(
      map(response => response.$values || [])
    );
  }
  createOrder(order: OrderDto): Observable<OrderDto> {
    return this.http.post<OrderDto>(this.apiUrl, order);
  }
}
