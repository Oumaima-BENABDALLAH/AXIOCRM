
export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  sales: number;
  revenue?: number;   // readonly côté backend, mais reçu en JSON
  status?: string; 
  imageUrl?: string;   // nouvelle propriété
  color?: string;

}