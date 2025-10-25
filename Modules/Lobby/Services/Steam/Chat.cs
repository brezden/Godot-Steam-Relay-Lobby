using System.Runtime.CompilerServices;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService 
{
    private Steam.LobbyMessageEventHandler OnChatMessageHandler;
    
    private void RegisterChatCallbacks()
    {
        OnChatMessageHandler += OnLobbyChatMessage;
    }
    
    public void SendLobbyMessage(string message)
    {
        bool was_sent = Steam.SendLobbyChatMsg(_lobbyId, message);
        
        if (!was_sent)
        {
            // IDK when this would fail tbh but just in case I want a log...
            Logger.Error("Failed to send lobby chat message.");
        }
    }

    private static void OnLobbyChatMessage(ulong lobbyId, long user, string message, long chatType)
    {
        var userName = Steam.GetFriendPersonaName((ulong) user);
        LobbyManager.ReceiveLobbyMessage(userName, message);
    }
}
