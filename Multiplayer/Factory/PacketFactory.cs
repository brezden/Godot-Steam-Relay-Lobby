using System;
using System.Buffers;
using Godot;

public static class PacketFactory
{
    private const byte HeaderSizeReliable = 3; // MainType, SubType, PlayerIndex
    private const byte HeaderSizeUnreliable = 5; // MainType, SubType, PlayerIndex, Tick

    public static byte[] CreateReliablePacket(byte mainType, byte subType, byte playerIndex, byte[] data)
    {
        byte[] packet = ArrayPool<byte>.Shared.Rent(HeaderSizeReliable + (data?.Length ?? 0));
        
        packet[0] = mainType;
        packet[1] = subType;
        packet[2] = playerIndex;
        
        if (data?.Length > 0)
        {
            Buffer.BlockCopy(data, 0, packet, HeaderSizeReliable, data.Length);
        }

        return packet;
    }

    public static byte[] CreateUnreliablePacket(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        byte[] packet = ArrayPool<byte>.Shared.Rent(HeaderSizeUnreliable + (data?.Length ?? 0));
        
        packet[0] = mainType;
        packet[1] = subType;
        packet[2] = playerIndex;
        
        BitConverter.GetBytes(tick).CopyTo(packet, HeaderSizeReliable);
        
        if (data?.Length > 0)
        {
            Buffer.BlockCopy(data, 0, packet, HeaderSizeUnreliable, data.Length);
        }

        return packet;
    }
    
    public static void ReturnPacket(byte[] packet)
    {
        ArrayPool<byte>.Shared.Return(packet);
    }
}