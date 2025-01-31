using Godot;
using Steamworks;

public partial class MultiplayerManager : Node
{
    public static MultiplayerManager Instance { get; private set; }

    private IMultiplayerService _multiplayerService;

    public override void _Ready()
    {
        Instance = this;

        _multiplayerService = new SteamMultiplayerService();
        _multiplayerService.Initialize();
    }
    
    public override void _Process(double delta)
	{
        _multiplayerService.Update();
	}

    public static void CreateLobby()
    {
        Instance._multiplayerService.CreateLobby(4);
    }

    public static void InviteLobbyOverlay()
    {
        Instance._multiplayerService.InviteLobbyOverlay();
    }

    public static void MemberJoinLobby(string playerName)
    {
        GD.Print("Player joined lobby: " + playerName);
    }

    public static void JoinLobby(string lobbyId)
    {
        Instance._multiplayerService.JoinLobby(lobbyId);
    }
}