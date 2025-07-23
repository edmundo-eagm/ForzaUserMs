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

    [HttpPost("signin")]
    public async Task<IActionResult> Signin([FromBody] UserSigninDto data)
    {
        var response = await _service.Signin(data);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto data)
    {
        var response = await _service.Login(data);
        return Ok(response);
    }
}
