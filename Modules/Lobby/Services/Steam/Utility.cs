using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using Steamworks;

namespace GodotPeer2PeerSteamCSharp.Services.Steam.Lobby;

public partial class LobbyService
{
    public async Task<LobbyMembersData> GatherLobbyMembersData()
    {
        var lobbyMembersData = new LobbyMembersData();
        var members = _lobby.Members;

        foreach (var member in members)
        {
            var playerInfo = GetPlayerInfo(member.Id.ToString()).Result;
            lobbyMembersData.Players.Add(member.Id.ToString(), playerInfo);
        }

        return lobbyMembersData;
    }

    public async Task<PlayerInfo> GetPlayerInfo(string playerId)
    {
        var friend = new Friend(ConvertStringToSteamId(playerId));
        var profilePicture = GetProfilePictureAsync(friend.Id).Result;
        return new PlayerInfo
        {
            PlayerId = playerId,
            Name = friend.Name,
            ProfilePicture = profilePicture,
            IsReady = false
        };
    }

    public async Task<List<PlayerInvite>> GetInGameFriends()
    {
        var inGameFriends = new List<PlayerInvite>();
        var friends = SteamFriends.GetFriends();

        foreach (var friend in friends)
            if (friend.IsPlayingThisGame)
            {
                var profilePicture = await GetProfilePictureAsync(friend.Id);
                inGameFriends.Add(new PlayerInvite
                {
                    PlayerId = friend.Id.ToString(),
                    PlayerName = friend.Name,
                    PlayerStatus = friend.State.ToString(),
                    PlayerPicture = profilePicture
                });
            }

        return inGameFriends;
    }

    private static async Task<ImageTexture?> GetProfilePictureAsync(SteamId steamId)
    {
        var steamImage = await SteamFriends.GetMediumAvatarAsync(steamId);
        if (steamImage == null)
            return null;

        var newImage = Image.CreateFromData(
            (int) steamImage.Value.Width,
            (int) steamImage.Value.Height,
            false,
            Image.Format.Rgba8,
            steamImage.Value.Data
        );

        var texture = new ImageTexture();
        texture.SetImage(newImage);

        return texture;
    }

    private static SteamId ConvertStringToSteamId(string playerId)
    {
        ulong.TryParse(playerId, out var steamIdValue);
        var steamId = new SteamId();
        steamId.Value = steamIdValue;
        return steamId;
    }
}
