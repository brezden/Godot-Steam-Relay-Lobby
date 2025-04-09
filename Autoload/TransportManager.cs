﻿using System;
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

    public static void SendPacketToServer(PacketTypes.MainType mainType, byte subType, byte[] data) {
        Instance._transportService.SendPacketToServer(mainType, subType, data);

        Logger.Network(
            $"Sent packet to server with main type: {Enum.GetName(typeof(PacketTypes.MainType), mainType)} " +
            $"and sub type: {Enum.GetName(Type.GetType($"PacketTypes+{mainType}"), subType) ?? subType.ToString()}"
        );
    }

    public static void SendPacketToClients(PacketTypes.MainType mainType, byte subType, byte[] data) {
        Instance._transportService.SendPacketToClients(mainType, subType, data);

        Logger.Network(
            $"Sent packet to clients with main type: {Enum.GetName(typeof(PacketTypes.MainType), mainType)} " +
            $"and sub type: {Enum.GetName(Type.GetType($"PacketTypes+{mainType}"), subType) ?? subType.ToString()}"
        );
    }
}