using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcMatchDataService
{
    bool AreQualificationMatchesPresent();
    Task<bool> UpsertMatch(FtcMatch ftcMatch);
}