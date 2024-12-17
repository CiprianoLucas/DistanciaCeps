using Microsoft.AspNetCore.Mvc;
using Back.Domain.Entities;
using Back.App.DTOs;

namespace Back.App.HTTP;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly Container _container = new Container();
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRegisterResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponseDto))]
    public async Task<ActionResult<UserRegisterResponseDto>> RegisterAsync([FromBody] UserRegisterDto dto)
    {
        try
        {
            _logger.LogInformation("Iniciando o registro do usuário: {Username}", dto.Username);

            User user = await _container.UserService.CreateUserAsync(dto.Username, dto.Password, dto.Email);
            UserRegisterResponseDto response = UserRegisterResponseDto.Create(user);

            _logger.LogInformation("Usuário registrado com sucesso: {Username}", dto.Username);

            return Created("", response);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Falha ao registrar usuário: {Username}. Erro: {Message}", dto.Username, ex.Message);
            return Conflict(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning("Dados inválidos fornecidos para o registro do usuário: {Username}. Erro: {Message}", dto.Username, ex.Message);
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro inesperado ao registrar usuário: {Username}. Erro: {Message}", dto.Username, ex.Message);
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponseDto))]
    public async Task<ActionResult<UserLoginResponseDto>> LoginAsync([FromBody] UserLoginDto dto)
    {
        try
        {
            _logger.LogInformation("Tentando fazer login para o usuário: {Email}", dto.Email);

            User user = await _container.UserService.VerifyPasswordAsync(dto.Email, dto.Password);
            string token = _container.TokenService.GenerateToken(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            };
            Response.Cookies.Append("X-Access-Token", token, cookieOptions);

            _logger.LogInformation("Login bem-sucedido para o usuário: {Email}", dto.Email);

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Falha no login para o usuário: {Email}. Erro: {Message}", dto.Email, ex.Message);
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Usuário não encontrado para login: {Email}. Erro: {Message}", dto.Email, ex.Message);
            return NotFound(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning("Dados inválidos fornecidos para o login do usuário: {Email}. Erro: {Message}", dto.Email, ex.Message);
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro inesperado durante o login para o usuário: {Email}. Erro: {Message}", dto.Email, ex.Message);
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        try
        {
            _logger.LogInformation("Usuário desconectado com sucesso.");

            Response.Cookies.Delete("X-Access-Token");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro inesperado durante o logout.");
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }
}
