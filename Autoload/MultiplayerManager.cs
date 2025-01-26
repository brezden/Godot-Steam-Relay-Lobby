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

    public void CreateLobby(int maxPlayers)
    {
        _multiplayerService.CreateLobby(maxPlayers);
    }

    public void JoinLobby(string lobbyId)
    {
        _multiplayerService.JoinLobby(lobbyId);
    }
}