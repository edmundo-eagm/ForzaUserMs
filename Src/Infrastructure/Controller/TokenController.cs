using Microsoft.AspNetCore.Mvc;
using Src.Application.Service;
using Src.Domain.Dto;


[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly JwtService _service;

    public TokenController(JwtService service)
    {
        _service = service;
    }

    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] TokenDto data)
    {
        bool isValid = _service.ValidateToken(data.Token);
        if (isValid)
            return Ok();
        else
            return Unauthorized();
    }
}
