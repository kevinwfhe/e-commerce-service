namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
  private readonly ILogger<ShippingAddressController> _logger;
  private readonly CommentService _commentService;
  public CommentController(ILogger<ShippingAddressController> logger, CommentService commentService)
  {
    _logger = logger;
    _commentService = commentService;
  }

  [Authorize]
  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Comment newComment)
  {
    Comment commentCreated = await _commentService.CreateAsync(newComment);
    User currentUser = (User)HttpContext.Items["User"];
    var res = new DetailedComment(
      comment: commentCreated,
      author: currentUser
    );
    return CreatedAtAction(nameof(Post), res);
  }

  [Authorize]
  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] Comment updatedComment)
  {
    var commentToUpdate = await _commentService.GetByIdAsync(id);
    if (commentToUpdate is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    updatedComment.id = commentToUpdate.id;
    await _commentService.UpdateByIdAsync(id, updatedComment);
    return NoContent();
  }

  [Authorize]
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var commentToDelete = await _commentService.GetByIdAsync(id);
    if (commentToDelete is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    await _commentService.DeleteByIdAsync(id);
    return NoContent();
  }
}