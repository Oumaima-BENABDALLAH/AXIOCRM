export interface ClientProduct {
  clientId: number;
  productId: number;
}
export interface Client {
  id: number;
  name: string;
  lastName: string;
  email: string;
  phone: string;
  designation: string; 
  status: 'Active' | 'Offline' | 'Away'; 
  // Optionnel : si tu veux inclure les commandes
  // orders?: OrderDto[];
}