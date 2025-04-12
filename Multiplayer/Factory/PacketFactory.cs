using System;
using System.Buffers;
using System.Runtime.InteropServices;
using Godot;

public static class PacketFactory
{
    public const byte HeaderSizeReliable = 4; // MainType, SubType, PlayerIndex, SendType
    public const byte HeaderSizeUnreliable = 6; // MainType, SubType, PlayerIndex, SendType, Tick

    public static IntPtr CreateReliablePacket(byte mainType, byte subType, byte playerIndex, byte[] data, out int totalSize)
    {
        totalSize = HeaderSizeReliable + (data?.Length ?? 0);
        IntPtr ptr = Marshal.AllocHGlobal(totalSize);

        unsafe
        {
            byte* buffer = (byte*)ptr;

            buffer[0] = mainType;
            buffer[1] = subType;
            buffer[2] = playerIndex;
            buffer[3] = 0;

            if (data?.Length > 0)
            {
                Marshal.Copy(data, 0, IntPtr.Add(ptr, HeaderSizeReliable), data.Length);
            }
        }

        return ptr;
    }

    public static IntPtr CreateUnreliablePacket(byte mainType, byte subType, byte playerIndex, ushort tick, byte[] data, out int totalSize)
    {
        totalSize = HeaderSizeUnreliable + (data?.Length ?? 0);
        IntPtr ptr = Marshal.AllocHGlobal(totalSize);
        
        unsafe
        {
            byte* buffer = (byte*)ptr;

            buffer[0] = mainType;
            buffer[1] = subType;
            buffer[2] = playerIndex;
            buffer[3] = 1;
            
            *(ushort*)(buffer + HeaderSizeReliable) = tick;

            if (data?.Length > 0)
            {
                Marshal.Copy(data, 0, IntPtr.Add(ptr, HeaderSizeReliable), data.Length);
            }
        }

        return ptr;
    }
}