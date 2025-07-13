export interface OrderProductDto {
  productId: number;
  quantity: number;
  productName?: string; // pour affichage
}

export interface OrderDto {
  id: number;
  clientId: number;
  orderDate: string; // Attention : string côté Angular pour un DateTime backend
  paymentMethod: string;
  products: OrderProductDto[];
}