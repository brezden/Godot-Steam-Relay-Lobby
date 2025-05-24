using System;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
    private static void RegisterHostCallbacks()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallback;
    }
    
    public async Task CreateLobby(int maxPlayers)
    {
        var lobby = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
        if (!lobby.HasValue)
            throw new Exception();

        _lobbyId = lobby.Value.Id;
        lobby.Value.SetPrivate();
        lobby.Value.SetJoinable(true);
    }

    private static void OnLobbyCreatedCallback(Result result, Steamworks.Data.Lobby lobby)
    {
        EventBus.Lobby.OnLobbyCreated(lobby.Id.ToString());
    }

    public void SetServerId(string serverId)
    {
        _lobby.SetGameServer(ConvertStringToSteamId(serverId));
    }
}
