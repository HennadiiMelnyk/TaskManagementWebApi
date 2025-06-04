using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TaskManagementWebApi.Application.DTO;
using TaskManagementWebApi.Application.Services;
using TaskManagementWebApi.Application.Services.Interfaces;

namespace TaskManagementWebApi.Host.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service) => _service = service;
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        try
        {
            var user = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());
}