using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILobbyService
{
    void Initialize();
    void Update();
    Task CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    void LeaveLobby();
    void InviteLobbyOverlay();
    void InvitePlayer(string playerId);
    void SendLobbyMessage(string message);
    
    Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends();
}