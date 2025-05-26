using System;
using System.Buffers.Binary;

namespace GodotPeer2PeerSteamCSharp.Routers;

public class SceneRouter
{
    public static void Route(byte subType, byte playerIndex, Span<byte> data = default)
    {
        switch ((PacketTypes.Scene) subType)
        {
            case PacketTypes.Scene.Change:
                ChangeScene(data);
                break;
            default:
                Logger.Error($"Unknown scene packet type: {subType}");
                break;
        }
    }

    public static void ChangeScene(Span<byte> data)
    {
        if (data.Length < 1)
        {
            Logger.Error("Scene change packet data is too short");
            return;
        }

        var sceneId = BinaryPrimitives.ReadUInt16LittleEndian(data);
        SceneManager.Instance.GotoScene(sceneId);
    }
}
