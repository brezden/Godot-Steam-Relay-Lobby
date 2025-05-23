using Godot;
using GodotPeer2PeerSteamCSharp.Core.Lobby.Gates;
using GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Core.Lobby;

public partial class LobbyManager : Node
{
    public static LobbyMembersData LobbyMembersData = new();
    public static LobbyConnectionGate LobbyConnectionGate = new();

    private static ILobbyService _lobbyService;
    private static bool _isHost;

    public static LobbyManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        Instance = this;

        _lobbyService = new LobbyService();
        _lobbyService.Initialize();

        RegisterHostCallbacks();
    }

    public override void _Process(double delta)
    {
        _lobbyService.Update();
    }
}
