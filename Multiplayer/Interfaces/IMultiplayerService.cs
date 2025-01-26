public interface IMultiplayerService
{
    // Initialize the networking service
    void Initialize();
    
    // Lobby management
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    
    // // Messaging and interaction
    // void SendMessage(long peerId, string message);
    // event Action<string> OnMessageReceived;
    //
    // // Player-related events
    // event Action<string> OnLobbyCreated;
    // event Action<string> OnLobbyJoined;
    // event Action<long> OnPlayerJoined;
    // event Action<long> OnPlayerLeft;
    //
    // // Get current lobby state
    // List<long> GetLobbyPlayers();
    // bool IsHost();
}