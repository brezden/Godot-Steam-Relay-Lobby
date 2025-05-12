public static class PacketTypes
{
    public enum MainType : byte
    {
        Input = 1,
        Scene = 2
    }

    public enum Input : byte
    {
        Movement = 1,
        Action = 2,
    }

    public enum Scene : byte
    {
        Change = 1,
    }
}
