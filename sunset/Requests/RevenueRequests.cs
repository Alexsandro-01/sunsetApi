using sunset.Model;

namespace sunset.Requests;

public class RevenueRequest
{
  public string? Date {get; set;}
  public string? Culture {get; set;}
  public string? Unity {get; set;}
  public float Quantity {get; set;}
  public Decimal UnityValue {get; set;}
  public Decimal TotalValue {get; set;}
}