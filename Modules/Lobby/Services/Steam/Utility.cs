using System.Linq;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using GodotSteam;

namespace GodotPeer2PeerSteamCSharp.Modules.Lobby.Services;

public partial class LobbyService
{
    private Steam.AvatarLoadedEventHandler _onAvatarLoadedHandler;
    
    private void RegisterUtilityCallbacks()
    {
        _onAvatarLoadedHandler += OnAvatarLoaded;
        Steam.AvatarLoaded += _onAvatarLoadedHandler;
    }
    
    public LobbyMembersData GatherLobbyMembersData()
    {
        var lobbyMembersData = new LobbyMembersData();

        int memberCount = Steam.GetNumLobbyMembers(_lobbyId);

        // For each index in the lobby members list
        // get the member index and add to members list
        foreach (int memberIndex in Enumerable.Range(0, memberCount))
        {
            ulong memberId = Steam.GetLobbyMemberByIndex(_lobbyId, memberIndex);
            var memberName = Steam.GetFriendPersonaName(memberId);
            Steam.GetPlayerAvatar(AvatarSize.Large, memberId);
            lobbyMembersData.Players.Add(memberId, new PlayerInfo
            {
                PlayerId = memberId,
                Name = memberName,
            });
        }
        
        return lobbyMembersData;
    }
    
    public PlayerInfo GetLobbyMember(ulong steamId)
    {
        var memberName = Steam.GetFriendPersonaName(steamId);
        Steam.GetPlayerAvatar(AvatarSize.Large, steamId);
        
        return new PlayerInfo
        {
            PlayerId = steamId,
            Name = memberName,
        };
    }
    
    private static void OnAvatarLoaded(ulong steamId, int width, byte[] data)
    {
        var newImage = Image.CreateFromData(
            (int) width,
            (int) width, // Height is same as width for avatars
            false,
            Image.Format.Rgba8,
            data
        );

        var texture = new ImageTexture();
        texture.SetImage(newImage);
        
        LobbyManager.LobbyMembersData.Players[steamId] = new PlayerInfo
        {
            PlayerId = steamId,
            Name = LobbyManager.LobbyMembersData.Players[steamId].Name,
            ProfilePicture = texture
        };
        
        EventBus.Lobby.OnLobbyMembersRefreshed();
    }

    public void OpenInviteOverlay()
    {
        Steam.ActivateGameOverlayInviteDialog(_lobbyId);
    }
}
