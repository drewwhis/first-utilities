using FIRST.Utilities.Models.FtcApi;
using FIRST.Utilities.Options;
using FIRST.Utilities.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FIRST.Utilities.Services;

public class FtcApiService(
    IOptions<FtcApiOptions> settings,
    IHttpClientFactory factory)
    : IFtcApiService
{
    private readonly FtcApiOptions _settings = settings.Value;
    private static readonly string ApiClientName = "ftc-api";

    public async Task<bool> CanConnect()
    {
        var client = factory.CreateClient(ApiClientName);
        var response = await client.GetAsync(string.Empty);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<EventDto>> FetchEventList()
    {
        var client = factory.CreateClient(ApiClientName);
        var endpoint = string.Format(_settings.EventsEndpoint, _settings.CurrentSeason);
        var response = await client.GetFromJsonAsync<EventsDto>(endpoint);
        return response?.Events ?? Enumerable.Empty<EventDto>();
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
}