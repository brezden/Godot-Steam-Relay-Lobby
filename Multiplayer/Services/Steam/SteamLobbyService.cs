using Godot;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodotPeer2PeerSteamCSharp.Types.Lobby;
using Steamworks.Data;

public class SteamLobbyService : ILobbyService
{
    private static SteamId _lobbyId;
    private static Lobby _lobby;

    #region Initialization

    public void Initialize()
    {
        InitializeSteam();
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreatedCallback;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEnteredCallback;
        SteamMatchmaking.OnChatMessage += OnLobbyChatMessage;
        SteamMatchmaking.OnLobbyMemberJoined += LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberDisconnected += LobbyMemberLeft;
        SteamMatchmaking.OnLobbyMemberLeave += LobbyMemberLeft;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }

    public void Update()
    {
        SteamClient.RunCallbacks();
    }

    private static bool InitializeSteam()
    {
        try
        {
            SteamClient.Init(3485870, true);

            if (SteamClient.IsValid)
            {
                Logger.Network($"Steam initialized successfully! User: {SteamClient.Name}");
                return true;
            }

            Logger.Error("Steam initialization failed.");
            return false;
        }
        catch (Exception ex)
        {
            Logger.Error($"Steam initialization error: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Lobby Management

    public async Task CreateLobby(int maxPlayers)
    {
        var lobbyResult = await SteamMatchmaking.CreateLobbyAsync(maxPlayers);
        if (lobbyResult.HasValue)
        {
            _lobbyId = lobbyResult.Value.Id;
            lobbyResult.Value.SetPublic();
            lobbyResult.Value.SetJoinable(true);
        }
        else
        {
            throw new Exception("Failed to create lobby.");
        }
    }

    private static void OnLobbyCreatedCallback(Result result, Lobby lobby)
    {
        LobbyManager.OnLobbyCreation(lobby.Id.ToString());
    }

    public void LeaveLobby()
    {
        _lobby.Leave();
        Logger.Network($"Left lobby: {_lobbyId}");
        _lobbyId = 0;
    }

    public bool IsLobbyActive()
    {
        return _lobbyId != 0 && _lobby.MemberCount > 0;
    }

    #endregion

    #region Lobby Chat

    public void SendLobbyMessage(string message)
    {
        _lobby.SendChatString(message);
    }

    private static void OnLobbyChatMessage(Lobby lobby, Friend friend, string message)
    {
        LobbyManager.OnLobbyMessageReceived(friend.Name, message);
    }

    #endregion

    #region Lobby Lifecycle

    private static void OnLobbyEnteredCallback(Lobby lobby)
    {
        Logger.Network($"Joined lobby: {lobby.Id}");
        _lobby = lobby;
        LobbyManager.OnLobbyJoin(lobby.Owner.Id.ToString());
        LobbyManager.GatherLobbyMembers();
    }

    private static void LobbyMemberJoined(Lobby lobby, Friend friend)
    {
        ImageTexture profilePicture = GetProfilePictureAsync(friend.Id).Result;
        LobbyManager.AddPlayer(profilePicture, friend.Name, friend.Id);
    }

    private static void LobbyMemberLeft(Lobby lobby, Friend friend)
    {
        LobbyManager.RemovePlayer(friend.Id.ToString());
    }

    #endregion

    #region Lobby Invitation and Joining

    public void InvitePlayer(string playerId)
    {
        try
        {
            ulong.TryParse(playerId, out ulong steamIdValue);
            SteamId steamId = new SteamId();
            steamId.Value = steamIdValue;
            _lobby.InviteFriend(steamId);
        }
        catch (Exception ex)
        {
            Logger.Error($"Error inviting player: {ex.Message}");
        }
    }

    public async void JoinLobby(string lobbyId)
    {
        try
        {
            var result = await SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
            if (!result.HasValue)
            {
                throw new Exception("Failed to join lobby.");
            }
        }
        catch (Exception ex)
        {
            LobbyManager.ErrorJoiningLobby();
        }
    }

    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        Logger.Network($"Accepted lobby invite through Steam UI. Joining lobby: {lobby.Id}");
        JoinLobby(lobby.Id.ToString());
    }

    #endregion

    #region Utiliy Methods

    public LobbyMembersData GatherLobbyMembersData()
    {
        LobbyMembersData lobbyMembersData = new LobbyMembersData();
        IEnumerable<Friend> members = _lobby.Members;

        foreach (Friend member in members)
        {
            var profilePicture = GetProfilePictureAsync(member.Id).Result;
            lobbyMembersData.Players.Add(member.Id.ToString(), new PlayerInfo
            {
                PlayerId = member.Id.ToString(),
                Name = member.Name,
                ProfilePicture = profilePicture
            });
        }

        return lobbyMembersData;
    }

    public async Task<List<GlobalTypes.PlayerInvite>> GetInGameFriends()
    {
        List<GlobalTypes.PlayerInvite> inGameFriends = new List<GlobalTypes.PlayerInvite>();
        var friends = SteamFriends.GetFriends();

        foreach (var friend in friends)
        {
            if (friend.IsPlayingThisGame)
            {
                var profilePicture = await GetProfilePictureAsync(friend.Id);
                inGameFriends.Add(new GlobalTypes.PlayerInvite
                {
                    PlayerId = friend.Id.ToString(),
                    PlayerName = friend.Name,
                    PlayerStatus = friend.State.ToString(),
                    PlayerPicture = profilePicture
                });
            }
        }

        return inGameFriends;
    }

    private static async Task<ImageTexture?> GetProfilePictureAsync(SteamId steamId)
    {
        var steamImage = await SteamFriends.GetMediumAvatarAsync(steamId);
        if (steamImage == null) return null;

        Godot.Image newImage = Godot.Image.CreateFromData(
            (int)steamImage.Value.Width,
            (int)steamImage.Value.Height,
            false,
            Godot.Image.Format.Rgba8,
            steamImage.Value.Data
        );

        var texture = new ImageTexture();
        texture.SetImage(newImage);

        return texture;
    }

    #endregion
}
