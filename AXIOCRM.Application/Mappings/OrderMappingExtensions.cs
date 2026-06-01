using AXIOCRM.Application.DTOs;
using AXIOCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Mappings
{
    public static class OrderMappingExtensions
    {
        public static OrderDto ToDto(this Order o)
        {
            if (o == null) return null;

            return new OrderDto
            {
                Id = o.Id,
                ClientId = o.ClientId,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod,
                CashAmount = o.CashAmount,
                PaymentDate = o.PaymentDate,
                DeliveryMethodId = o.DeliveryMethodId,
                CardNumber = o.CardNumber,
                CardHolder = o.CardHolder,
                ExpiryDate = o.ExpiryDate,
                CVV = o.CVV,
                TotalAmount = o.TotalAmount,
                InvoiceId = o.Invoice?.Id, 

                Client = o.Client == null ? null : new ClientDto
                {
                    Id = o.Client.Id,
                    Name = o.Client.Name,
                    LastName = o.Client.LastName,
                    Email = o.Client.Email,
                    Phone = o.Client.Phone,
                    Address = o.Client.Address
                },

                OrderProducts = o.OrderProducts?.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    UnitPrice = op.UnitPrice,
                    Color = op.Color,
                    ImageUrl = op.ImageUrl,
                    ProductName = op.Product?.Name 
                }).ToList()
            };
        }
    }
}