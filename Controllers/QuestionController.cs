namespace csi5112group1project_service.Controllers;
using Microsoft.AspNetCore.Mvc;
using csi5112group1project_service.Models;
using csi5112group1project_service.Services;
using csi5112group1project_service.Utils;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
  private readonly ILogger<ShippingAddressController> _logger;
  private readonly QuestionService _questionService;
  public QuestionController(ILogger<ShippingAddressController> logger, QuestionService questionService)
  {
    _logger = logger;
    _questionService = questionService;
  }

  [HttpGet(Name = "GetQuestions")]
  public async Task<List<IntermediateQuestion>> Get([FromQuery] string? keyword)
  {
    return await _questionService.GetAsync(keyword);
  }

  [HttpGet("{id}", Name = "GetQuestionById")]
  public async Task<ActionResult<Question>> GetById(string id)
  {
    var question = await _questionService.GetByIdAsync(id);
    if (question is null)
    {
      _logger.LogWarning("Question {id} could not be found.", id);
      return NotFound();
    }
    return question;
  }

  [Authorize]
  [HttpPost]
  public async Task<ActionResult> Post([FromBody] Question newQuestion)
  {
    Question questionCreated = await _questionService.CreateAsync(newQuestion);
    return CreatedAtAction(nameof(Post), questionCreated);
  }

  [Authorize]
  [HttpPut("{id}")]
  public async Task<ActionResult> Update(string id, [FromBody] Question updatedQuestion)
  {
    var questionToUpdate = await _questionService.GetByIdAsync(id);
    if (questionToUpdate is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    updatedQuestion.id = questionToUpdate.id;
    await _questionService.UpdateByIdAsync(id, updatedQuestion);
    return NoContent();
  }

  [Authorize]
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(string id)
  {
    var questionToDelete = await _questionService.GetByIdAsync(id);
    if (questionToDelete is null)
    {
      _logger.LogWarning("Address {id} could not be found.", id);
      return NotFound();
    }
    await _questionService.DeleteByIdAsync(id);
    return NoContent();
  }
}