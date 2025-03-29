using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
  private readonly ISupplierService _supplierService;
  private readonly ILogger<SupplierController> _logger;

  public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
  {
    _supplierService = supplierService;
    _logger = logger;
  }
  /// <summary>
  /// Obtiene todos los proveedores registrados.
  /// </summary>
  /// <returns>Una lista de proveedores o 204 si no hay datos.</returns>
  [HttpGet("get-all")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> GetAll()
  {
    _logger.LogInformation("Entrando al endpoint GET: /api/supplier/get-all");

    var response = await _supplierService.GetAllSuppliers();
    return response.IsSuccessful ? Ok(response) : NoContent();
  }
  /// <summary>
  /// Busca un proveedor por su NIT.
  /// </summary>
  /// <param name="nit">NIT del proveedor</param>
  /// <returns>Proveedor si existe, 404 si no.</returns>
  [HttpGet("{nit}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetByNIT(string nit)
  {
    _logger.LogInformation("Entrando al endpoint GET: /api/supplier/{nit}", nit);

    var response = await _supplierService.GetSupplier(nit);
    return response.IsSuccessful ? Ok(response) : NotFound(response);
  }

  /// <summary>
  /// Registra un nuevo proveedor.
  /// </summary>
  /// <param name="registerSupplier">Datos del proveedor</param>
  /// <returns>Resultado de la operación</returns>
  [HttpPost("save")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(Detail), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Save(RegisterSupplierDto registerSupplier)
  {
    _logger.LogInformation("Entrando al endpoint POST: /api/supplier/save");

    var response = await _supplierService.SaveSupplier(registerSupplier);
    return response.IsSuccessful ? Ok(response) : BadRequest(response);
  }
  /// <summary>
  /// Actualiza un proveedor por ID.
  /// </summary>
  /// <param name="id">ID del proveedor</param>
  /// <param name="updateSupplier">Datos a actualizar</param>
  /// <returns>Resultado de la operación</returns>
  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Update(string id, UpdateSupplierDto updateSupplier)
  {
    _logger.LogInformation("Entrando al endpoint PUT: /api/supplier/{id}", id);

    if (!ObjectId.TryParse(id, out ObjectId objectId))
    {
      return BadRequest(new Detail
      {
        IsSuccessful = false,
        Message = "ID inválido",
        Status = ResponseStatus.BadRequest
      });
    }

    var response = await _supplierService.UpdateSupplier(objectId, updateSupplier);
    return response.IsSuccessful ? Ok(response) : NotFound(response);
  }
  /// <summary>
  /// Elimina un proveedor por ID.
  /// </summary>
  /// <param name="id">ID del proveedor</param>
  /// <returns>Resultado de la operación</returns>
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(string id)
  {
    _logger.LogInformation("Entrando al endpoint DELETE: /api/supplier/{id}", id);

    if (!ObjectId.TryParse(id, out ObjectId objectId))
    {
      return BadRequest(new Detail
      {
        IsSuccessful = false,
        Message = "ID inválido",
        Status = ResponseStatus.BadRequest
      });
    }

    var response = await _supplierService.DeleteSupplier(objectId);
    return response.IsSuccessful ? Ok(response) : NotFound(response);
  }
}
