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

    public async Task<bool> CanConnect()
    {
        var client = factory.CreateClient("ftc-api");
        var response = await client.GetAsync(string.Empty);
        return response.IsSuccessStatusCode;
    }
}