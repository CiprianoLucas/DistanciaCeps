using Microsoft.AspNetCore.Mvc;
using Back.Domain.Services;
using Back.Domain.Entities;
using Back.App.DTOs;

namespace Back.App.HTTP;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public UserController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("user")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto dto)
    {
        try
        {
            User User = await _userService.CreateUserAsync(dto.Username, dto.Password, dto.Email);
            UserRegisterResponseDto Response = UserRegisterResponseDto.Create(User);
            return Created("", Response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto dto)
    {
        try
        {
            User user = await _userService.VerifyPasswordAsync(dto.Email, dto.Password);
            string token = _tokenService.GenerateToken(user);
            return Ok(UserLoginResponseDto.Create(token));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }
}