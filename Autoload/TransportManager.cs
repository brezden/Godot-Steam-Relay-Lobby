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
        SetProcess(false);
    }
    
    public override void _Process(double delta)
    {
        _transportService.Update();
    }
    
    public void ExecuteProcessMethodStatus(bool status)
    {
        SetProcess(status);
    }
    
    public static bool CreateServer()
    {
        bool result = Instance._transportService.CreateServer();
        
        if (!result)
        {
            GD.PrintErr("[TransportManager] Failed to create server.");
            return false;
        }
        
        GD.Print("[TransportManager] Server created successfully.");
        return true;
    }

    public static bool ConnectToServer(string serverId)
    {
        GD.Print("[TransportManager] Attempting to connect to server: " + serverId);
        return Instance._transportService.ConnectToServer(serverId);
    }

    public static void SendPacketToServer(PacketTypes.MainType mainType, byte subType, byte[] data)
    {
        GD.Print("[TransportManager] Sending packet to server with main type: " + mainType + " and sub type: " + subType);
        Instance._transportService.SendPacketToServer(mainType, subType, data);
    }
    
    public static void SendPacketToClients(PacketTypes.MainType mainType, byte subType, byte[] data)
    {
        GD.Print("[TransportManager] Sending packet to clients with main type: " + mainType + " and sub type: " + subType);
        Instance._transportService.SendPacketToClients(mainType, subType, data);
    }
}