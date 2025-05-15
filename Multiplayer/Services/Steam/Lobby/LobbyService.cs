namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

using Steamworks;
using System;
using Steamworks.Data;

public partial class LobbyService : ILobbyService
{
    private SteamId _lobbyId;
    private Lobby _lobby;

    public void Initialize()
    {
        InitializeSteam();
        RegisterChatCallbacks();
        RegisterGuestCallbacks();
        RegisterHostCallbacks();
        RegisterParticipantCallbacks();
    }

    private void InitializeSteam()
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
    
    public void Update()
    {
        SteamClient.RunCallbacks();
    }
}
