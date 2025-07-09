import { Component, OnInit } from '@angular/core';
import { OrderDto } from 'src/app/models/order.model';
import { OrderService } from 'src/app/services/order.service';
import { ClientDto } from 'src/app/models/client-product.model';
import { ProductDto } from 'src/app/models/product.model';
import { ClientService } from 'src/app/services/client.service';
import { ProductService } from 'src/app/services/product.service';
import { Observable, of } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
declare var $: any;

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: OrderDto[] = [];
  filteredOrders$: Observable<OrderDto[]> = of([]);
  filter = new FormControl('');
  selectedOrder: OrderDto | null = null;
  orderForm!: FormGroup;
  clients: ClientDto[] = [];
  products: ProductDto[] = [];


  constructor(private orderService: OrderService ,private fb: FormBuilder,
     private clientService: ClientService,
      private productService: ProductService
  ) {}

 ngOnInit(): void {
  // Charger les données
  this.loadOrders();
  this.loadClients();
  this.loadProducts();

  // Initialiser le formulaire de commande
  this.orderForm = this.fb.group({
    id: [null],
    clientId: [null, Validators.required],
    orderDate: ['', Validators.required],
    paymentMethod: ['', Validators.required],
    productId: [null, Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]]
  });

  // Mettre en place le filtre pour les commandes
  this.filteredOrders$ = this.filter.valueChanges.pipe(
    startWith(''),
    map(value => this.applyFilter(value || ''))
  );
}

loadOrders(): void {
  this.orderService.getOrders().subscribe({
    next: (data) => {
      this.orders = data; // ✅ data est déjà un tableau pur
      console.log('Commandes chargées', data);

      this.filteredOrders$ = this.filter.valueChanges.pipe(
        startWith(''),
        map(value => this.applyFilter(value || ''))
      );
    },
    error: (err) => {
      console.error('Erreur chargement commandes', err);
    }
  });
}
   loadClients(): void {
    this.clientService.getClients().subscribe({
      next: (data) => {
        this.clients = data;
        console.log('Clients chargés', data);
      },
      error: (err) => {
        console.error('Erreur chargement clients', err);
      }
    });
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products = data;
        console.log('Produits chargés', data);
      },
      error: (err) => {
        console.error('Erreur chargement produits', err);
      }
    });
  }
  applyFilter(value: string): OrderDto[] {
    const filterValue = value.toLowerCase();
    return this.orders.filter(order =>
      order.paymentMethod.toLowerCase().includes(filterValue) ||
      order.orderDate.toLowerCase().includes(filterValue)
    );
  }

  selectOrder(order: OrderDto): void {
    this.selectedOrder = order;
  }

  onAdd(): void {
   this.orderForm.reset();
  ($('#orderModal') as any).modal('show'); // ou ta méthode modale
  }

  onEdit(): void {
  if (this.selectedOrder) {
     const product = this.selectedOrder.products && this.selectedOrder.products.length > 0
     ? this.selectedOrder.products[0]
     : null;

    this.orderForm.patchValue({
    id: this.selectedOrder.id,
    clientId: this.selectedOrder.clientId,
    orderDate: this.selectedOrder.orderDate,
    paymentMethod: this.selectedOrder.paymentMethod,
    productId: product ? product.productId : null,
    quantity: product ? product.quantity : 1
});

      ($('#orderModal') as any).modal('show');
    }
  }

  onDelete(): void {
    if (this.selectedOrder) {
      // logique de suppression à implémenter
      console.log('Supprimer commande', this.selectedOrder);
    }
  }
saveOrder(): void {
  const order: OrderDto = {
      id: this.orderForm.value.id,
      clientId: this.orderForm.value.clientId,
      orderDate: this.orderForm.value.orderDate,
      paymentMethod: this.orderForm.value.paymentMethod,
      products: [
        {
          productId: this.orderForm.value.productId,
          quantity: this.orderForm.value.quantity
        }
      ]
    };

    this.orderService.createOrder(order).subscribe(() => {
      this.loadOrders();
      this.closeModal();
    });
}
closeModal(): void {
  ($('#orderModal') as any).modal('hide');
}

  highlight(text: string, search: string): string {
    if (!search) return text;
    const regex = new RegExp(`(${search})`, 'gi');
    return text.replace(regex, `<mark>$1</mark>`);
  }
}
