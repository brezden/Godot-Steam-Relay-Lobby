using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

public interface ILobbyService
{
    void Initialize();
    void Update();
    Task StartHost();
    void JoinLobby(ulong lobbyId);
    void LeaveLobby();
    void InvitePlayer(ulong playerId);
    void SendLobbyMessage(string message);
    PlayerInfo GetLobbyMember(ulong playerId);
    LobbyMembersData GatherLobbyMembersData();
    void OpenInviteOverlay();
}
