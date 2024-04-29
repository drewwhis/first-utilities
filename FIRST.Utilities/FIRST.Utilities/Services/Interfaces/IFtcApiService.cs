using FIRST.Utilities.Models.FtcApi;

namespace FIRST.Utilities.Services.Interfaces;

public interface IFtcApiService
{
    Task<bool> CanConnect();
    Task<IEnumerable<EventDto>> FetchEventList();
    Task<IEnumerable<EventDto>> FetchEventList(bool areOfficial);
    Task<IEnumerable<EventDto>> FetchEventList(string regionCode, bool areOfficial);
    Task<IEnumerable<EventDto>> FetchEventList(string regionCode);
}