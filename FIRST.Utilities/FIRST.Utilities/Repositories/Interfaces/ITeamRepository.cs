using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface ITeamRepository
{
    Task<bool> Create(Team model);
    Task<bool> Update(Team model);
    Team? Get(string programCode, int teamNumber, int seasonYear);
    IEnumerable<Team> Filter(string programCode, int seasonYear);
}