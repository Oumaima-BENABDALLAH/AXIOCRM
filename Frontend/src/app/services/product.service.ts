import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReturnStatement } from '@angular/compiler';
import { Client } from './client.service';
import { map } from 'rxjs/operators';
import { Product } from 'src/app/models/product.model';
interface ProductApiResponse  {
  $values: Product[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiURL ='https://localhost:7063/api/product';
  
  constructor( private http : HttpClient) {}

  /* getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiURL);
  } */
getAll(): Observable<ProductApiResponse | Product[]> {
  return this.http.get<ProductApiResponse | Product[]>(this.apiURL);
}

  getById(id : number) : Observable<Product>{
    return this.http.get<Product>(`${this.apiURL}/${id}`);
  }

  create(product: Product) : Observable<Product>
   {
     return this.http.post<Product>(this.apiURL, product);

   }
  update(product: Product) {
  return this.http.put(`${this.apiURL}/${product.id}`, product);
}
   delete (id : number): Observable<void> {
    return this.http.delete<void>(`${this.apiURL}/${id}`);
   }
}
