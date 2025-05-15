namespace GodotPeer2PeerSteamCSharp.Core.Lobby.Gates;

public class LobbyConnectionGate
{
    private bool _lobbyEntered;
    private bool _lobbyInformationGathered;
    private bool _transportReady;

    private bool IsReady => _lobbyEntered && _transportReady && _lobbyInformationGathered;

    public void MarkLobbyEntered()
    {
        _lobbyEntered = true;
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
            LobbyManager.PlayerReadyToJoinGame();
            Reset();
        }
    }

    public void Reset()
    {
        _lobbyEntered = false;
        _lobbyInformationGathered = false;
        _transportReady = false;
    }
}
