using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService : ILobbyService
{
    private static ulong _lobbyId;

    public void Initialize()
    {
        RegisterChatCallbacks();
        RegisterGuestCallbacks();
        RegisterParticipantCallbacks();
        RegisterUtilityCallbacks();
    }

    public void Update()
    {
        Steam.RunCallbacks();
    }
}
