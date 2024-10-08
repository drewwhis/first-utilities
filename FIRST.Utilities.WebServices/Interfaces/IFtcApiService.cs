using FIRST.Utilities.WebServices.Models.FtcApi;

namespace FIRST.Utilities.WebServices.Interfaces;

public interface IFtcApiService
{
    Task<bool> CanConnect();
    Task<IEnumerable<EventDto>> FetchEventList();
    Task<IEnumerable<EventDto>> FetchEventList(bool areOfficial);
    Task<IEnumerable<EventDto>> FetchEventList(string regionCode, bool areOfficial);
    Task<IEnumerable<EventDto>> FetchEventList(string regionCode);
    Task<IEnumerable<EventDto>> FetchEventList(bool includeOfficialEvents, bool includeScrimmages, bool includeOtherEvents);
    Task<IEnumerable<EventDto>> FetchEventList(string regionCode, bool includeOfficialEvents, bool includeScrimmages, bool includeOtherEvents);
    Task<IEnumerable<TeamDto>> FetchTeams(string eventCode);
    Task<IEnumerable<MatchDto>> FetchMatches(string eventCode, ScheduleType scheduleType);
}