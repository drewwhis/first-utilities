using System.Net.Http.Headers;
using System.Text;
using FIRST.Utilities.Options;
using FIRST.Utilities.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace FIRST.Utilities.Test.Services;

public class FtcApiServiceTests
{
    private static IHttpClientFactory GetMockFactory()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<FtcApiOptions>()
            .Build();

        var username = configuration.GetValue<string>("FtcApi:Username")!;
        var password = configuration.GetValue<string>("FtcApi:Password")!;
        
        var basicAuthenticationValue = Convert.ToBase64String(
            Encoding.ASCII.GetBytes(
                $"{username}:{password}"
            )
        );
        
        var mockFactory = new Mock<IHttpClientFactory>();
        var client = new HttpClient
        {
            BaseAddress = new Uri("https://ftc-api.firstinspires.org/v2.0/")
        };
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthenticationValue);
        mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
        return mockFactory.Object;
    }

    private static IOptions<FtcApiOptions> GetMockOptions()
    {
        var mockOptions = new Mock<IOptions<FtcApiOptions>>();
        return mockOptions.Object;
    }
    
    [Fact]
    public async Task Test_CanConnect()
    {
        var factory = GetMockFactory();
        var settings = GetMockOptions();
        var service = new FtcApiService(settings, factory);

        var result = await service.CanConnect();
        Assert.True(result);
    }
}