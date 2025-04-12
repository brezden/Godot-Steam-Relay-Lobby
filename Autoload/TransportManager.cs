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
    
    public class Server
    {
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
        
        public static void SendReliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data = null) {
            Instance._transportService.CreateAndSendReliablePacketToClients((byte) mainType, subType, playerIndex, data);
            Logger.Network($"Sent reliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }
        
        public static void SendUnreliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data = null) {
            Instance._transportService.CreateAndSendUnreliablePacketToClients((byte) mainType, subType, playerIndex, tick, data);
            Logger.Network($"Sent unreliable packet to clients. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
        
        public void OnPacketReceived(byte[] packet)
        {
            Logger.Network($"Received server packet. Length: {packet.Length}");
        }
    }

    public class Client
    {
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
        
        public static void SendReliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex, byte[] data = null) 
        {
            Instance._transportService.CreateAndSendReliablePacketToServer((byte) mainType, subType, playerIndex, data);
            Logger.Network($"Sent reliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }
        
        public static void SendUnreliablePacket(PacketTypes.MainType mainType, byte subType, byte playerIndex, ushort tick, byte[] data = null) 
        {
            Instance._transportService.CreateAndSendUnreliablePacketToServer((byte)mainType, subType, playerIndex, tick, data);
            Logger.Network($"Sent unreliable packet to server. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
        
        public void OnReliablePacketReceived(byte mainType, byte subType, byte playerIndex, byte[] data = null)
        {
            Logger.Network($"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}");
        }

        public void OnUnreliablePacketReceived(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data = null)
        {
            Logger.Network($"Received packet. MainType: {mainType}, SubType: {subType}, PlayerIndex: {playerIndex}, Tick: {tick}");
        }
    }
}