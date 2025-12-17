import { OrderDto } from './order.model';
export interface InvoiceDto {
  id: number;
  orderId: number;
  invoiceNumber: string;
  issueDate: string;
  dueDate: string;
  order?: OrderDto;   
  subTotal: number;
  taxRate: number;
  taxAmount: number;
  total: number;
 
  status: string;
  notes?: string;

  items: InvoiceItemDto[];
}

export interface InvoiceItemDto {
  id: number;
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
  total: number;
  invoice:InvoiceDto;
  invoiceId : number;
}