using Godot;
using Steamworks;
using System;
using Steamworks.Data;

public class SteamMultiplayerService : IMultiplayerService
{
    private static SteamId _lobbyId;

    public void Initialize()
    {
        InitializeSteam();
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallback;
        SteamMatchmaking.OnLobbyInvite += OnLobbyInviteReceivedCallback;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoinedCallback;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallback;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
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

    private void OnLobbyCreatedCallback(Result result, Lobby lobby)
    {
        GD.Print("[DEBUG] Lobby created: " + lobby.Id);
    }

    private void OnLobbyMemberJoinedCallback(Lobby lobby, Friend friend)
    {
        GD.Print("User has joined the Lobby: " + friend.Name);
    }

    private void OnLobbyInviteReceivedCallback(Friend friend, Lobby lobby)
    {
        GD.Print($"[DEBUG] Received lobby invite from: {friend.Name}. Lobby ID: {lobby.Id}");
    }

    private void OnLobbyEnteredCallback(Lobby lobby)
    {
        GD.Print("[DEBUG] Lobby entered: " + lobby.Id);
    }

    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        GD.Print($"[DEBUG] User accepted lobby invite through Steam UI. Joining lobby: {lobby.Id}");
        JoinLobby(lobby.Id.ToString());
    }

    public void InviteLobbyOverlay()
    {
        SteamFriends.OpenGameInviteOverlay(_lobbyId);
    }

    public async void JoinLobby(string lobbyId)
    {
        try
        {
            var result = await SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
            if (!result.HasValue)
            {
                GD.PrintErr($"Failed to join lobby: {lobbyId}");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error joining lobby: {ex.Message}");
        }
    }
}