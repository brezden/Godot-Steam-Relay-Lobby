using Godot;
using System.Collections.Generic;
using System;
using Steamworks;

public static class GlobalTypes
{
    public struct PlayerInfo
    {
        public string PlayerId;
        public string Name;
        public ImageTexture ProfilePicture;
        public bool IsReady;
    }

    public struct PlayerInvite
    {
        public string PlayerId;
        public string PlayerName;
        public string PlayerStatus;
        public ImageTexture PlayerPicture;
    }

    public struct LobbyInfo
    {
        public string LobbyId;
        public string HostId;
        public int MaxPlayers;
        public Dictionary<string, PlayerInfo> Players;
    }

    public enum GameType
    {
        Duel,
        TeamDuel,
        OneVersusMany,
        FreeForAll
    }

    public class LobbyMessageArgs : EventArgs
    {
        public string PlayerName { get; set; }
        public string Message { get; set; }
    }
}
