using Godot;
using Steamworks;
using System;
using Steamworks.Data;

public class SteamMultiplayerService : IMultiplayerService
{
    private static SteamId _lobbyId;
    
    public void Initialize()
    {
        var connectionStatus = InitializeSteam();
        
        if (connectionStatus)
        {
            GD.Print("YERR");
            SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        }
    }
    
    public void Update()
    {
        SteamClient.RunCallbacks();
    }

    private static bool InitializeSteam()
    {
        try
        {
            SteamClient.Init(3485870, true);

            if (SteamClient.IsValid)
            {
                GD.Print($"Steam initialized successfully! User: {SteamClient.Name}");
                return true;
            }
            
            GD.PrintErr("Steam initialization failed.");
            return false;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Steam initialization error: {ex.Message}");
            return false;
        }
    }

    public async void CreateLobby(int maxPlayers)
    {
        try
        {
            var lobbyResult = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
            if (lobbyResult.HasValue)
            {
                _lobbyId = lobbyResult.Value.Id;
                lobbyResult.Value.SetPublic();
                lobbyResult.Value.SetJoinable(true);
                GD.Print($"Steam lobby created: {_lobbyId}");
            }
            else
            {
                GD.PrintErr("Failed to create Steam lobby.");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error creating lobby: {ex.Message}");
        }
    }


    private void OnLobbyMemberJoined(Lobby lobby, Friend member)
    {
        GD.Print("Lobby member joined");
        if (lobby.Id == _lobbyId)
        {
            var playerName = member.Name;
            GD.Print($"Player joined: {playerName}");
            MultiplayerManager.MemberJoinLobby(playerName);
        }
    }

    public void InviteLobbyOverlay()
    {
        GD.Print(_lobbyId.Value);
        Steamworks.SteamFriends.OpenGameInviteOverlay(_lobbyId);
    }

    public void JoinLobby(string lobbyId)
    {
        SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
    }
}
