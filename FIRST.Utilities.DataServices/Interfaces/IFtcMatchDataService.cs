using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcMatchDataService
{
    bool AreQualificationMatchesPresent();
    bool ArePlayoffMatchesPresent();
    Task<bool> CreateMatch(FtcMatch match, Dictionary<(Alliance, int), int> teams);
    Task<bool> ClearQualificationMatches();
    Task<bool> ClearPlayoffMatches();
}