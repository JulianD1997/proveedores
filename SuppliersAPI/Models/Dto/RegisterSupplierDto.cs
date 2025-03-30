
using System.ComponentModel.DataAnnotations;
namespace SuppliersApi.Models.Dto;

public class RegisterSupplierDto
{

  [Required(ErrorMessage = "Por favor ingresa el NIT del proveedor")]
  [Display(Name = "Nit")]
  public string NIT { get; set; }
  [Required(ErrorMessage = "Por favor ingresa la razón social del proveedor")]
  [Display(Name = "Razón social")]
  public string CompanyName { get; set; }
  [Display(Name = "Dirección")]
  public string Address { get; set; }
  [Display(Name = "Municipio")]
  public string Municipality { get; set; }
  [Display(Name = "Departamento")]
  public string Department { get; set; }

  [Required(ErrorMessage = "Por favor ingresa nombre de contacto para proveedor")]
  [Display(Name = "Nombre del contacto")]
  public string ContactName { get; set; }
  [Required(ErrorMessage = "Por favor ingresa email del contacto para proveedor")]
  [Display(Name = "Email del contacto")]
  public string ContactEmail { get; set; }
}