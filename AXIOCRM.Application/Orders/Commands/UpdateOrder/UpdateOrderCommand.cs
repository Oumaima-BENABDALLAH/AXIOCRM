using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand
    {
        public int Id { get; init; }
        public int ClientId { get; init; }
        public DateTime OrderDate { get; init; }
        public string PaymentMethod { get; init; } = string.Empty;
        public decimal? CashAmount { get; init; }
        public DateTime? PaymentDate { get; init; }
        public int? DeliveryMethodId { get; init; }

        public string? CardNumber { get; init; }
        public string? CardHolder { get; init; }
        public string? ExpiryDate { get; init; }
        public string? CVV { get; init; }

        public List<UpdateOrderProductItem> OrderProducts { get; init; } = new();
    }

    public record UpdateOrderProductItem
    {
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public string? Color { get; init; }
        public string? ImageUrl { get; init; }
    }
}