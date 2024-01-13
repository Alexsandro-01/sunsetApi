using Microsoft.AspNetCore.Mvc;
using sunset.Core;
using sunset.Requests;

namespace sunset.Controllers;

[ApiController]
[Route("[controller]")]
public class RevenuesController : ControllerBase
{
  private static List<Revenue> revenues = new();
  private static int NextId {get; set;} = 1;

  [HttpGet]
  public ActionResult GetAll()
  {
    return Ok(revenues);
  }

  [HttpGet("{id}")]
  public ActionResult GetById(int id)
  {
    var revenue = revenues.Find(r => r.Id == id);

    if (revenue is null)
      return NotFound(new {
        message = "Revenue not found!"
      });

    return Ok(revenue);
  }

  [HttpPost]
  public ActionResult Create(RevenueRequest request)
  {
    Revenue revenue = new
    (
      NextId++,
      request
    );

    revenues.Add(revenue);

    return StatusCode(201, revenue);
  }

  [HttpPut("{id}")]
  public ActionResult Update(int id, RevenueRequest request)
  {
    var revenue = revenues.Find(r => r.Id == id);

    if (revenue is null)
      return NotFound(new {
        message = "Revenue not found!"
      });
    
    Revenue updatedRevenue = new
    (
      id,
      request
    );

    revenues.Remove(revenue);
    revenues.Add(updatedRevenue);

    return Ok(updatedRevenue);
  }

  [HttpDelete("{id}")]
  public ActionResult Delete(int id)
  {
    var revenue = revenues.Find(r => r.Id == id);

    if (revenue is null)
      return NotFound(new {
        message = "Revenue not found!"
      });
    
    revenues.Remove(revenue);

    return NoContent();
  }
}