namespace sunset.Service;

using sunset.Model;
using sunset.Requests;

public class RevenueService : IRevenueService
{
  private List<Revenue> _revenues {get; set;}
  private int _nextId {get; set;}

  public RevenueService()
  {
    _revenues = new();
    _nextId = 1;
  }

  public List<Revenue> GetRevenueList()
  {
    return _revenues;
  }

  public Revenue GetRevenue(int id)
  {
    var revenue = _revenues.Find(r => r.Id == id) 
      ?? throw new Exception("Revenue not found");

    return revenue;
  }

  public Revenue CreateRevenue(RevenueRequest request)
  {
    Revenue revenue = new
    (
      _nextId++,
      request
    );

    _revenues.Add(revenue);

    return revenue;
  }

  public Revenue UpdateRevenue(int id, RevenueRequest request)
  {
    var revenue = _revenues.Find(r => r.Id == id)
      ?? throw new Exception("Revenue not found");

    Revenue updatedRevenue = new
    (
      id,
      request
    );

    _revenues.Remove(revenue);
    _revenues.Add(updatedRevenue);

    return updatedRevenue;
  }

  public void DeleteRevenue(int id)
  {
    var revenue = _revenues.Find(r => r.Id == id)
      ?? throw new Exception("Revenue not found");
    
    _revenues.Remove(revenue);
  }
}