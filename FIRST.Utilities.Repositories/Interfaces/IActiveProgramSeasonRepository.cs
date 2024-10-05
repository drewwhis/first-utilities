using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IActiveProgramSeasonRepository
{
    ActiveProgramSeason? GetActiveProgramSeason(string programCode);
}