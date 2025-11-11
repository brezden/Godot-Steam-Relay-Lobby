using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Transport.Services.Steam;

public partial class TransportService : Node, ITransportService
{
    private bool IsOffline() => Multiplayer.MultiplayerPeer is OfflineMultiplayerPeer;

    public void CreateHost()
    {
        if (!IsOffline())
        {
            Logger.Lobby("Already connected to a transport");
            return;
        }

        var bridge = GetNode("/root/SteamPeerBridge");
        bridge.Call("create_host", 0);

        var active = Multiplayer.MultiplayerPeer;
        var activeType = active?.GetClass();
        var isOffline = active is OfflineMultiplayerPeer;
        var status = active?.GetConnectionStatus();

        Logger.Lobby($"HasPeer={Multiplayer.HasMultiplayerPeer()}");
        Logger.Lobby($"ActivePeerType={activeType}");
        Logger.Lobby($"IsOfflinePeer={isOffline}");
        Logger.Lobby($"Status={status}");
        Logger.Lobby($"IsServer={Multiplayer.IsServer()}");
        Logger.Lobby($"UniqueId={Multiplayer.GetUniqueId()}");
    }

    public void CreateClient(ulong hostId)
    {
        if (!IsOffline())
        {
            Logger.Lobby("Already connected to a transport");
            return;
        }
        
        var bridge = GetNode("/root/SteamPeerBridge");
        bridge.Call("create_client", hostId, 0);
        
        var active = Multiplayer.MultiplayerPeer;
        var activeType = active?.GetClass();
        var isOffline = active is OfflineMultiplayerPeer;
        var status = active?.GetConnectionStatus();

        Logger.Lobby($"HasPeer={Multiplayer.HasMultiplayerPeer()}");
        Logger.Lobby($"ActivePeerType={activeType}");
        Logger.Lobby($"IsOfflinePeer={isOffline}");
        Logger.Lobby($"Status={status}");
        Logger.Lobby($"IsServer={Multiplayer.IsServer()}");
        Logger.Lobby($"UniqueId={Multiplayer.GetUniqueId()}");
    }

    public async Task Disconnect()
    {
        Multiplayer.SetMultiplayerPeer(null);
        Logger.Lobby("Transport disconnected (offline peer restored)");
    }
}