using Godot;

namespace GodotPeer2PeerSteamCSharp.Modules;

public partial class TransportManager : Node
{
    private ITransportService _transportService;

    public static TransportManager Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        Instance = this;

        _transportService = new TransportService();
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        _transportService.Update();
    }

    public void ExecuteProcessMethodStatus(bool status)
    {
        SetProcess(status);
    }
}
