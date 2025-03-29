using MongoDB.Bson;

public interface ISupplierService
{
  Task<Detail> SaveSupplier(RegisterSupplierDto registerSupplier);
  Task<Detail> GetAllSuppliers();
  Task<Detail> GetSupplier(String nit);
  Task<Detail> UpdateSupplier(ObjectId id, UpdateSupplierDto updateSupplier);
  Task<Detail> DeleteSupplier(ObjectId id);
}