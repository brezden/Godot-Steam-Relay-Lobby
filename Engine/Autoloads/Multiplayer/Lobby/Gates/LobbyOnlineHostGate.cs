namespace GodotPeer2PeerSteamCSharp.Core.Lobby.Gates;

public class LobbyOnlineHostGate
{
    private bool _lobbyCreated;
    private bool _lobbyInformationGathered;
    private bool _transportReady;

    private bool IsReady => _lobbyCreated && _transportReady && _lobbyInformationGathered;

    public void MarkLobbyCreated()
    {
        _lobbyCreated = true;
        CheckLobbyReady();
    }

    public void MarkTransportReady()
    {
        _transportReady = true;
        CheckLobbyReady();
    }

    public void MarkLobbyInformationGathered()
    {
        _lobbyInformationGathered = true;
        CheckLobbyReady();
    }

    private void CheckLobbyReady()
    {
        if (IsReady)
        {
            
            Reset();
        }
    }

    public void Reset()
    {
        _lobbyCreated = false;
        _lobbyInformationGathered = false;
        _transportReady = false;
    }
}
