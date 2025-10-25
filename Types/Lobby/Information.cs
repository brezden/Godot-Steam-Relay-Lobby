using System.Collections.Generic;
using System.Reflection.Emit;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Types.Lobby;

public enum LobbyType
{
Public,
FriendsOnly,
Private
}

public enum ConnectionType
{
    Local,
    LAN,
    Online
}

public struct PlayerInfo
{
    public ulong PlayerId;
    public string Name;
    public ImageTexture? ProfilePicture;
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
    public Dictionary<ulong, PlayerInfo> Players;

    public LobbyMembersData()
    {
        Players = new Dictionary<ulong, PlayerInfo>();
    }
}

public struct LobbyMessageArgs
{
    public string PlayerName;
    public string Message;
}
