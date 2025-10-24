using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService 
{
    public void SendLobbyMessage(string message)
    {
        _lobby.SendChatString(message);
    }

    private static void RegisterChatCallbacks()
    { 
        OnChatMessage += OnLobbyChatMessage;
    }

    private static void OnLobbyChatMessage(Steamworks.Data.Lobby lobby, Friend friend, string message)
    {
        LobbyManager.ReceiveLobbyMessage(friend.Name, message);
    }
}
