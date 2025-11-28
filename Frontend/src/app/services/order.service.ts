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
   return this.http.get<OrderDto[]>(this.apiUrl);
}

 createOrder(order: OrderDto) {

  const dtoToSend: OrderDto = {
    ...order,
    paymentDate: order.paymentDate
      ? new Date(order.paymentDate).toISOString().slice(0, 10)
      : null
  };

  return this.http.post<OrderDto>(this.apiUrl, dtoToSend);
}

  updateOrder(order: OrderDto) {

  const dtoToSend: OrderDto = {
    ...order,
    paymentDate: order.paymentDate
      ? new Date(order.paymentDate).toISOString().slice(0, 10)
      : null
  };

  return this.http.put<OrderDto>(`${this.apiUrl}/${order.id}`, dtoToSend);
}




  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  getDashboardStats() {
  return this.http.get<any>(this.apiUrl + '/dashboard');
}

}
