using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class ActiveProgramSeasonDataService(IActiveProgramSeasonRepository repository) : IActiveProgramSeasonDataService
{
    public ActiveProgramSeason? GetActiveSeason(string programCode)
    {
        return repository.GetActiveProgramSeason(programCode);
    }
}