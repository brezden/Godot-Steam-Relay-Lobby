using System;
using Godot;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService : ILobbyService
{
    private static string appId = "3485870";
    private ulong _lobbyId;

    public void Initialize()
    {
        InitializeSteam();
        RegisterChatCallbacks();
        RegisterGuestCallbacks();
        RegisterParticipantCallbacks();
    }

    public void Update()
    {
        Steam.RunCallbacks();
    }

    private static void InitializeSteam()
    {
        try
        {
            OS.SetEnvironment("SteamAppId", appId);
            OS.SetEnvironment("SteamGameId", appId); 
            Steam.SteamInit();
        }
        catch (Exception ex)
        {
            Logger.Error($"Steam initialization error: {ex.Message}");
        }

        Logger.Network($"Steam initialized successfully! User: {Steam.GetFriendPersonaName(Steam.GetSteamID())}");
    }
}
