namespace sunset.Service;

using sunset.Model;
using sunset.Requests;

public interface IRevenueService
{
  List<Revenue> GetRevenueList();
  Revenue GetRevenue(int id);
  Revenue CreateRevenue(RevenueRequest request);
  Revenue UpdateRevenue(int id, RevenueRequest request);
  void DeleteRevenue(int id);
}

