using System;
using Godot;

public static class PacketFactory
{
    private const int HEADER_SIZE = 11; // 1 + 1 + 1 + 8 bytes

    public static byte[] CreatePacket(PacketTypes.MainType mainType, byte subType, byte[] data)
    {
        byte[] packet = new byte[HEADER_SIZE + (data?.Length ?? 0)];
        
        packet[0] = (byte)mainType;
        packet[1] = subType;
        packet[2] = (byte)91;
        
        BitConverter.GetBytes(Time.GetTicksMsec()).CopyTo(packet, 3);
        
        if (data != null && data.Length > 0)
        {
            Buffer.BlockCopy(data, 0, packet, HEADER_SIZE, data.Length);
        }

        return packet;
    }

    public static (PacketTypes.PacketHeader header, byte[] data) ParsePacket(byte[] packet)
    {
        if (packet == null || packet.Length < HEADER_SIZE)
        {
            throw new ArgumentException("Packet is null or too small to contain a header");
        }

        var header = new PacketTypes.PacketHeader
        {
            MainType = (PacketTypes.MainType)packet[0],
            SubType = packet[1],
            SenderId = packet[2],
            Timestamp = BitConverter.ToDouble(packet, 3)
        };

        byte[] data = null;
        if (packet.Length > HEADER_SIZE)
        {
            data = new byte[packet.Length - HEADER_SIZE];
            Buffer.BlockCopy(packet, HEADER_SIZE, data, 0, data.Length);
        }

        return (header, data);
    }

    public static byte[] CreatePacketWithString(PacketTypes.MainType mainType, byte subType, string message)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
        return CreatePacket(mainType, subType, data);
    }

    public static string GetStringFromPacketData(byte[] data)
    {
        if (data == null || data.Length == 0) return string.Empty;
        return System.Text.Encoding.UTF8.GetString(data);
    }
}