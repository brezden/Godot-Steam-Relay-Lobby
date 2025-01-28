public interface IMultiplayerService
{
    // Initialize the networking service
    void Initialize();
    
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
}