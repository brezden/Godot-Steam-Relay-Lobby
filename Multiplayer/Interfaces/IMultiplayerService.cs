public interface IMultiplayerService
{
    void Initialize();
    void Update();
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    void InviteLobbyOverlay();
}