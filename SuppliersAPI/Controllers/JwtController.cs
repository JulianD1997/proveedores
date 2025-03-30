using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SuppliersApi.Models;
using SuppliersApi.Models.Dto;
using SuppliersApi.Services.Interfaces;
namespace SuppliersApi.Controllers;


[Route("/[controller]")]
[ApiController]
public class JwtController : ControllerBase
{
  private readonly IJwtService _jwtService;
  public JwtController(IJwtService jwtService)
  {
    _jwtService = jwtService;
  }
  /// <summary>
  /// Obtener el token de autorización.
  /// </summary>
  /// <returns>Resultado de la operación</returns>
  [HttpPost("get_token")]
  public async Task<IActionResult> GetToken()
  {
    var response = await _jwtService.GetToken();
    return response.IsSuccessful ? Ok(response) : NoContent();
  }
}