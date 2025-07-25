namespace GodotPeer2PeerSteamCSharp.Modules;

public partial class TransportManager
{
    public bool IsConnectionActive()
    {
        return _transportService.IsConnectionActive();
    }
    
    public void Disconnect()
    {
        _transportService.Disconnect();
        ExecuteProcessMethodStatus(false);
        Logger.Network("Disconnected from server");
    }
}
