using Microsoft.AspNetCore.Mvc;
using Back.Domain.Services;
using Back.Domain.Entities;
using Back.App.DTOs;
using Back.App;

namespace Back.App.HTTP;

[ApiController]
[Route("api/distance/[controller]")]
public class DistanceController : ControllerBase
{
    private readonly Container Container = new Container();


    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateAsync([FromBody] DistanceDto dto)
    {
        try
        {
            User User = await Container.TokenService.VerifyToken(Request.Cookies["X-Access-Token"]);
            Distance Distance = await Container.DistanceService.GetCalculateAndSaveAsync(dto.De, dto.Para, User);
            return Ok(DistanceResponseDto.Create(Distance.De, Distance.Para, Distance.Distancia));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ExceptionResponseDto.Create(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ExceptionResponseDto.Create(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ExceptionResponseDto.Create(ex.Message));
        }

    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync()
    {
        try
        {
            User user = await Container.TokenService.VerifyToken(Request.Cookies["X-Access-Token"]);
            Distance[] distances = await Container.DistanceService.GetDistancesByUserAsync(user);
            DistanceResponseDto[] response = distances.Select(d => new DistanceResponseDto(
                de: d.De,
                para: d.Para,
                distancia: d.Distancia
            )).ToArray();

            return Ok(distances);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ExceptionResponseDto.Create(ex.Message));
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