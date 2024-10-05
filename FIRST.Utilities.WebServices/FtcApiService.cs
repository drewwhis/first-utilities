using System.Net.Http.Json;
using FIRST.Utilities.Configuration;
using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.WebServices.Interfaces;
using FIRST.Utilities.WebServices.Models.FtcApi;
using Microsoft.Extensions.Options;

namespace FIRST.Utilities.WebServices;

public class FtcApiService(
    IOptions<FtcApiOptions> ftcApiSettings,
    IActiveProgramSeasonDataService activeProgramSeasonDataService,
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
        var ftcRecord = activeProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null) return [];
        
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(_ftcApiSettings.EventsEndpoint, ftcRecord.SeasonYear);

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
        var ftcRecord = activeProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null) return [];
        
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(_ftcApiSettings.TeamsEndpoint, ftcRecord.SeasonYear, eventCode);

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

    public async Task<IEnumerable<MatchDto>> FetchMatches(string eventCode, ScheduleType scheduleType)
    {
        var ftcRecord = activeProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null) return [];
        
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(
            _ftcApiSettings.ScheduleEndpoint, 
            ftcRecord.SeasonYear, 
            eventCode, 
            scheduleType
        );
        
        try
        {
            var response = await client.GetFromJsonAsync<MatchScheduleDto>(endpoint);
            if (response is null) return [];
            return response.Schedule ?? [];
        }
        catch
        {
            return [];
        }
    }
}