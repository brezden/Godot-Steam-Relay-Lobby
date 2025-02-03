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

    public enum LobbyType : byte
    {
        PlayerJoin = 1,
        PlayerLeft = 2,
        PlayerListUpdate = 3,
        SettingsChange = 4,
        ReadyStateChange = 5,
        StartGame = 6
    }

    public struct PacketHeader
    {
        public MainType MainType;
        public byte SubType;
        public byte SenderId;
        public double Timestamp;
    }
}