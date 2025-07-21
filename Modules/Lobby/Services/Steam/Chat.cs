using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService 
{
    public void SendLobbyMessage(string message)
    {
        _lobby.SendChatString(message);
    }

    private static void RegisterChatCallbacks()
    {
        SteamMatchmaking.OnChatMessage += OnLobbyChatMessage;
    }

    private static void OnLobbyChatMessage(Steamworks.Data.Lobby lobby, Friend friend, string message)
    {
        LobbyManager.ReceiveLobbyMessage(friend.Name, message);
    }
}
