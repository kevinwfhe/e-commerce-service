namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
  private readonly ILogger<AdminController> _logger;
  private readonly AdminService _adminService;
  public AdminController(ILogger<AdminController> logger, AdminService adminService)
  {
    _logger = logger;
    _adminService = adminService;
  }

  [HttpGet(Name = "GetAdmins")]
  public async Task<List<Admin>> Get()
  {
    return await _adminService.GetAsync();
  }

  [HttpGet("{id}", Name = "GetAdminById")]
  public async Task<ActionResult<Admin>> GetById(string id)
  {
    var admin = await _adminService.GetByIdAsync(id);
    if (admin is null)
    {
      _logger.LogWarning("Admin {id} could not be found.", id);
      return NotFound();
    }
    return admin;
  }

  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Admin newAdmin)
  {
    await _adminService.CreateAsync(newAdmin);
    return CreatedAtAction(nameof(Get), new { id = newAdmin.id }, newAdmin);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, Admin updatedAdmin)
  {
    var adminToUpdate = await _adminService.GetByIdAsync(id);
    if (adminToUpdate is null)
    {
      _logger.LogWarning("Admin {id} could not be found.", id);
      return NotFound();
    }
    updatedAdmin.id = adminToUpdate.id;
    await _adminService.UpdateByIdAsync(id, updatedAdmin);
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var adminToDelete = await _adminService.GetByIdAsync(id);
    if (adminToDelete is null)
    {
      _logger.LogWarning("Admin {id} could not be found.", id);
      return NotFound();
    }
    await _adminService.DeleteByIdAsync(id);
    return NoContent();
  }
}