using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcMatchDataService
{
    IEnumerable<FtcMatch> GetByScheduleType(ScheduleType scheduleType);
    bool AreQualificationMatchesPresent();
    bool ArePlayoffMatchesPresent();
    Task<bool> CreateMatch(FtcMatch match, Dictionary<(Alliance, int), int> teams);
    Task<bool> ClearQualificationMatches();
    Task<bool> ClearPlayoffMatches();
    Task<bool> SetActiveMatch(int matchId);
    Task<ActiveFtcMatch?> GetActiveMatch();
    Task<FtcMatch?> GetNextQualificationMatch(FtcMatch match);
    Task<FtcMatch?> GetPreviousQualificationMatch(FtcMatch match);
}