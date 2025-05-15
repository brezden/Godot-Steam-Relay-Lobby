using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager
{
    public static void SendLobbyMessage(string message)
    {
        _lobbyService.SendLobbyMessage(message);
        Logger.Network($"Lobby message sent: {message}");
    }

    public static void ReceiveLobbyMessage(string sender, string message)
    {
        var args = new LobbyMessageArgs
        {
            PlayerName = sender,
            Message = message
        };

        Logger.Network($"Lobby message received from {args.PlayerName}: {args.Message}");

        EventBus.Lobby.OnLobbyMessageReceived(args);
    }
}
