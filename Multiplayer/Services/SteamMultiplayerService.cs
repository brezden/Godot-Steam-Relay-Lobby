using Godot;
using Steamworks;
using System;

public class SteamMultiplayerService : IMultiplayerService
{
    public void Initialize()
    {
        try
        {
            GD.Print("Attempting to initialize Steam...");
            SteamClient.Init(3485870, true);

            if (SteamClient.IsValid)
            {
                GD.Print($"Steam initialized successfully! User: {SteamClient.Name}");
            }
            else
            {
                GD.PrintErr("Steam initialization failed.");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Steam initialization error: {ex.Message}");
        }
    }

    public void CreateLobby(int maxPlayers)
    {
        SteamMatchmaking.CreateLobbyAsync(maxPlayers).ContinueWith(task =>
        {
            if (task.Result.HasValue)
            {
                GD.Print($"Steam lobby created: {task.Result.Value.Id}");
            }
        });
    }

    public void JoinLobby(string lobbyId)
    {
        SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
    }
}