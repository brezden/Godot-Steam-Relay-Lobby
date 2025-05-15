using System;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService 
{
    private void RegisterGuestCallbacks()
    {
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }
    
    public async void JoinLobby(string lobbyId)
    {
        LobbyManager.AttemptingToJoinLobby(lobbyId);
        
        try
        {
            var result = await SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
            if (!result.HasValue)
            {
                throw new Exception("Failed to join lobby.");
            }
        }
        catch (Exception ex)
        {
            LobbyManager.ErrorJoiningLobby();
        }
    }

    private void OnGameLobbyJoinRequested(Steamworks.Data.Lobby lobby, SteamId id)
    {
        JoinLobby(lobby.Id.ToString());
    }
}
