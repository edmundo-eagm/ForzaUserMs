using Microsoft.AspNetCore.Mvc;
using Src.Application.Service;
using Src.Domain.Dto;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    // [HttpGet]
    // public IActionResult GetAll() => Ok(_service.GetAll());

    // [HttpGet("{id}")]
    // public IActionResult GetById(int id)
    // {
    //     var usero = _service.GetById(id);
    //     if (usero == null) return NotFound();
    //     return Ok(usero);
    // }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto data)
    {
        var response = await _service.Create(data);
        return Ok(response);
    }
}
