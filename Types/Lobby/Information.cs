using System.Collections.Generic;
using Godot;

namespace GodotPeer2PeerSteamCSharp.Types.Lobby;

public struct PlayerInfo
{
    public string PlayerId;
    public string Name;
    public ImageTexture ProfilePicture;
    public bool IsReady;
}

public struct LobbyMembersData
{
    public Dictionary<string, PlayerInfo> Players;

    public LobbyMembersData()
    {
        Players = new Dictionary<string, PlayerInfo>();
    }
}


