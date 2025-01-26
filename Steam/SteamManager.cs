using Godot;
using System;
using Steamworks;

public partial class SteamManager : Node
{
    public override void _Ready()
    {
        try
        {
            GD.Print("Attempting to initialize Steam...");
            SteamClient.Init(3485870, true);

            if (SteamClient.IsValid)
            {
                GD.Print("Steam initialized successfully!");
                GD.Print($"Steam Username: {SteamClient.Name}");
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

    public override void _Process(double delta)
    {
        SteamClient.RunCallbacks();
    }

    public override void _ExitTree()
    {
        SteamClient.Shutdown();
    }
}