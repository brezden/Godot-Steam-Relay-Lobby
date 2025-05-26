using System;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService : ILobbyService
{
    private Steamworks.Data.Lobby _lobby;
    private SteamId _lobbyId;

    public void Initialize()
    {
        InitializeSteam();
        RegisterChatCallbacks();
        RegisterGuestCallbacks();
        RegisterParticipantCallbacks();
    }

    public void Update()
    {
        SteamClient.RunCallbacks();
    }

    private static void InitializeSteam()
    {
        try
        {
            SteamClient.Init(3485870);
        }
        catch (Exception ex)
        {
            Logger.Error($"Steam initialization error: {ex.Message}");
        }

        Logger.Network($"Steam initialized successfully! User: {SteamClient.Name}");
    }
}
