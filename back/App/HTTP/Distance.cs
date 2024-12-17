using Microsoft.AspNetCore.Mvc;
using Back.Domain.Entities;
using Back.App.DTOs;

namespace Back.App.HTTP;

[ApiController]
[Route("api/[controller]")]
public class DistanceController : ControllerBase
{
    private readonly Container _container = new Container();
    private readonly ILogger<DistanceController> _logger;

    public DistanceController(ILogger<DistanceController> logger)
    {
        _logger = logger;
    }

    [HttpPost("calculate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistanceResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponseDto))]
    public async Task<IActionResult> CalculateAsync([FromBody] DistanceDto dto)
    {
        try
        {
            _logger.LogInformation("Iniciando cálculo de distância de {De} para {Para}.", dto.De, dto.Para);

            User user = await _container.TokenService.VerifyToken(Request.Cookies["X-Access-Token"]);
            Distance distance = await _container.DistanceService.GetCalculateAndSaveAsync(dto.De, dto.Para, user);
            _logger.LogInformation("Cálculo concluído com sucesso. Distância: {Distancia} km.", distance.Distancia);

            return Ok(DistanceResponseDto.Create(distance.De, distance.Para, distance.Distancia));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Tentativa de acesso não autorizado: {Message}", ex.Message);
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning("Argumento nulo: {Message}", ex.Message);
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Chave não encontrada: {Message}", ex.Message);
            return NotFound(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro inesperado: {Message}", ex.Message);
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DistanceResponseDto[]))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ExceptionResponseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ExceptionResponseDto))]
    public async Task<IActionResult> ListAsync()
    {
        try
        {
            _logger.LogInformation("Iniciando listagem das distâncias.");

            User user = await _container.TokenService.VerifyToken(Request.Cookies["X-Access-Token"]);
            Distance[] distances = await _container.DistanceService.GetDistancesByUserAsync(user);
            _logger.LogInformation("Listagem de distâncias concluída. Total de {Count} distâncias encontradas.", distances.Length);

            DistanceResponseDto[] response = distances.Select(d => new DistanceResponseDto(
                de: d.De,
                para: d.Para,
                distancia: d.Distancia
            )).ToArray();

            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Tentativa de acesso não autorizado: {Message}", ex.Message);
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning("Argumento nulo: {Message}", ex.Message);
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro inesperado: {Message}", ex.Message);
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }
    }
}
