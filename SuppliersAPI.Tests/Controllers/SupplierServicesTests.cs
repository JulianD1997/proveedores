using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

using SuppliersApi.Controllers;
using SuppliersApi.Services.Interfaces;
using SuppliersApi.Models;
using SuppliersApi.Models.Dto;

namespace SuppliersAPI.Tests.Controllers;
public class SupplierControllerTests
{
  private readonly SupplierController _controller;
  private readonly Mock<ISupplierService> _mockService = new();
  private readonly Mock<ILogger<SupplierController>> _mockLogger = new();

  public SupplierControllerTests()
  {
    _controller = new SupplierController(_mockService.Object, _mockLogger.Object);
  }

  [Fact]
  public async Task GetAll_ReturnsOk_WhenSuccessful()
  {
    // Arrange
    _mockService.Setup(s => s.GetAllSuppliers())
        .ReturnsAsync(new Detail
        {
          IsSuccessful = true,
          Suppliers = new List<Supplier>(),
          Message = "Success",
          Status = ResponseStatus.Success
        });

    // Act
    var result = await _controller.GetAll();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var detail = Assert.IsType<Detail>(okResult.Value);
    Assert.True(detail.IsSuccessful);
  }

  [Fact]
  public async Task GetAll_ReturnsNoContent_WhenNotSuccessful()
  {
    _mockService.Setup(s => s.GetAllSuppliers())
        .ReturnsAsync(new Detail { IsSuccessful = false });

    var result = await _controller.GetAll();

    Assert.IsType<NoContentResult>(result);
  }

  [Fact]
  public async Task GetByNIT_ReturnsOk_WhenSupplierExists()
  {
    var nit = "123456";
    _mockService.Setup(s => s.GetSupplier(nit))
        .ReturnsAsync(new Detail
        {
          IsSuccessful = true,
          Status = ResponseStatus.Success,
          Suppliers = new List<Supplier> { new Supplier { NIT = nit } }
        });

    var result = await _controller.GetByNIT(nit);

    var okResult = Assert.IsType<OkObjectResult>(result);
    var detail = Assert.IsType<Detail>(okResult.Value);
    Assert.True(detail.IsSuccessful);
  }
  [Fact]
  public async Task GetByNIT_ReturnsNotFound_WhenSupplierDoesNotExist()
  {
    var nit = "000000";
    _mockService.Setup(s => s.GetSupplier(nit))
        .ReturnsAsync(new Detail
        {
          IsSuccessful = false,
          Status = ResponseStatus.NotFound
        });

    var result = await _controller.GetByNIT(nit);

    Assert.IsType<NotFoundObjectResult>(result);
  }
  [Fact]
  public async Task Save_ReturnsOk_WhenSuccessful()
  {
    var dto = new RegisterSupplierDto { NIT = "999999" };
    _mockService.Setup(s => s.SaveSupplier(dto))
        .ReturnsAsync(new Detail { IsSuccessful = true });

    var result = await _controller.Save(dto);

    var okResult = Assert.IsType<OkObjectResult>(result);
    var detail = Assert.IsType<Detail>(okResult.Value);
    Assert.True(detail.IsSuccessful);
  }
  [Fact]
  public async Task Save_ReturnsBadRequest_WhenFailed()
  {
    var dto = new RegisterSupplierDto { NIT = "999999" };
    _mockService.Setup(s => s.SaveSupplier(dto))
        .ReturnsAsync(new Detail
        {
          IsSuccessful = false,
          Message = "Ya existe"
        });

    var result = await _controller.Save(dto);

    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    var detail = Assert.IsType<Detail>(badRequest.Value);
    Assert.False(detail.IsSuccessful);
  }
  [Fact]
  public async Task Delete_ReturnsOk_WhenSuccessful()
  {
    var id = ObjectId.GenerateNewId().ToString();
    _mockService.Setup(s => s.DeleteSupplier(It.IsAny<ObjectId>()))
        .ReturnsAsync(new Detail { IsSuccessful = true });

    var result = await _controller.Delete(id);

    Assert.IsType<OkObjectResult>(result);
  }
  [Fact]
  public async Task Delete_ReturnsBadRequest_WhenIdInvalid()
  {
    var result = await _controller.Delete("invalid_id");

    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    var detail = Assert.IsType<Detail>(badRequest.Value);
    Assert.False(detail.IsSuccessful);
  }
  [Fact]
  public async Task Delete_ReturnsNotFound_WhenNotFound()
  {
    var id = ObjectId.GenerateNewId().ToString();
    _mockService.Setup(s => s.DeleteSupplier(It.IsAny<ObjectId>()))
        .ReturnsAsync(new Detail { IsSuccessful = false });

    var result = await _controller.Delete(id);

    Assert.IsType<NotFoundObjectResult>(result);
  }
  [Fact]
  public async Task Update_ReturnsOk_WhenSuccessful()
  {
    var id = ObjectId.GenerateNewId().ToString();
    var dto = new UpdateSupplierDto();

    _mockService.Setup(s => s.UpdateSupplier(It.IsAny<ObjectId>(), dto))
        .ReturnsAsync(new Detail { IsSuccessful = true });

    var result = await _controller.Update(id, dto);

    Assert.IsType<OkObjectResult>(result);
  }
  [Fact]
  public async Task Update_ReturnsBadRequest_WhenIdInvalid()
  {
    var dto = new UpdateSupplierDto();
    var result = await _controller.Update("invalid_id", dto);

    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    var detail = Assert.IsType<Detail>(badRequest.Value);
    Assert.False(detail.IsSuccessful);
  }
  [Fact]
  public async Task Update_ReturnsNotFound_WhenNotFound()
  {
    var id = ObjectId.GenerateNewId().ToString();
    var dto = new UpdateSupplierDto();

    _mockService.Setup(s => s.UpdateSupplier(It.IsAny<ObjectId>(), dto))
        .ReturnsAsync(new Detail { IsSuccessful = false });

    var result = await _controller.Update(id, dto);

    Assert.IsType<NotFoundObjectResult>(result);
  }

}
