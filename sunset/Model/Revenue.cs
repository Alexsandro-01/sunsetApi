using sunset.Requests;

namespace sunset.Model;
public class Revenue
{

  public int Id {get; set;}
  public DateTime Date {get; set;}
  public string? Culture {get; set;}
  public string? Unity {get; set;}
  public float Quantity {get; set;}
  public Decimal UnityValue {get; set;}
  public Decimal TotalValue {get; set;}

  public Revenue(int id, RevenueRequest request)
  {
    Id = id;
    
    if (request.Date is not null)
      Date = DateTime.Parse(request.Date);

    Culture = request.Culture;
    Unity = request.Unity;
    Quantity = request.Quantity;
    UnityValue = request.UnityValue;
    TotalValue = request.TotalValue;
  }

  public Revenue() { }

}