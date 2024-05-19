namespace FIRST.Utilities.Repositories.Interfaces;

public interface IProgramRepository
{
    Models.Database.Program? Get(string programCode);
}