import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from 'src/app/models/product.model';
import { ClientDto } from 'src/app/models/client-product.model';
import { ClientProduct } from 'src/app/models/client-product.model';
import { map } from 'rxjs/operators';

export interface Client{
  id?: number;
  name:string;
  email:string;
  phone :string;
  products?:Product[];
}

interface ClientApiResponse  {
  $values: Client[];
}
@Injectable({
  providedIn: 'root'
})
export class ClientService {
  private apiURL ='https://localhost:7063/api/client'

  constructor(private http :HttpClient) { }

   /* getAll(): Observable<Client[]> {
      return this.http.get<Client[]>(this.apiURL);
    }
   */
getAll(): Observable<Client[]> {
  return this.http.get<ClientApiResponse | Client[]>(this.apiURL).pipe(
    map(response => {
      if (Array.isArray(response)) {
        return response;
      }
      if (response && '$values' in response && Array.isArray(response.$values)) {
        return response.$values;
      }
      return []; // cas d'erreur ou réponse inattendue
    })
  );
}
 getClients(): Observable<ClientDto[]> {
  return this.http
    .get<{ $id: string; $values: ClientDto[] }>(this.apiURL)
    .pipe(map(resp => resp.$values || []));
}
    getById(id : number) : Observable<Client>{
      return this.http.get<Client>(`${this.apiURL}/${id}`);
    }
  
    create(client: Client) : Observable<Client>
     {
       return this.http.post<Client>(this.apiURL, client);
  
     }
    update(client: Client) {
    return this.http.put(`${this.apiURL}/${client.id}`, client);
  }
 
     delete (id : number): Observable<void> {
      return this.http.delete<void>(`${this.apiURL}/${id}`);
     }
}
