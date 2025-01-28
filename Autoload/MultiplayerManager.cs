using Godot;

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

    public static void CreateLobby()
    {
        Instance._multiplayerService.CreateLobby(4);
    }

    public static void JoinLobby(string lobbyId)
    {
        Instance._multiplayerService.JoinLobby(lobbyId);
    }
}