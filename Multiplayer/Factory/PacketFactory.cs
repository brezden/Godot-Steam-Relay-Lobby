using System;
using System.Buffers;
using System.Runtime.InteropServices;
using Godot;

public static class PacketFactory
{
    public const byte HeaderSizeReliable = 4; // MainType, SubType, PlayerIndex, SendType
    public const byte HeaderSizeUnreliable = 6; // MainType, SubType, PlayerIndex, SendType, Tick

    public static IntPtr CreateReliablePacket(byte mainType, byte subType, byte playerIndex, Span<byte> data, out int totalSize)
    {
        totalSize = HeaderSizeReliable + data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(totalSize);

        unsafe
        {
            byte* buffer = (byte*)ptr;

            buffer[0] = mainType;
            buffer[1] = subType;
            buffer[2] = playerIndex;
            buffer[3] = 0;

            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    buffer[HeaderSizeReliable + i] = data[i];
                }
            }
        }

        return ptr;
    }

    public static IntPtr CreateUnreliablePacket(byte mainType, byte subType, byte playerIndex, ushort tick, Span<byte> data, out int totalSize)
    {
        totalSize = HeaderSizeUnreliable + data.Length;
        IntPtr ptr = Marshal.AllocHGlobal(totalSize);

        unsafe
        {
            byte* buffer = (byte*)ptr;

            buffer[0] = mainType;
            buffer[1] = subType;
            buffer[2] = playerIndex;
            buffer[3] = 1;

            *(ushort*)(buffer + HeaderSizeUnreliable) = tick;

            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    buffer[HeaderSizeUnreliable + i] = data[i];
                }
            }
        }

        return ptr;
    }
}
