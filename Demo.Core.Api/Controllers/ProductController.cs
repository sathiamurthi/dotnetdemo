using Demo.Core.Framework.Command;
using Demo.Core.Domain.Models;
using Demo.Core.Framework.Notify;
using Demo.Core.Framework.Query;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Serilog;
namespace Demo.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ProductController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("product")]
        public async Task<IActionResult> Get(string productSKu)
        {

            _logger.Information("Getting product information");
            var result = await _mediator.Send(new GetProductQuery() {Sku = productSKu});
            return Ok(result);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            // Create product
            var result = await _mediator.Send(new AddOrUpdateProductCommand() { productModel = product});

            // Notify consumers
            await _mediator.Publish(new PublishProductNotify() {Message = $"Product {product.Sku} created"});

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            // Update product
            var result = await _mediator.Send(new AddOrUpdateProductCommand() { productModel = product });

            // Notify consumers
            await _mediator.Publish(new PublishProductNotify() { Message = $"Product {product.Sku} updated" });

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(string productSku)
        {
            // Remove product
            var result = await _mediator.Send(new DeleteProductCommand() {Sku = productSku});

            // Notify consumers
            await _mediator.Publish(new PublishProductNotify() { Message = $"Product {productSku} removed" });

            return Ok(result);
        }

        [HttpPost("notify")]
        public async Task<IActionResult> NotifyAsync(string message)
        {
            await _mediator.Publish(new PublishProductNotify() {Message = message});
            return Ok();
        }
    }
}
