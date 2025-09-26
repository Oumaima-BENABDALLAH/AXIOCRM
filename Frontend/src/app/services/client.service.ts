import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductDto } from 'src/app/models/product.model';
import { ClientProduct } from 'src/app/models/client-product.model';
import { map } from 'rxjs/operators';

export interface Client{
  id?: number;
  name:string;
  lastName: string;
  fullName?: string;
  email:string;
 phone: {
    dialCode: string;
    phoneNumber: string;
  };
  designation :string;
  status:string;
  dateOfBirth?: Date | string;
  profilePic:string;     
  company?: string;
  address?: string;
  city?: string;
  postalCode?: string;
  province?: string;
  jobTitle?: string;
  country: string;
  hireDate?: Date | string;
  workReferenceNumber?: string;
  occupationGroup?: string;
  department?: string;
  division?: string;
  salary?: number;
  manager: string;
  employmentType: string;
  notes: string;
  orders: any[];

  products?:ProductDto[];
}

interface ClientApiResponse  {
  $values: Client[];
}
@Injectable({
  providedIn: 'root'
})

export class ClientService {
  private apiURL = 'https://localhost:7063/api/client';

  constructor(private http: HttpClient) {}

//getAll(): Observable<Client[]> {
   // return this.http.get<Client[]>(this.apiURL);
//  }
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
  
  // Ajoutez une méthode pour la page de détails si votre API a un endpoint dédié
  getClientDetails(id: number): Observable<Client> {
    return this.http.get<Client>(`${this.apiURL}/${id}/details`);
  }

  create(client: Client): Observable<Client> {
    return this.http.post<Client>(this.apiURL, client);
  }
  
  update(client: Client): Observable<any> {
    return this.http.put(`${this.apiURL}/${client.id}`, client);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiURL}/${id}`);
  }
}