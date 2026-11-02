using System.ComponentModel.DataAnnotations;

namespace TaburaiStore.Api.Models;

public sealed class Product(string productCode, string name, decimal price, int stock, string category)
{
  [Required(ErrorMessage = "Product code is required")]
  public string ProductCode { get; } = productCode;
  [Required(ErrorMessage = "Name is required")]
  public string Name { get; } = name;
  [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
  public decimal Price { get; } = price;
  [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
  public int Stock { get; } = stock;
  [Required(ErrorMessage = "Category is required")]
  public string Category { get; } = category;
}