using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;
using GodotPeer2PeerSteamCSharp.Types.Scene;

public partial class LobbyButtons : Node
{
    Button startGameButton;
    Button inviteLobbyButton;
    Button leaveLobbyButton;
    
    public override void _Ready()
    {
        startGameButton = GetNode<Button>("%StartGame");
        inviteLobbyButton = GetNode<Button>("%InviteMembers");
        leaveLobbyButton = GetNode<Button>("%LeaveLobby");

        inviteLobbyButton.Pressed += InviteLobby;
        leaveLobbyButton.Pressed += LeaveLobby;
        startGameButton.Pressed += StartGame;
    }

    private void InviteLobby()
    {
        LobbyManager.OpenInviteOverlay();
    }

    private void LeaveLobby()
    {
        LobbyManager.LeaveLobbyAndTransport();
        SceneManager.Instance.GotoScene(SceneRegistry.MainMenu.Home);
    }

    private void StartGame()
    {
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
        
        const string msg = "I'm connected";

        if (Multiplayer.IsServer())
        {
            // Host can directly broadcast to all peers (and self via CallLocal)
            Rpc(nameof(BroadcastLog), msg);
        }
        else
        {
            // Client tells server to relay
            RpcId(1, nameof(ServerReceiveAndRelay), msg);
        }
    }

    // Clients call this on the server; server then relays to everyone
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ServerReceiveAndRelay(string msg)
    {
        if (!Multiplayer.IsServer()) return;
        Rpc(nameof(BroadcastLog), msg);
    }

    // Runs on everyone; CallLocal=true ensures the caller also logs immediately
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    private void BroadcastLog(string msg)
    {
        var sender = Multiplayer.GetRemoteSenderId();
        if (sender == 0) sender = Multiplayer.GetUniqueId(); // local call case
        Logger.Lobby($"RPC ping from {sender}: {msg}");
    }
}
