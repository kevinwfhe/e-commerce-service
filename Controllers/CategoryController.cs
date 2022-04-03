namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
  private readonly ILogger<CategoryController> _logger;
  private readonly CategoryService _categoryService;
  public CategoryController(ILogger<CategoryController> logger, CategoryService categoryService)
  {
    _logger = logger;
    _categoryService = categoryService;
  }

  [HttpGet(Name = "GetCategory")]
  public async Task<List<Category>> Get()
  {
    return await _categoryService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetCategoryById")]
  public async Task<ActionResult<Category>> GetById(string id)
  {
    var category = await _categoryService.GetByIdAsync(id);
    if (category is null)
    {
      _logger.LogWarning("Category {id} could not be found.", id);
      return NotFound();
    }
    return category;
  }

  [HttpPost]
  [AdminAuthorize]
  public async Task<ActionResult> Post([FromBody] Category newCategory)
  {
    try
    {
      Category categoryCreated = await _categoryService.CreateAsync(newCategory);
      return CreatedAtAction(nameof(Get), categoryCreated);
    }
    catch (Exception ex)
    {
      return BadRequest(error: ex.Message);
    }
  }

  [HttpPut("{id}")]
  [AdminAuthorize]
  public async Task<ActionResult> Update(string id, [FromBody] Category updatedCategory)
  {
    var categoryToUpdate = await _categoryService.GetByIdAsync(id);
    if (categoryToUpdate is null)
    {
      _logger.LogWarning("Category {id} could not be found.", id);
      return NotFound();
    }
    updatedCategory.id = categoryToUpdate.id;
    try
    {
      await _categoryService.UpdateByIdAsync(id, updatedCategory);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(error: ex.Message);
    }
  }

  [HttpDelete("{id}")]
  [AdminAuthorize]
  public async Task<ActionResult> Delete(string id)
  {
    var categoryToDelete = await _categoryService.GetByIdAsync(id);
    if (categoryToDelete is null)
    {
      _logger.LogWarning("Category {id} could not be found.", id);
      return NotFound();
    }
    await _categoryService.DeleteByIdAsync(id);
    return NoContent();
  }
}