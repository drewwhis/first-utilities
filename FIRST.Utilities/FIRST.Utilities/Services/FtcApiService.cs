using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.FtcApi;
using FIRST.Utilities.Options;
using FIRST.Utilities.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FIRST.Utilities.Services;

public class FtcApiService(
    IOptions<FtcApiOptions> ftcApiSettings,
    IProgramDataService programDataService,
    IHttpClientFactory factory)
    : IFtcApiService
{
    private readonly FtcApiOptions _ftcApiSettings = ftcApiSettings.Value;
    private const string ApiClientName = "ftc-api";

    public async Task<bool> CanConnect()
    {
        var client = factory.CreateClient(ApiClientName);
        var response = await client.GetAsync(string.Empty);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<EventDto>> FetchEventList()
    {
        var ftcRecord = programDataService.GetProgram("FTC");
        if (ftcRecord is null) return [];
        
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(_ftcApiSettings.EventsEndpoint, ftcRecord.ActiveSeasonYear);

        try
        {
            var response = await client.GetFromJsonAsync<EventsDto>(endpoint);
            if (response is null) return [];
            return response.Events ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<IEnumerable<EventDto>> FetchEventList(bool areOfficial)
    {
        var types = new HashSet<string> { "Qualifier", "Championship" };
        var results = await FetchEventList();
        return results.Where(e => !(areOfficial ^ types.Contains(e.TypeName)));
    }

    public async Task<IEnumerable<EventDto>> FetchEventList(string regionCode, bool areOfficial)
    {
        return (await FetchEventList(areOfficial)).Where(e => e.RegionCode == regionCode).ToList();
    }

    public async Task<IEnumerable<EventDto>> FetchEventList(string regionCode)
    {
        return (await FetchEventList()).Where(e => e.RegionCode == regionCode).ToList();
    }

    public async Task<IEnumerable<EventDto>> FetchEventList(bool includeOfficialEvents, bool includeScrimmages,
        bool includeOtherEvents)
    {
        return (await FetchEventList())
            .Where(e => (includeOfficialEvents && e.TypeName is "Championship" or "Qualifier")
                        || (includeScrimmages && e.TypeName == "Scrimmage")
                        || (includeOtherEvents && e.TypeName != "Scrimmage" && e.TypeName != "Championship" &&
                            e.TypeName != "Qualifier"))
            .ToList();
    }

    public async Task<IEnumerable<EventDto>> FetchEventList(string regionCode, bool includeOfficialEvents,
        bool includeScrimmages, bool includeOtherEvents)
    {
        return (await FetchEventList(includeOfficialEvents, includeScrimmages, includeOtherEvents))
            .Where(e => e.RegionCode == regionCode)
            .ToList();
    }

    public async Task<IEnumerable<TeamDto>> FetchTeams(string eventCode)
    {
        var ftcRecord = programDataService.GetProgram("FTC");
        if (ftcRecord is null) return [];
        
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(_ftcApiSettings.TeamsEndpoint, ftcRecord.ActiveSeasonYear, eventCode);

        try
        {
            var response = await client.GetFromJsonAsync<TeamsDto>(endpoint);
            if (response is null) return [];
            return response.Teams ?? [];
        }
        catch
        {
            return [];
        }
    }
}