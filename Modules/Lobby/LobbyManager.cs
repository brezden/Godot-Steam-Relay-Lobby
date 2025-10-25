using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;
using GodotPeer2PeerSteamCSharp.Types.Lobby;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby;

public partial class LobbyManager : Node
{
    public static LobbyMembersData LobbyMembersData = new();

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

        RegisterHostCallbacks();
        RegisterGuestCallbacks();

        _lobbyService = new LobbyService();
        _lobbyService.Initialize();
    }

    public override void _Process(double delta)
    {
        _lobbyService.Update();
    }
}
