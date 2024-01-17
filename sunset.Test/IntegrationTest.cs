using System.Reflection.Metadata.Ecma335;

namespace sunset.Test;

using Moq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using sunset.Controllers;
using sunset.Service;
using sunset.Model;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using sunset.Requests;
using System;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
  public HttpClient _webClientTest;
  public Mock<IRevenueService> mockService;

  public IntegrationTest(WebApplicationFactory<Program> factory)
  {
    mockService = new Mock<IRevenueService>();

    _webClientTest = factory.WithWebHostBuilder(builder => {
      builder.ConfigureServices(services => {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IRevenueService));

        if (descriptor is not null)
          services.Remove(descriptor);

        services.AddSingleton(mockService.Object);
      });
    }).CreateClient();
  }

  [Theory(DisplayName = "Testing route /GET Revenues")]
  [InlineData("/revenues")]
  public async Task TestGetRevenue(string url)
  {
    // Arrange
    List<Revenue> revenuesMoq = new();
    
    Revenue revenue1 = new 
    (
      1,
      RequestExemple("example1", "kg")
    );

    Revenue revenue2 = new
    (
      2,
      RequestExemple("example2", "cx")
    );

    revenuesMoq.Add(revenue1);
    revenuesMoq.Add(revenue2);

    mockService.Setup(s => s.GetRevenueList()).Returns(revenuesMoq);

    // Act
    var response = await _webClientTest.GetAsync(url);
    var responseString = await response.Content.ReadAsStringAsync();
    Revenue[] jsonResponse = JsonConvert.DeserializeObject<Revenue[]>(responseString)!;

    // Assert
    Assert.Equal(revenuesMoq[1].Id, jsonResponse[1].Id);
    Assert.Equal(revenuesMoq[1].Date, jsonResponse[1].Date);
    Assert.Equal(revenuesMoq[1].Culture, jsonResponse[1].Culture);
    Assert.Equal(revenuesMoq[1].UnityValue, jsonResponse[1].UnityValue);
    Assert.Equal(revenuesMoq[1].Unity, jsonResponse[1].Unity);
    Assert.Equal(revenuesMoq[1].TotalValue, jsonResponse[1].TotalValue);

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
  }

  protected static RevenueRequest RequestExemple(string culture, string unity)
  {
    RevenueRequest request = new()
    {
      Culture = culture,
      Unity = unity,
      UnityValue = 4,
      Quantity = 10,
      TotalValue = 40,
      Date = "2024-01-07T00:00:00"
    };

    return request;
  }
  
}