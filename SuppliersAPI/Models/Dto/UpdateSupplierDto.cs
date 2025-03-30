namespace SuppliersApi.Models.Dto;
public class UpdateSupplierDto
{
  public string? NIT { get; set; }
  public string? CompanyName { get; set; }
  public string? Address { get; set; }
  public string? Municipality { get; set; }
  public string? Department { get; set; }
  public bool? IsActive { get; set; }
  public string? ContactName { get; set; }
  public string? ContactEmail { get; set; }
}