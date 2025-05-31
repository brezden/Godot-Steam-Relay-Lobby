using System;
using GodotPeer2PeerSteamCSharp.Routers;

namespace GodotPeer2PeerSteamCSharp.Core;

public partial class TransportManager
{
    private static class Dispatcher
    {
        public static void ReliablePacket(byte mainType, byte subType, byte playerIndex,
            Span<byte> data = default)
        {
            switch ((PacketTypes.MainType) mainType)
            {
                case PacketTypes.MainType.Scene:
                    SceneRouter.Route(subType, playerIndex, data);
                    break;
                default:
                    Logger.Error($"Unknown main type: {mainType}");
                    break;
            }
        }
    }
}
