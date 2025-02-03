using System;
using System.Collections.Generic;
using Godot;
using Steamworks;

public partial class TransportManager : Node
{
    public static TransportManager Instance { get; private set; } 
    private ITransportService _transportService;
    
    public override void _Ready()
    {
        Instance = this;

        _transportService = new SteamTransportService();
    }
    
    public static void CreateServer()
    {
        Instance._transportService.CreateServer();
    }

    public static void ConnectToServer(string serverId)
    {
        GD.Print("[TransportManager] Attempting to connect to server: " + serverId);
        Instance._transportService.ConnectToServer(serverId);
    }
}