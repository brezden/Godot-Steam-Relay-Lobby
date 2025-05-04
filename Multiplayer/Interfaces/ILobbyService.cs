using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILobbyService
{
    void Initialize();
    void Update();
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    void LeaveLobby();
    void InviteLobbyOverlay();
    void SendLobbyMessage(string message);
    
    Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends();
}