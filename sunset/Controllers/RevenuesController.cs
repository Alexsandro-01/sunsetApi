using Microsoft.AspNetCore.Mvc;
using sunset.Service;
using sunset.Requests;
using sunset.Model;

namespace sunset.Controllers;

[ApiController]
[Route("[controller]")]
public class RevenuesController : ControllerBase
{

  protected IRevenueService _service;
  public RevenuesController(IRevenueService service)
  {
    _service = service;
  }

  [HttpGet]
  public ActionResult GetAll()
  {
    try
    {
      List<Revenue> revenue = _service.GetRevenueList();
      return Ok(revenue);
    }
    catch (System.Exception ex)
    {
      return BadRequest(new{
        message = ex.Message
      });
    }
  }

  [HttpGet("{id}")]
  public ActionResult GetById(int id)
  {
    try
    {
      Revenue revenue = _service.GetRevenue(id);
      
      return Ok(revenue);
    }
    catch (System.Exception ex)
    {
      return NotFound(new{
        message = ex.Message
      });
    }
  }

  [HttpPost]
  public ActionResult Create(RevenueRequest request)
  {
    try
    {
      Revenue revenue = _service.CreateRevenue(request);

      return StatusCode(201, revenue);
    }
    catch (System.Exception ex)
    { 
      return BadRequest(new{
        message = ex.Message
      });
    }
  }

  [HttpPut("{id}")]
  public ActionResult Update(int id, RevenueRequest request)
  {
    try
    {
      Revenue revenue = _service.UpdateRevenue(id, request);

      return Ok(revenue);
    }
    catch (System.Exception ex)
    {
      return NotFound(new{
        message = ex.Message
      });
      
    }
  }

  [HttpDelete("{id}")]
  public ActionResult Delete(int id)
  {
    try
    {
      _service.DeleteRevenue(id);

      return NoContent();
    }
    catch (System.Exception ex)
    {
      return NotFound(new{
        message = ex.Message
      });
    }
  }
}