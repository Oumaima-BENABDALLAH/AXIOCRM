import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReturnStatement } from '@angular/compiler';
import { Client } from './client.service';
import { map } from 'rxjs/operators';
import { ProductDto } from 'src/app/models/product.model';

interface ProductApiResponse  {
  $values: ProductDto[];
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiURL ='https://localhost:7063/api/product';
  
  constructor( private http : HttpClient) {}

getAll(): Observable<ProductDto[]> {
  return this.http.get<ProductApiResponse | ProductDto[]>(this.apiURL).pipe(
    map((resp: ProductApiResponse | ProductDto[]) => {
      if (Array.isArray(resp)) {
        return resp;
      }
      if (resp && Array.isArray(resp.$values)) {
        return resp.$values;
      }
      return [];
    })
  );
}
  getById(id : number) : Observable<ProductDto>{
    return this.http.get<ProductDto>(`${this.apiURL}/${id}`);
  }

  create(product: ProductDto) : Observable<ProductDto>
   {
     return this.http.post<ProductDto>(this.apiURL, product);

   }
update(product: ProductDto): Observable<ProductDto> {
  return this.http.put<ProductDto>(`${this.apiURL}/${product.id}`, product);
}
   delete (id : number): Observable<void> {
    return this.http.delete<void>(`${this.apiURL}/${id}`);
   }
}
