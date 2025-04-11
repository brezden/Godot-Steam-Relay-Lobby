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
            Logger.Error("Failed to create server.");
            return false;
        }
        
        Logger.Network("Server created successfully.");
        return true;
    }

    public static bool ConnectToServer(string serverId)
    {
        Logger.Network($"Attempting to connect to server: {serverId}");
        return Instance._transportService.ConnectToServer(serverId);
    }
    
    public static void Disconnect()
    {
        Instance._transportService.Disconnect();
        Logger.Network("Disconnected from server.");
    }
    
    public static void SendReliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data = null) 
    {
        Instance._transportService.SendReliablePacketToServer((byte) mainType, subType, playerIndex, data);
        Logger.Network($"Sent reliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
    }
    
    public static void SendUnreliablePacketToServer(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data = null) 
    {
        Instance._transportService.SendUnreliablePacketToServer((byte)mainType, subType, playerIndex, tick, data);
        Logger.Network($"Sent unreliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
    }

    public static void SendReliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data = null) {
        Instance._transportService.SendReliablePacketToClients((byte) mainType, subType, playerIndex, data);
        Logger.Network($"Sent reliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
    }
    
    public static void SendUnreliablePacketToClients(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data = null) {
        Instance._transportService.SendUnreliablePacketToClients((byte) mainType, subType, playerIndex, tick, data);
        Logger.Network($"Sent unreliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
    }
}