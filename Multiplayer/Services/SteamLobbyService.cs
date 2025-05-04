using Godot;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks.Data;

public class SteamLobbyService : ILobbyService
{
    private static SteamId _lobbyId;
    private static Lobby _lobby;

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

    public async void CreateLobby(int maxPlayers)
    {
        try
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
                Logger.Error("Failed to create Steam lobby.");
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Error creating lobby: {ex.Message}");
        }
    }

    private static void OnLobbyCreatedCallback(Result result, Lobby lobby)
    {
        LobbyManager.OnLobbyCreation(lobby.Id.ToString());
    }

    private static void OnLobbyEnteredCallback(Lobby lobby)
    {
        _lobby = lobby;
        ImageTexture profilePicture = GetProfilePictureAsync(SteamClient.SteamId).Result;
        LobbyManager.AddPlayer(profilePicture, SteamClient.Name, SteamClient.SteamId);
        LobbyManager.OnLobbyJoin(lobby.Owner.Id.ToString());
    }

    public void SendLobbyMessage(string message)
    {
        _lobby.SendChatString(message);
    }
    
    private static void OnLobbyChatMessage(Lobby lobby, Friend friend, string message)
    {
        LobbyManager.OnLobbyMessageReceived(friend.Name, message);
    }
    
    private static void LobbyMemberJoined(Lobby lobby, Friend friend)
    {
        ImageTexture profilePicture = GetProfilePictureAsync(friend.Id).Result;
        LobbyManager.AddPlayer(profilePicture, friend.Name, friend.Id);
    }

    public void LeaveLobby()
    {
        _lobby.Leave();
        _lobbyId = 0;
    }
    
    private static void LobbyMemberLeft(Lobby lobby, Friend friend)
    {
        LobbyManager.RemovePlayer(friend.Id.ToString());
    }
    
    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        Logger.Network($"User accepted lobby invite through Steam UI. Joining lobby: {lobby.Id}");
        JoinLobby(lobby.Id.ToString());
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
    
    public void InviteLobbyOverlay()
    {
        SteamFriends.OpenGameInviteOverlay(_lobbyId);
    }

    public async void JoinLobby(string lobbyId)
    {
        try
        {
            var result = await SteamMatchmaking.JoinLobbyAsync(ulong.Parse(lobbyId));
            if (!result.HasValue)
            {
                Logger.Error($"Failed to join lobby: {lobbyId}");
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"Error joining lobby: {ex.Message}");
        }
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
}