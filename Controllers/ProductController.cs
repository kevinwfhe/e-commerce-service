namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly ILogger<ProductController> _logger;
  private readonly ProductService _productService;
  private readonly JwtService _jwtService;
  public ProductController(ILogger<ProductController> logger, ProductService productService, JwtService jwtService)
  {
    _logger = logger;
    _productService = productService;
    _jwtService = jwtService;
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
    var product = await _productService.GetById(id);
    if (product is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    return product;
  }

  [AdminAuthorize]
  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Product newProduct)
  {
    Product productCreated = await _productService.CreateAsync(newProduct);
    return CreatedAtAction(nameof(Get), productCreated);
  }

  [AdminAuthorize]
  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] Product updatedProduct)
  {
    var productToUpdate = await _productService.GetById(id);
    if (productToUpdate is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    updatedProduct.id = productToUpdate.id;
    await _productService.UpdateByIdAsync(id, updatedProduct);
    return NoContent();
  }

  [AdminAuthorize]
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var productToDelete = await _productService.GetById(id);
    if (productToDelete is null)
    {
      _logger.LogWarning("Product {id} could not be found.", id);
      return NotFound();
    }
    await _productService.DeleteByIdAsync(id);
    return NoContent();
  }
}