using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public interface ILobbyService
{
    void Initialize();
    void Update();
    Task CreateLobby(int maxPlayers);
    void LeaveLobby();
    void SetServerId(string serverId);
    string GetServerId();
    void InvitePlayer(string playerId);
    void SendLobbyMessage(string message);
    void EnterLobbyScene();
    Task<PlayerInfo> GetPlayerInfo(string playerId);
    Task<LobbyMembersData> GatherLobbyMembersData();
    Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends();
}
