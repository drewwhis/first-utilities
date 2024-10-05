using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IActiveProgramSeasonDataService
{
    ActiveProgramSeason? GetActiveSeason(string programCode);
}