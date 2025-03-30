using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
namespace SuppliersApi.Models;
[Collection("suppliers")]
public class Supplier
{
  [BsonId]
  public ObjectId Id { get; set; }

  [Required(ErrorMessage = "Por favor ingresa el NIT del proveedor")]
  public string NIT { get; set; }
  [Required(ErrorMessage = "Por favor ingresa la razón social del proveedor")]
  public string CompanyName { get; set; }
  public string Address { get; set; }
  public string Municipality { get; set; }
  public string Department { get; set; }
  public bool IsActive { get; set; }
  public DateTime CreatedAT { get; set; }
  [Required(ErrorMessage = "Por favor ingresa nombre de contacto para proveedor")]
  public string ContactName { get; set; }
  [Required(ErrorMessage = "Por favor ingresa email del contacto para proveedor")]
  public string ContactEmail { get; set; }

}