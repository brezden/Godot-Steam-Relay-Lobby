using System;
using Godot;
using System.Buffers.Binary;
using GodotPeer2PeerSteamCSharp.Autoload;

public partial class InputReceiver : Node
{
    private const string L = "move_left_0";
    private const string R = "move_right_0";
    private const string U = "move_up_0";
    private const string D = "move_down_0";

    private ushort _tick;
    
    public InputReceiver()
    {
        SetProcess(false);
        _tick = 0;
    }

    public override void _Process(double delta)
    {
        _tick++;
    }
    
    public void TurnOn()
    {
        _tick = 0;
        SetProcess(true);
        Logger.Game("InputReceiver Turned On");
    }
    
    public void TurnOff()
    {
        SetProcess(false);
        Logger.Game("InputReceiver Turned Off");
    }

    public byte[] BuildClientPacket(double delta)
    {
        Vector2 mv = Input.GetVector(L, R, U, D);
        InputManager.CurrentInputHandler.ProcessPositionalInput(0, mv, delta);
        sbyte qx = QuantizeToI8(mv.X);
        sbyte qy = QuantizeToI8(mv.Y);

        var buf = new byte[2];
        buf[0] = unchecked((byte)qx);
        buf[1] = unchecked((byte)qy);
        return buf;
    }

    public byte[] BuildServerPacket(double delta)
    {
        Vector2 mv = Input.GetVector(L, R, U, D);
        InputManager.CurrentInputHandler.ProcessPositionalInput(0, mv, delta);
        sbyte qx = QuantizeToI8(mv.X);
        sbyte qy = QuantizeToI8(mv.Y);

        var buf = new byte[4];
        BinaryPrimitives.WriteUInt16LittleEndian(buf.AsSpan(0, 2), _tick);
        buf[2] = unchecked((byte)qx);
        buf[3] = unchecked((byte)qy);
        return buf;
        
    }

    private static sbyte QuantizeToI8(float x)
    {
        // clamp [-1,1] -> [-127,127]
        if (x < -1f) x = -1f; else if (x > 1f) x = 1f;
        return (sbyte)Mathf.RoundToInt(x * 127f);
    }
}
