using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IFtcMatchParticipantRepository
{
    IEnumerable<FtcMatchParticipant> GetAll();
    FtcMatchParticipant? Get(int participantId);
    FtcMatchParticipant? GetByMatchTeam(int matchId, int teamId);
    FtcMatchParticipant? GetByAllianceStation(Alliance alliance, int station);
    Task<bool> Update(FtcMatchParticipant ftcMatchParticipant);
    Task<bool> Create(FtcMatchParticipant ftcMatchParticipant);
    Task<bool> ClearMatchParticipants(HashSet<int> matchIds);
}