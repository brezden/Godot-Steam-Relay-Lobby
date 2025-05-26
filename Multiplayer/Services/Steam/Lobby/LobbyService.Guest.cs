using System;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Core.Lobby;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
    private static void RegisterGuestCallbacks()
    {
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }

    public static async Task JoinLobby(string lobbyId)
    {
        LobbyManager.AttemptingToJoinLobby(lobbyId);

        var result = await SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
        if (!result.HasValue)
        {
            LobbyManager.ErrorJoiningLobby();
            throw new Exception("Failed to join lobby");
        }
    }

    private static void OnGameLobbyJoinRequested(Steamworks.Data.Lobby lobby, SteamId id)
    {
        JoinLobby(lobby.Id.ToString());
    }
}
