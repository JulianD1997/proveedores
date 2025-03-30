using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuppliersApi.Models;

public class JwtService : IJwtService
{
  private readonly JWTModel _jwtVariables;
  public JwtService(IOptions<JWTModel> jwtOptions)
  {
    _jwtVariables = jwtOptions.Value;
  }

  public async Task<Detail> GetToken()
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtVariables.Key));

    var token = new JwtSecurityToken(
      issuer: _jwtVariables.Issuer,
      audience: _jwtVariables.Audience,
      expires: DateTime.UtcNow.AddHours(_jwtVariables.Expiration_time),
      signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
    );

    return new Detail
    {
      IsSuccessful = true,
      Message = "Lista de proveedores",
      Status = ResponseStatus.Success,
      Token = new JwtSecurityTokenHandler().WriteToken(token)
    };
  }
}