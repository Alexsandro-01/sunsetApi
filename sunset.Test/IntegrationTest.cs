
using System.Collections;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualBasic;
using sunset.Controllers;
using sunset.Core;
using sunset.Requests;
using Xunit;

namespace sunset.Test;
public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
  public HttpClient _webClientTest;

  public IntegrationTest(WebApplicationFactory<Program> factory)
  {
    _webClientTest = factory.CreateClient();
  }

  [Theory(DisplayName = "Testing route /Get Revenues")]
  [InlineData("/revenues")]
  public async Task TestGetRevenues(string url)
  {
    var response = await _webClientTest.GetAsync(url);

    response.StatusCode.Equals(HttpStatusCode.OK);
  }
}