using System;
using Godot;

public static class PacketFactory
{
    private const byte HeaderSizeReliable = 3; // MainType, SubType, PlayerIndex
    private const byte HeaderSizeUnreliable = 5; // MainType, SubType, PlayerIndex, Tick

    public static byte[] CreateReliablePacket(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data)
    {
        byte[] packet = new byte[ + (data?.Length ?? 0)];
        
        packet[0] = mainType;
        packet[1] = subType;
        packet[2] = playerIndex;
        
        BitConverter.GetBytes(tick).CopyTo(packet, 3);
        
        if (data != null && data.Length > 0)
        {
            Buffer.BlockCopy(data, 0, packet, HeaderSizeUnreliable, data.Length);
        }

        return packet;
    }
}