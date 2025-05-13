using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public interface ILobbyService
{
    void Initialize();
    void Update();
    bool IsLobbyActive();
    Task CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    void LeaveLobby();
    void InvitePlayer(string playerId);
    void SendLobbyMessage(string message);
    LobbyMembersData GatherLobbyMembersData();
    Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends();
}
