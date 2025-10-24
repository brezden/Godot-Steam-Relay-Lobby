using System;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Modules;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotSteam;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService 
{
    private static void RegisterGuestCallbacks()
    {
        Steam.LobbyJoined += OnGameLobbyJoinRequested()
            Steam.LobbyJoinedEventHandler
    }
    
    public async Task StartGuest(string lobbyId)
    {
        await JoinLobby(lobbyId);
        await LobbyManager.GatherLobbyMembers();
        TransportManager.Client.ConnectToServer();
    }

    private async Task JoinLobby(string lobbyId)
    {
        var result = await SteamMatchmaking.JoinLobbyAsync(ConvertStringToSteamId(lobbyId));
        
        if (!result.HasValue)
        {
            throw new Exception("Failed to join lobby");
        }

        _lobby = result.Value;
    }

    private static void OnGameLobbyJoinRequested(Steamworks.Data.Lobby lobby, SteamId id)
    {
        Logger.Network($"Lobby join accepted for {lobby.Id} by {id}");
        EventBus.Lobby.OnStartGuest(lobby.Id.ToString());
    }
}
