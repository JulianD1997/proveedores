using SuppliersApi.Models;

public interface IJwtService
{
  Task<Detail> GetToken();
}
