export interface ClientProduct {
  clientId: number;
  productId: number;
}
export interface ClientDto {
  id: number;
  name: string;
  lastName: string;
  email: string;
  phone: string;
  // Optionnel : si tu veux inclure les commandes
  // orders?: OrderDto[];
}