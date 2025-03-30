using MongoDB.Bson;
using SuppliersApi.Models;
using SuppliersApi.Models.Dto;
namespace SuppliersApi.Services.Interfaces;
public interface ISupplierService
{
  Task<Detail> SaveSupplier(RegisterSupplierDto registerSupplier);
  Task<Detail> GetAllSuppliers();
  Task<Detail> GetSupplier(string nit);
  Task<Detail> UpdateSupplier(string nit, UpdateSupplierDto updateSupplier);
  Task<Detail> DeleteSupplier(string nit);
}