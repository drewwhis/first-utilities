using System.Net.Http.Headers;
using System.Text;
using FIRST.Utilities.Configuration;
using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.WebServices.Models.FtcApi;
using Microsoft.Extensions.Options;
using Moq;
using ScheduleType = FIRST.Utilities.WebServices.Models.FtcApi.ScheduleType;

namespace FIRST.Utilities.WebServices.Test;

public class FtcApiServiceTests
{
    private static IHttpClientFactory GetMockFactory()
    {
        var username = Environment.GetEnvironmentVariable("FtcApiUsername")!;
        var password = Environment.GetEnvironmentVariable("FtcApiPassword")!;

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

    private static IOptions<FtcApiOptions> GetFtcApiMockOptions()
    {
        var ftcApiOptionValues = new FtcApiOptions
        {
            BaseUrl = "https://ftc-api.firstinspires.org/v2.0/",
            EventsEndpoint = "{0}/events",
            TeamsEndpoint = "{0}/teams?eventCode={1}",
            ScheduleEndpoint = "{0}/schedule/{1}?tournamentLevel={2}"
        };

        var ftcApiMockOptions = new Mock<IOptions<FtcApiOptions>>();
        ftcApiMockOptions.Setup(o => o.Value).Returns(ftcApiOptionValues);
        return ftcApiMockOptions.Object;
    }

    private static IActiveProgramSeasonDataService GetProgramDataServiceMock()
    {
        var mock = new Mock<IActiveProgramSeasonDataService>();
        mock.Setup(s => s.GetActiveSeason("FTC")).Returns(new ActiveProgramSeason
        {
            ProgramCode = "FTC",
            SeasonYear = 2023
        });
        
        return mock.Object;
    }

    [Fact]
    public async Task Test_CanConnect()
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var result = await service.CanConnect();
        Assert.True(result);
    }

    [Fact]
    public async Task Test_FetchEventList()
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

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
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var result = await service.FetchEventList(regionCode, areOfficial);
        Assert.Equal(count, result.Count());
    }

    [Fact]
    public async Task Test_FetchEventList_WithRegion()
    {
        // ReSharper disable once StringLiteralTypo
        const string regionCode = "USAL";
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var result = await service.FetchEventList(regionCode);
        Assert.Equal(9, result.Count());
    }

    public static IEnumerable<object[]> FetchEventListIsOfficialTestCases
    {
        get
        {
            yield return [true, (Func<EventDto, bool>)(e => e.TypeName is "Championship" or "Qualifier")];
            yield return
                [false, (Func<EventDto, bool>)(e => e.TypeName != "Championship" && e.TypeName != "Qualifier")];
        }
    }

    [Theory]
    [MemberData(nameof(FetchEventListIsOfficialTestCases))]
    public async Task Test_FetchEventList_IsOfficial(bool areOfficial, Func<EventDto, bool> filter)
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var results = (await service.FetchEventList(areOfficial)).ToList();
        Assert.NotEmpty(results);
        Assert.True(results.All(filter));
    }

    public static IEnumerable<object[]> FetchEventListEventTypesTestCases
    {
        get
        {
            yield return
            [
                false, false, true,
                (Func<EventDto, bool>)(e =>
                    e.TypeName != "Championship" && e.TypeName != "Qualifier" && e.TypeName != "Scrimmage")
            ];
            yield return [false, true, false, (Func<EventDto, bool>)(e => e.TypeName is "Scrimmage")];
            yield return
            [
                false, true, true,
                (Func<EventDto, bool>)(e => e.TypeName != "Championship" && e.TypeName != "Qualifier")
            ];
            yield return [true, false, false, (Func<EventDto, bool>)(e => e.TypeName is "Championship" or "Qualifier")];
            yield return
            [
                true, false, true,
                (Func<EventDto, bool>)(e => e.TypeName is "Championship" or "Qualifier" or not "Scrimmage")
            ];
            yield return
            [
                true, true, false,
                (Func<EventDto, bool>)(e => e.TypeName is "Championship" or "Qualifier" or "Scrimmage")
            ];
            yield return [true, true, true, (Func<EventDto, bool>)(e => !string.IsNullOrWhiteSpace(e.TypeName))];
        }
    }

    [Theory]
    [MemberData(nameof(FetchEventListEventTypesTestCases))]
    public async Task Test_FetchEventList_EventTypes(bool includeOfficial, bool includeScrimmages,
        bool includeOtherEvents, Func<EventDto, bool> filter)
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var results = (await service.FetchEventList(includeOfficial, includeScrimmages, includeOtherEvents)).ToList();
        Assert.NotEmpty(results);
        Assert.True(results.All(filter));
    }

    [Fact]
    public async Task Test_FetchEventList_NoEventTypes()
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var results = (await service.FetchEventList(false, false, false)).ToList();
        Assert.Empty(results);
    }

    [Theory]
    [InlineData(false, false, false, 0)]
    [InlineData(false, false, true, 4)]
    [InlineData(false, true, false, 4)]
    [InlineData(false, true, true, 8)]
    [InlineData(true, false, false, 1)]
    [InlineData(true, false, true, 5)]
    [InlineData(true, true, false, 5)]
    [InlineData(true, true, true, 9)]
    public async Task Test_FetchEventList_EventTypes_WithRegion(bool includeOfficial, bool includeScrimmage,
        bool includeOthers, int count)
    {
        // ReSharper disable once StringLiteralTypo
        const string regionCode = "USAL";
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var result = await service.FetchEventList(regionCode, includeOfficial, includeScrimmage, includeOthers);
        Assert.Equal(count, result.Count());
    }
    
    [Fact]
    public async Task Test_FetchTeams_USALCMP()
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var results = (await service.FetchTeams("USALCMP")).ToList();
        Assert.Equal(16, results.Count);
    }

    [Theory]
    [InlineData(ScheduleType.qual, 24)]
    [InlineData(ScheduleType.playoff, 6)]
    public async Task Test_FetchMatches_USALCMP(ScheduleType scheduleType, int count)
    {
        var factory = GetMockFactory();
        var settings = GetProgramDataServiceMock();
        var apiSettings = GetFtcApiMockOptions();
        var service = new FtcApiService(apiSettings, settings, factory);

        var results = (await service.FetchMatches("USALCMP", scheduleType)).ToList();
        Assert.Equal(count, results.Count);
    }
}