using Microsoft.AspNetCore.Mvc;
using ProductManager.API.Models.dto;
using ProductManager.API.Models;
using ProductManager.API.Services;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpPost("generate/{orderId}")]
        public async Task<IActionResult> Generate(int orderId)
        {
            var invoice = await _service.GenerateInvoiceFromOrderAsync(orderId);
            return Ok(invoice);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _service.GetInvoiceByIdAsync(id);
            return invoice != null ? Ok(invoice) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _service.GetAllInvoicesAsync();

            var dto = invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                OrderId = i.OrderId,
                InvoiceNumber = i.InvoiceNumber,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate,
                SubTotal = i.SubTotal,
                TaxRate = i.TaxRate,
                TaxAmount = i.TaxAmount,
                Total = i.Total,
                Status = i.Status.ToString(),
                Notes = i.Notes,

                Items = i.Items.Select(it => new InvoiceItemDto
                {
                    ProductId = it.ProductId,
                    ProductName = it.ProductName,
                    InvoiceId = it.InvoiceId,
                    Invoice = it.Invoice,
                    Quantity = it.Quantity,
                    UnitPrice = it.UnitPrice,
                    //Total = it.UnitPrice * it.Quantity
                }).ToList(),

                // 💙 Ajout de Order + Client dans la facture DTO
                Order = i.Order == null ? null : new OrderDto
                {
                    Id = i.Order.Id,
                    ClientId = i.Order.ClientId,
                    PaymentMethod = i.Order.PaymentMethod,
                    CashAmount = i.Order.CashAmount,
                    TotalAmount = i.Order.TotalAmount,
                    OrderDate = i.Order.OrderDate,

                    Client = i.Order.Client == null ? null : new ClientDto
                    {
                        Id = i.Order.Client.Id,
                        Name = i.Order.Client.Name,
                        LastName = i.Order.Client.LastName,
                        Email = i.Order.Client.Email,
                        Phone = i.Order.Client.Phone,
                        Address = i.Order.Client.Address
                    }
                }
            });

            return Ok(dto);
        }
    }
}