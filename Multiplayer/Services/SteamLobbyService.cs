using Godot;
using Steamworks;
using System;
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
                GD.Print($"Steam initialized successfully! User: {SteamClient.Name}");
                return true;
            }
            
            GD.PrintErr("Steam initialization failed.");
            return false;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Steam initialization error: {ex.Message}");
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
                GD.PrintErr("Failed to create Steam lobby.");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error creating lobby: {ex.Message}");
        }
    }

    private static void OnLobbyCreatedCallback(Result result, Lobby lobby)
    {
        LobbyManager.OnLobbyCreation(lobby.Id.ToString());
    }

    private static void OnLobbyEnteredCallback(Lobby lobby)
    {
        _lobby = lobby;
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

    public async Task GatherPlayerInformation()
    {
        SteamId id = SteamClient.SteamId;
        string name = SteamClient.Name;
        
        ImageTexture? profilePicture = await GetProfilePictureAsync(id);
    
        if (profilePicture != null)
        {
            LobbyManager.Instance.OnPlayerJoinedLobby(profilePicture, name, id);
        }
        else
        {
            GD.Print("No profile picture available for: " + name);
        }
    }
    
    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        GD.Print($"[DEBUG] User accepted lobby invite through Steam UI. Joining lobby: {lobby.Id}");
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
                GD.PrintErr($"Failed to join lobby: {lobbyId}");
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error joining lobby: {ex.Message}");
        }
    }
}