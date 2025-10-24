using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public interface ILobbyService
{
    void Initialize();
    void Update();
    Task StartHost();
    Task StartGuest(string lobbyId);
    void LeaveLobby();
    void InvitePlayer(ulong playerId);
    void SendLobbyMessage(string message);
    void EnterLobbyScene();
    Task<PlayerInfo> GetPlayerInfo(ulong playerId);
    Task<LobbyMembersData> GatherLobbyMembersData();
    Task<List<PlayerInvite>> GetInGameFriends();
}
