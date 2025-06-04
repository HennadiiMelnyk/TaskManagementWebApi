using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManagementWebApi.Application.DTO;
using TaskManagementWebApi.Application.Services.Interfaces;

namespace TaskManagementWebApi.Host.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
    {
        try
        {
            var task = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<TaskDto[]>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }
}