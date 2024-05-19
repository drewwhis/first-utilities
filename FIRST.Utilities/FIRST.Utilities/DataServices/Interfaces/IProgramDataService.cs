namespace FIRST.Utilities.DataServices.Interfaces;

public interface IProgramDataService
{
    Models.Database.Program? GetProgram(string programCode);
}