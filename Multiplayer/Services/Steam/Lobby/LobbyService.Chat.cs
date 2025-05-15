using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
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
        LobbyManager.OnLobbyMessageReceived(friend.Name, message);
    }
}
