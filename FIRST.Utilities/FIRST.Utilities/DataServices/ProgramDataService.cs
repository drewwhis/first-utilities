using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class ProgramDataService(IProgramRepository repository) : IProgramDataService
{
    public Models.Database.Program? GetProgram(string programCode)
    {
        return repository.Get(programCode);
    }
}