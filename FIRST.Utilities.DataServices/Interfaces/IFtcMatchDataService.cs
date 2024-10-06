using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcMatchDataService
{
    bool AreQualificationMatchesPresent();
    Task<bool> CreateMatch(FtcMatch match, Dictionary<(Alliance, int), int> teams);
    Task<bool> ClearQualificationMatches();
}