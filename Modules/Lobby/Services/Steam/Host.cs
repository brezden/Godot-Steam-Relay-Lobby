using System;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Core;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService
{
    public async Task StartHost()
    {
        await CreateLobby();
        TransportManager.Server.CreateServer();
        await LobbyManager.GatherLobbyMembers();
    }
    
    private async Task CreateLobby()
    {
        var lobby = await SteamMatchmaking.CreateLobbyAsync(4);
        if (!lobby.HasValue)
            throw new Exception();

        _lobby = lobby.Value;
        _lobbyId = _lobby.Id;
        lobby.Value.SetPrivate();
        lobby.Value.SetJoinable(true);
        Logger.Lobby($"Lobby created ({_lobbyId})");
    }

    public void SetServerId(string serverId)
    {
        _lobby.SetGameServer(ConvertStringToSteamId(serverId));
    }
}
