using Microsoft.AspNetCore.Mvc;
using sunset.Core;
using sunset.Requests;

namespace sunset.Controllers;

[ApiController]
[Route("[controller]")]
public class RevenuesController : ControllerBase
{
  private List<Revenue> revenues = new();
  private int NextId {get; set;} = 1;

  [HttpPost]
  public ActionResult CreateRevenue(RevenueRequest request)
  {
    Revenue revenue = new
    (
      NextId,
      request
    );

    NextId++;

    revenues.Add(revenue);

    return StatusCode(201, revenue);
  }
}