using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class ActiveProgramSeasonRepository(ApplicationDbContext context) : IActiveProgramSeasonRepository
{
    public ActiveProgramSeason? GetActiveProgramSeason(string programCode)
    {
        return context.ActiveProgramSeasons.FirstOrDefault(p => p.ProgramCode == programCode);
    }
}