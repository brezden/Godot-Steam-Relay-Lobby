using System;
using System.Threading.Tasks;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
    public async Task CreateLobby(int maxPlayers)
    {
        var lobby = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
        if (!lobby.HasValue)
            throw new Exception();

        _lobbyId = lobby.Value.Id;
        lobby.Value.SetPrivate();
        lobby.Value.SetJoinable(true);
    }

    private static void RegisterHostCallbacks()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallback;
    }

    private static void OnLobbyCreatedCallback(Result result, Steamworks.Data.Lobby lobby)
    {
        LobbyManager.OnLobbyCreation(lobby.Id.ToString());
    }
}
