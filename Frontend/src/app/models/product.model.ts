
export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  sales: number;
  revenue?: number;   
  status?: string; 
  imageUrl?: string;
  color?: string;

}