using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager
{
    public static void SendLobbyMessage(string message)
    {
        _lobbyService.SendLobbyMessage(message);
        Logger.Lobby($"Lobby message sent: {message}");
    }

    public static void ReceiveLobbyMessage(string sender, string message)
    {
        var args = new LobbyMessageArgs
        {
            PlayerName = sender,
            Message = message
        };

        Logger.Lobby($"Lobby message received from {args.PlayerName}: {args.Message}");

        EventBus.Lobby.OnLobbyMessageReceived(args);
    }
}
