using System.Text.Json.Serialization;
namespace SuppliersApi.Models;
public class Detail
{
  [JsonIgnore]
  public ResponseStatus Status { get; set; }

  public bool IsSuccessful { get; set; }

  public string Message { get; set; }

  public List<Supplier>? Suppliers { get; set; }
}
public enum ResponseStatus : int
{
  Success = 200,
  Created = 201,
  NoContent = 204,
  BadRequest = 400,
  NotFound = 404,
}