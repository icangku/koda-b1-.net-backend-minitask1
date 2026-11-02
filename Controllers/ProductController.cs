using Microsoft.AspNetCore.Mvc;
using TaburaiStore.Api.Data;
using TaburaiStore.Api.DTOs;
using TaburaiStore.Api.Models;

namespace TaburaiStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public sealed class ProductController : ControllerBase
{
  private readonly InMemoryProductCatalog _catalog;
  public ProductController(InMemoryProductCatalog catalog)
  {
    _catalog = catalog;
  }

  [HttpGet]
  [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
  public ActionResult<IReadOnlyList<Product>> GetAll()
  {
    var products = _catalog.GetAll().Select(ProductResponse.FromModel).ToArray();
    return Ok(products);
  }

  [HttpGet("{productCode}", Name = "GetProductByCode")]
  [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public ActionResult<ProductResponse> GetByCode([FromRoute] string productCode)
  {
    var product = _catalog.GetByCode(productCode);
    if(product is null)
    {
      return Problem(
        statusCode: StatusCodes.Status404NotFound,
        title: "Product Not Found",
        detail: $"No product was found for code {productCode}"
      );
    }
    return Ok(ProductResponse.FromModel(product));
  }

  [HttpPost]
  [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
  [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
  public ActionResult<ProductResponse> Create([FromBody] CreateProductRequest createProductRequest)
  {
    var product = _catalog.Add(new Product(
      createProductRequest.ProductCode,
      createProductRequest.Name,
      createProductRequest.Price,
      createProductRequest.Stock,
      createProductRequest.Category
    ));

    if(product is null)
    {
      return Conflict(new {message = $"Product code '{createProductRequest.ProductCode.Trim()}' already exits"});
    }
    
    return CreatedAtRoute("GetProductByCode", new {productCode = product.ProductCode}, ProductResponse.FromModel(product));
  }

  [HttpDelete("{productCode}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  public IActionResult Delete([FromRoute] string productCode)
  {
    if (!_catalog.Delete(productCode.Trim()))
    {
      return Problem(
        statusCode: StatusCodes.Status404NotFound,
        title: "Product Not Found",
        detail: $"No Product was found for code {productCode}"
      );
    }
    return NoContent();
  }

  [HttpPut("{productCode}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
  [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
  public ActionResult<ProductResponse> Update([FromRoute] string productCode ,[FromBody] UpdateProductRequest request)
  {
    var updated = _catalog.Update(new Product(
      productCode.Trim(),
      request.Name.Trim(),
      request.Price,
      request.Stock,
      request.Category.Trim()
    ));

    if(updated is null)
    {
      return Problem(
        statusCode: StatusCodes.Status404NotFound,
        title: "Product Not Found",
        detail: $"No product was found for code {productCode}"
      );
    }

    return Ok(ProductResponse.FromModel(updated));
  }

}