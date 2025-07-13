import { Component, OnInit } from '@angular/core';
import { ClientDto } from 'src/app/models/client-product.model';
import { ProductDto } from 'src/app/models/product.model';
import { ClientService } from 'src/app/services/client.service';
import { ProductService } from 'src/app/services/product.service';
@Component({
  selector: 'app-client-product-list',
  templateUrl: './client-product-list.component.html',
  styleUrls: ['./client-product-list.component.css']
})
export class ClientProductListComponent implements OnInit {
  clients: ClientDto[] = [];
  products: ProductDto[] = [];
constructor(
    private clientService: ClientService,
    private productService: ProductService
  ) {}
  ngOnInit(): void {
    this.clientService.getClients().subscribe(data => {
      this.clients = data;
      console.log('Clients chargés', data);
    });

    this.productService.getProducts().subscribe(data => {
      this.products = data;
      console.log('Produits chargés', data);
    });
  }
}
