using System.Collections.Generic;
using System.Reflection.Emit;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Types.Lobby;

public enum ConnectionType
{
    Local,
    LAN,
    Online
}

public struct PlayerInfo
{
    public string PlayerId;
    public string Name;
    public ImageTexture? ProfilePicture;
    public bool IsReady;
}

public struct PlayerInvite
{
    public string PlayerId;
    public string PlayerName;
    public string PlayerStatus;
    public ImageTexture PlayerPicture;
}

public struct LobbyMembersData
{
    public Dictionary<string, PlayerInfo> Players;

    public LobbyMembersData()
    {
        Players = new Dictionary<string, PlayerInfo>();
    }
}

public struct LobbyMessageArgs
{
    public string PlayerName;
    public string Message;
}
