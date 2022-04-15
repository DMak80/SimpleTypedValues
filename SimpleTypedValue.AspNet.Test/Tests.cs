using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace SimpleTypedValue.AspNet.Test;

public class Tests
{
    public HttpClient CreateClient<T>()
        where T : class
    {
        var server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseStartup<T>());
        var client = server.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    [Fact]
    public void Test_AspNetModelBinder()
    {
        using var client = CreateClient<TestStartup>();
        Test(client);
    }

    [Fact]
    public void Test_AspNetModelBinderWithNewtonsoftJson()
    {
        using var client = CreateClient<TestStartupWithNewtonsoftJson>();
        Test(client);
    }

    private void Test(HttpClient client)
    {
        var response = client.GetAsync("/test/123").Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<long>(content);
        Assert.Equal(123L, result);
    }
}