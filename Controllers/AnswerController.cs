namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[ApiController]
[Route("api/[controller]")]
public class AnswerController : ControllerBase
{
  private readonly ILogger<ShippingAddressController> _logger;
  private readonly AnswerService _answerService;
  public AnswerController(ILogger<ShippingAddressController> logger, AnswerService answerService)
  {
    _logger = logger;
    _answerService = answerService;
  }

  [Authorize]
  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Answer newAnswer)
  {
    DetailedAnswer answerCreated = await _answerService.CreateAsync(newAnswer);
    return CreatedAtAction(nameof(Post), answerCreated);
  }

  [Authorize]
  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] Answer updatedAnswer)
  {
    var answerToUpdate = await _answerService.GetByIdAsync(id);
    if (answerToUpdate is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    updatedAnswer.id = answerToUpdate.id;
    await _answerService.UpdateByIdAsync(id, updatedAnswer);
    return NoContent();
  }

  [Authorize]
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var answerToDelete = await _answerService.GetByIdAsync(id);
    if (answerToDelete is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    await _answerService.DeleteByIdAsync(id);
    return NoContent();
  }
}