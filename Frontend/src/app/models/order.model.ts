export interface OrderProductDto {
  productId: number;
  quantity: number;
  productName?: string; // facultatif pour l’affichage
}

export interface OrderDto {
  id?: number;
  clientId: number;
  orderDate: string;
  paymentMethod: string;
  products: OrderProductDto[];
}
