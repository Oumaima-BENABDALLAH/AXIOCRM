using AXIOCRM.Application.DTOs;
using AXIOCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Mappings
{
    public static class ProductExtensions
    {
        public static AXIOCRM.Application.DTOs.ProductDTO ToDto(this Product p)
        {
            if (p == null) return null!; 
            return new AXIOCRM.Application.DTOs.ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                Sales = p.Sales,
                ImageUrl = p.ImageUrl,
                Color = p.Color,
                Revenue = p.Revenue,
                Status = p.Status
            };
        }
    }
}
