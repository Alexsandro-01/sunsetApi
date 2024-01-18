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

  [Theory(DisplayName = "Testing route /GET revenues")]
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
    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    AssertResponseContent(revenuesMoq[0], jsonResponse[0]);
    AssertResponseContent(revenuesMoq[1], jsonResponse[1]);
  }

  [Theory(DisplayName = "Testing route /POST revenues")]
  [InlineData("/revenues")]
  public async Task TestCreateRevenue(string url)
  {
    // Arrange
    RevenueRequest requestEx = RequestExemple("abóbora", "tela");
    Revenue revenueMoq = new(3, requestEx);
    mockService.Setup(s => s.CreateRevenue(It.IsAny<RevenueRequest>())).Returns(revenueMoq);

    // Act
    var response = await _webClientTest.PostAsync
    (
      url,
      new StringContent
      (
        JsonConvert.SerializeObject(requestEx),
        System.Text.Encoding.UTF8,
        "application/json"
      )
    );
    var responseString = await response.Content.ReadAsStringAsync();
    Revenue jsonResponse = JsonConvert.DeserializeObject<Revenue>(responseString)!;

    // Assert
    Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    AssertResponseContent(revenueMoq, jsonResponse);
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

  protected static void AssertResponseContent(Revenue revenueMoq, Revenue response)
  {
    Assert.Equal(revenueMoq.Id, response.Id);
    Assert.Equal(revenueMoq.Date, response.Date);
    Assert.Equal(revenueMoq.Culture, response.Culture);
    Assert.Equal(revenueMoq.Unity, response.Unity);
    Assert.Equal(revenueMoq.UnityValue, response.UnityValue);
    Assert.Equal(revenueMoq.TotalValue, response.TotalValue);
  }
  
}