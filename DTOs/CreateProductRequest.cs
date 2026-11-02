using System.ComponentModel.DataAnnotations;

namespace TaburaiStore.Api.DTOs;

public sealed class CreateProductRequest()
{
  [Required, StringLength(30)]
  public string ProductCode{get; init;} = string.Empty;
  [Required, StringLength(100)]
  public string Name{get; init;} = string.Empty;
  [Range(typeof(decimal), "0", "1000000000")]
  public required decimal Price{get; init;}
  [Range(0, int.MaxValue)]
  public required int Stock {get; init;}
  [Required, StringLength(100)]
  public string Category {get; init;} = string.Empty;
}