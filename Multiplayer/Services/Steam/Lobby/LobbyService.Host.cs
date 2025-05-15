namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

using System;
using System.Threading.Tasks;
using Steamworks;

public partial class LobbyService: ILobbyService
{
    private void RegisterHostCallbacks()
    {
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallback;
    } 
    
    public async Task CreateLobby(int maxPlayers)
    {
        var lobby = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
        if (!lobby.HasValue)
        {
            throw new Exception();
        }
        
        _lobbyId = lobby.Value.Id;
        lobby.Value.SetPrivate();
        lobby.Value.SetJoinable(true);
    } 
    
    private void OnLobbyCreatedCallback(Result result, Steamworks.Data.Lobby lobby)
    {
        LobbyManager.OnLobbyCreation(lobby.Id.ToString());
    }
}
