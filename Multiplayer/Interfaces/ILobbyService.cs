public interface ILobbyService
{
    void Initialize();
    void Update();
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    void InviteLobbyOverlay();
    void SendLobbyMessage(string message);
}