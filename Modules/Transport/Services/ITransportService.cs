using System.Threading.Tasks;

namespace GodotPeer2PeerSteamCSharp.Modules.Transport.Services.Steam;

public interface ITransportService
{
    void CreateHost();
    void CreateClient(ulong hostId);
}