using TaburaiStore.Api.Models;

namespace TaburaiStore.Api.Data;

public sealed class InMemoryProductCatalog
{
  private readonly List<Product> _products = [];
  public IReadOnlyList<Product> GetAll()
  {
    return [.. _products];
  }

  public Product? GetByCode(string productCode)
  {
    return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
  }

  public Product? Add(Product product)
  {
    if(_products.Any(p => string.Equals(p.ProductCode, product.ProductCode, StringComparison.OrdinalIgnoreCase))){
      return null;
    }

    _products.Add(product);
    return product;
  }

  public bool Delete(string productCode)
  {
    return _products.RemoveAll(p => string.Equals(p.ProductCode, productCode, StringComparison.OrdinalIgnoreCase)) > 0;
  }

  public Product? Update(Product product)
  {
    var index = _products.FindIndex(p => string.Equals(p.ProductCode, product.ProductCode, StringComparison.OrdinalIgnoreCase));
    if(index < 0) return null;

    _products[index] = product;
    return product;
  }
}