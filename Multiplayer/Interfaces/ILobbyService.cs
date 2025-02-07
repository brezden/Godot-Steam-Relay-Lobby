using System.Threading.Tasks;

public interface ILobbyService
{
    void Initialize();
    void Update();
    void CreateLobby(int maxPlayers);
    void JoinLobby(string lobbyId);
    Task GatherPlayerInformation();
    void InviteLobbyOverlay();
    void SendLobbyMessage(string message);
}