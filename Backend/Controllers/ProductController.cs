using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Orders.Commands.CreateOrder;
using AXIOCRM.Application.Orders.Commands.DeleteOrder;
using AXIOCRM.Application.Orders.Commands.UpdateOrder;
using AXIOCRM.Application.Orders.Queries;
using AXIOCRM.Application.Products.Commands.CreateProduct;
using AXIOCRM.Application.Products.Commands.DeleteProduct;
using AXIOCRM.Application.Products.Commands.UpdateProduct;
using AXIOCRM.Application.Products.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Threading.Tasks;


namespace ProductManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly CreateProductCommandHandler _handler;
        private readonly GetAllProductsQueryHandler _getProductHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdHandler;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;

        public ProductController(CreateProductCommandHandler handler ,
                                  GetAllProductsQueryHandler getProductHandler,
                                  GetProductByIdQueryHandler getProductByIdHandler,
                                  DeleteProductCommandHandler deleteProductCommandHandler,
                                  UpdateProductCommandHandler updateProductCommandHandler) 
        {
            _handler = handler;
            _getProductHandler = getProductHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _deleteProductCommandHandler = deleteProductCommandHandler;
            _updateProductCommandHandler = updateProductCommandHandler;
        }
     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _getProductHandler.HandleAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _getProductByIdHandler.HandleAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] CreateProductCommand command)
        {
            var created = await _handler.HandleAsync(command);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] UpdateProductCommand command)
        {
            var updated = await _updateProductCommandHandler.HandleAsync(command);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removed = await _deleteProductCommandHandler.HandleAsync(id);
            return removed ? NoContent() : NotFound();
        }
    }

}
