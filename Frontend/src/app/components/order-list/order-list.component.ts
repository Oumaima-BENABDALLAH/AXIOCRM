import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/services/order.service';
import { OrderDto } from 'src/app/models/order.model';
import { ClientService } from 'src/app/services/client.service';
import { ProductService } from 'src/app/services/product.service';
import { HttpClient } from '@angular/common/http';
import { FormControl } from '@angular/forms';
import { debounceTime } from 'rxjs/operators';
@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
   orders: OrderDto[] = [];
   selectedOrder?: OrderDto;
   filteredOrders: OrderDto[] = [];
   filter = new FormControl('');

    clients: any[] = [];
   products: any[] = [];

 constructor(private http: HttpClient ,private orderService: OrderService) {}

loadOrders() {
    this.orderService.getOrders().subscribe({
      next: data => {
          this.orders = data.filter(order => order && order.orderDate);
          this.filteredOrders = [...this.orders];  // IMPORTANT
          console.log('Commandes chargées', this.orders);
      },
      error: err => {
        console.error('Erreur lors du chargement des commandes', err);
      }
    });
  }

  ngOnInit(): void {
   this.loadOrders();
   this.filter.valueChanges.pipe(debounceTime(300)).subscribe(val => {
      this.applyFilter(val);
    });
  }
  applyFilter(searchText: string) {
    if (!searchText) {
      this.filteredOrders = [...this.orders];
      return;
    }
    const lower = searchText.toLowerCase();
    this.filteredOrders = this.orders.filter(order => {
    const paymentMethod = order.paymentMethod ? order.paymentMethod.toLowerCase() : '';
    const paymentMatch = paymentMethod.includes(lower);
    const productMatch = order.products ? order.products.some(p => {
    const productName = p.productName ? p.productName.toLowerCase() : '';
    return productName.includes(lower);
  }) : false;

    return paymentMatch || productMatch;
    });
  }
  selectOrder(order: OrderDto) {
    this.selectedOrder = order;
  }
}
