
export interface Product{
  id? : number;
  name :string;
  price: number;
}
export interface ProductDto {
  id: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
}