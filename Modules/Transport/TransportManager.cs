using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Transport.Services.Steam;

namespace GodotPeer2PeerSteamCSharp.Modules.Transport;

public partial class TransportManager : Node
{
    public static TransportManager Instance { get; private set; } = null!;
    public MultiplayerPeer Peer => Multiplayer.MultiplayerPeer;

    private ITransportService _transportService = null!;

    public override void _Ready()
    {
        Instance = this;

        // Initialize Transport Service with tree access
        var svcNode = new TransportService();
        AddChild(svcNode);
        _transportService = svcNode;
        
        RegisterCallbacks();
    }
    
    private void RegisterCallbacks()
    {
        EventBus.Lobby.LobbyCreated += (_,_) => CreateHost();
    }
    
    public override void _Process(double delta)
    {
        Multiplayer.MultiplayerPeer?.Poll();
    }
    
    public void CreateHost() => _transportService.CreateHost();
    public void CreateClient(ulong hostId) => _transportService.CreateClient(hostId);
}