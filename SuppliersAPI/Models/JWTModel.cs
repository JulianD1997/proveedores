namespace SuppliersApi.Models;
public class JWTModel
{
  public string Key { get; set; }
  public string Issuer { get; set; }
  public string Audience { get; set; }
  public int Expiration_time { get; set; } // En horas, por ejemplo
}