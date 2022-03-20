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
  public async Task<PageInfo<List<Product>>> Get([FromQuery] string? keyword, [FromQuery] string? offset, [FromQuery] string? pageSize, [FromQuery] string? sortIndex, [FromQuery] string? sortAsc, [FromQuery] string? category)
  {
    // TODO: deprecate GetByKeywordAsync and use GetByPageAsync instead
    if (offset == null && (keyword != null || category != null))
    {
      if (keyword != null && category == null)
      {
        return await _productService.GetByKeywordAsync(keyword);
      }
      if (category != null && keyword == null)
      {
        return await _productService.GetByCategoryAsync(category);
      }
      if (category != null && keyword != null)
      {
        // TODO: else return a conbine search of keyword and category
        return await _productService.GetByCategoryAsync(category);
      }
    }
    if (offset != null)
    {
      var res = await _productService.GetByPageAsync(offset, pageSize, sortIndex, sortAsc, keyword, category);
      return res;
    }
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
    Product productCreated = await _productService.CreateAsync(newProduct);
    return CreatedAtAction(nameof(Get), productCreated);
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