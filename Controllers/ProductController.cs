namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly ILogger<ProductController> _logger;
  private readonly ProductService _productService;
  public ProductController(ILogger<ProductController> logger, ProductService productService)
  {
    _logger = logger;
    _productService = productService;
  }

  [HttpGet(Name = "GetProducts")]
  public async Task<List<Product>> Get()
  {
    return await _productService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetProductById")]
  public async Task<ActionResult<Product>> GetById(string id)
  {
    var product = await _productService.GetByIdAsync(id);
    if (product is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    return product;
  }

  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Product newProduct)
  {
    await _productService.CreateAsync(newProduct);
    return CreatedAtAction(nameof(Get), new { id = newProduct.id }, newProduct);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, Product updatedProduct)
  {
    var productToUpdate = await _productService.GetByIdAsync(id);
    if (productToUpdate is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    updatedProduct.id = productToUpdate.id;
    await _productService.UpdateByIdAsync(id, updatedProduct);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var productToDelete = await _productService.GetByIdAsync(id);
    if (productToDelete is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    await _productService.DeleteByIdAsync(id);
    return NoContent();
  }
}