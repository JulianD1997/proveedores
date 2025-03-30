using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using SuppliersApi.Context;
using SuppliersApi.Models;
using SuppliersApi.Models.Dto;
using SuppliersApi.Services.Interfaces;
namespace SuppliersApi.Services;

public class SupplierService : ISupplierService
{
  private readonly SupplierDbContext _dbContext;
  private readonly ILogger<SupplierService> _logger;

  public SupplierService(SupplierDbContext dbContext, ILogger<SupplierService> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task<Detail> GetAllSuppliers()
  {
    _logger.LogInformation("Obteniendo todos los proveedores...");
    try
    {
      var suppliers = await _dbContext.Suppliers.OrderBy(s => s.NIT).ToListAsync();

      return new Detail
      {
        IsSuccessful = true,
        Message = "Lista de proveedores",
        Status = ResponseStatus.Success,
        Suppliers = suppliers
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al obtener proveedores");
      return new Detail
      {
        IsSuccessful = false,
        Message = "Se produjo un error al obtener los proveedores",
        Status = ResponseStatus.BadRequest
      };
    }
  }

  public async Task<Detail> GetSupplier(string nit)
  {
    _logger.LogInformation("Buscando proveedor con NIT: {NIT}", nit);
    var supplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.NIT == nit);

    if (supplier == null)
    {
      _logger.LogWarning("Proveedor con NIT {NIT} no encontrado", nit);
      return new Detail
      {
        IsSuccessful = false,
        Message = $"Proveedor {nit} no encontrado",
        Status = ResponseStatus.NotFound
      };
    }

    return new Detail
    {
      IsSuccessful = true,
      Message = "Proveedor encontrado",
      Status = ResponseStatus.Success,
      Suppliers = new List<Supplier> { supplier }
    };
  }

  public async Task<Detail> SaveSupplier(RegisterSupplierDto registerSupplier)
  {
    _logger.LogInformation("Guardando proveedor con NIT: {NIT}", registerSupplier.NIT);
    try
    {
      var existing = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.NIT == registerSupplier.NIT);
      if (existing != null)
      {
        _logger.LogWarning("Proveedor con NIT ya existe");
        return new Detail
        {
          IsSuccessful = false,
          Message = "Ya existe un proveedor con ese NIT",
          Status = ResponseStatus.BadRequest
        };
      }

      var supplier = new Supplier
      {
        NIT = registerSupplier.NIT,
        CompanyName = registerSupplier.CompanyName,
        Address = registerSupplier.Address,
        Municipality = registerSupplier.Municipality,
        Department = registerSupplier.Department,
        ContactName = registerSupplier.ContactName,
        ContactEmail = registerSupplier.ContactEmail,
        CreatedAT = DateTime.UtcNow,
        IsActive = true
      };

      await _dbContext.Suppliers.AddAsync(supplier);
      await _dbContext.SaveChangesAsync();

      return new Detail
      {
        IsSuccessful = true,
        Message = "Proveedor guardado exitosamente",
        Status = ResponseStatus.Created,
        Suppliers = new List<Supplier> { supplier }
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al guardar proveedor");
      return new Detail
      {
        IsSuccessful = false,
        Message = "Ha ocurrido un error al guardar el proveedor",
        Status = ResponseStatus.BadRequest
      };
    }
  }

  public async Task<Detail> DeleteSupplier(ObjectId id)
  {
    _logger.LogInformation("Eliminando proveedor con ID: {ID}", id);
    try
    {
      var supplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
      if (supplier == null)
      {
        _logger.LogWarning("Proveedor no encontrado");
        return new Detail
        {
          IsSuccessful = false,
          Message = "Proveedor no encontrado",
          Status = ResponseStatus.NotFound
        };
      }

      _dbContext.Suppliers.Remove(supplier);
      await _dbContext.SaveChangesAsync();

      return new Detail
      {
        IsSuccessful = true,
        Message = "Proveedor eliminado correctamente",
        Status = ResponseStatus.Success
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al eliminar proveedor");
      return new Detail
      {
        IsSuccessful = false,
        Message = "Error al eliminar proveedor",
        Status = ResponseStatus.BadRequest
      };
    }
  }

  public async Task<Detail> UpdateSupplier(ObjectId id, UpdateSupplierDto updateSupplier)
  {
    _logger.LogWarning("Actualización de proveedor con ID: {ID}", id);

    try
    {
      var supplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
      if (supplier == null)
      {
        return new Detail
        {
          IsSuccessful = false,
          Message = "Proveedor no encontrado",
          Status = ResponseStatus.NotFound
        };
      }
      supplier.NIT = updateSupplier.NIT ?? supplier.NIT;
      supplier.CompanyName = updateSupplier.CompanyName ?? supplier.CompanyName;
      supplier.Address = updateSupplier.Address ?? supplier.Address;
      supplier.Municipality = updateSupplier.Municipality ?? supplier.Municipality;
      supplier.Department = updateSupplier.Department ?? supplier.Department;
      supplier.ContactName = updateSupplier.ContactName ?? supplier.ContactName;
      supplier.ContactEmail = updateSupplier.ContactEmail ?? supplier.ContactEmail;
      supplier.IsActive = updateSupplier.IsActive ?? supplier.IsActive;

      await _dbContext.SaveChangesAsync();

      return new Detail
      {
        IsSuccessful = true,
        Message = "Proveedor actualizado correctamente",
        Status = ResponseStatus.Success,
        Suppliers = new List<Supplier> { supplier }
      };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error al actualizar proveedor");
      return new Detail
      {
        IsSuccessful = false,
        Message = "Error al actualizar proveedor",
        Status = ResponseStatus.BadRequest
      };
    }
  }
}
