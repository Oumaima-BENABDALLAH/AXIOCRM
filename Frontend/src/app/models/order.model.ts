export interface OrderProductDto {
  orderId?: number;
  productId: number;
  quantity: number;
  productName?: string;
  unitPrice?: number;
  color?: string;
  imageUrl?: string;
}
export interface DeliveryMethodDto {
  id: number;
  name: string;
  price: number;
  estimatedDays: number;
}
export interface ClientDto{
  id:number;
  name :string;
  lastName:string;
  email:string;
  phone:string;
  address:string
}
export interface OrderDto {
   id: number;
   client?: ClientDto; 
  clientId: number;
  orderDate: string | null;
  paymentMethod: string;
  cashAmount: number ;
  paymentDate?: string | null;
  cardNumber?: string | null;
  cardHolder?: string | null;
  expiryDate?: string | null;
  cvv?: string | null;
  totalAmount: number;
  deliveryMethodId?: number;             // 🔹 identifiant du mode de livraison
  deliveryMethod?: DeliveryMethodDto;
  orderProducts: OrderProductDto[];

}
