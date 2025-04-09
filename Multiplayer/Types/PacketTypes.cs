public static class PacketTypes
{
    public enum MainType : byte
    {
        Lobby = 1,
        Player = 2,
        Game = 3,
        Minigame = 4,
        Chat = 5,
        System = 6
    }

    public enum Lobby : byte
    {
        PlayerJoin = 1,
        PlayerLeft = 2,
        PlayerListUpdate = 3,
        SettingsChange = 4,
        ReadyStateChange = 5,
        StartGame = 6
    }

    public enum Player : byte
    {
        Move = 1,
        Jump = 2,
        Shoot = 3,
        Respawn = 4
    }

    public enum Game : byte
    {
        Start = 1,
        Pause = 2,
        Resume = 3,
        End = 4
    }

    public enum Minigame : byte
    {
        Load = 1,
        Begin = 2,
        Win = 3,
        Lose = 4
    }

    public enum Chat : byte
    {
        Message = 1,
        PrivateMessage = 2,
        Emote = 3
    }

    public enum System : byte
    {
        Ping = 1,
        Pong = 2,
        Heartbeat = 3,
        ServerShutdown = 4
    }

    public struct PacketHeader
    {
        public MainType MainType;
        public byte SubType;
        public byte PlayerIndex;
    }
}