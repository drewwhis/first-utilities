using System.Net.Http.Headers;
using System.Text;
using FIRST.Utilities.Models.FtcApi;
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
        var optionValues = new FtcApiOptions
        {
            CurrentSeason = 2023,
            EventsEndpoint = "{0}/events"
        };
        
        var mockOptions = new Mock<IOptions<FtcApiOptions>>();
        mockOptions.Setup(o => o.Value).Returns(optionValues);
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

    [Fact]
    public async Task Test_FetchEventList()
    {
        var factory = GetMockFactory();
        var settings = GetMockOptions();
        var service = new FtcApiService(settings, factory);

        var result = await service.FetchEventList();
        Assert.NotEmpty(result);
    }
    
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 8)]
    public async Task Test_FetchEventList_WithRegion_IsOfficial(bool areOfficial, int count)
    {
        // ReSharper disable once StringLiteralTypo
        const string regionCode = "USAL";
        var factory = GetMockFactory();
        var settings = GetMockOptions();
        var service = new FtcApiService(settings, factory);
        
        var result = await service.FetchEventList(regionCode, areOfficial);
        Assert.Equal(count, result.Count());
    }
    
    [Fact]
    public async Task Test_FetchEventList_WithRegion()
    {
        // ReSharper disable once StringLiteralTypo
        const string regionCode = "USAL";
        var factory = GetMockFactory();
        var settings = GetMockOptions();
        var service = new FtcApiService(settings, factory);
        
        var result = await service.FetchEventList(regionCode);
        Assert.Equal(9, result.Count());
    }
    
    public static IEnumerable<object[]> FetchEventListIsOfficialTestCases
    {
        get
        {
            yield return [true, (Func<EventDto, bool>)(e => e.TypeName is "Championship" or "Qualifier")];
            yield return [false, (Func<EventDto, bool>)(e => e.TypeName != "Championship" && e.TypeName != "Qualifier")];
        }
    }
    
    [Theory]
    [MemberData(nameof(FetchEventListIsOfficialTestCases))]
    public async Task Test_FetchEventList_IsOfficial(bool areOfficial, Func<EventDto, bool> filter)
    {
        var factory = GetMockFactory();
        var settings = GetMockOptions();
        var service = new FtcApiService(settings, factory);
        
        var results = (await service.FetchEventList(areOfficial)).ToList();
        Assert.NotEmpty(results);
        Assert.True(results.All(filter));
    }
}