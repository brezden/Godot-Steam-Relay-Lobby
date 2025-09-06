# Godot Steam Lobby & Relay Server Multiplayer (C#)

This is a lightweight demo of a **Steam-integrated multiplayer lobby and relay-hosted server system**, built in **Godot 4 (C#)** using the **Steamworks SDK** and **Facepunch.Steamworks** wrapper.

Players can:
- Create or join a Steam lobby
- See who’s connected
- Send messages between players
- Transition into a synced mini-game using SteamNetworkingSockets

> ⚠️ This is a **work-in-progress** example project. Not production-ready, but useful for learning and prototyping.

---

## Versions Used
- [Facepunch Steamworks C#](https://github.com/Facepunch/Facepunch.Steamworks) : 2.4.1
- [Steamworks SDK](https://partner.steamgames.com): 1.61 

---
## General Setup

Make sure to modify the `app_id` inside of the `steam_appid.txt` file to whatever ID you are using for development.
You will also want to modify this value inside the `SteamLobbyService.cs` file which initializes steam.

This repository targets **.NET 6** so make sure you have the correct version installed.
```bash
sudo apt install dotnet-runtime-6.0
```

## Linux Setup

This project has been developed and tested on both Windows and Linux.  
Getting it working on Linux can be tricky, but following these instructions should save you a lot of time.

### Requirements for Linux

1. **Install Steam via APT (not Flatpak)**

   Installing Steam through Flatpak causes issues with Facepunch.Steamworks locating Steam libraries correctly.  
   Install the native Steam package instead:

   ```bash
   sudo apt update
   sudo apt install steam
   ```
   
   > You can also install it through steams website via a `.deb` file. 

2. **Fix `libsteam_api.so` Location**

   .NET expects `libsteam_api.so` to be found in a specific location.  
   You can create a symbolic link to point to the correct version:

   ```bash
   sudo ln -sf $(pwd)/libsteam_api.so /usr/lib/dotnet/shared/Microsoft.NETCore.App/9.0.4/libsteam_api.so
   ```

   > Replace `$(pwd)/libsteam_api.so` with the absolute path if running this from a different directory.
   
   > Make sure you modified the version in the path. You might not have 9.0.4

3. **Fix `steamclient.so` Location**

   Facepunch.Steamworks expects `steamclient.so` to be directly under `~/.steam/sdk64/`.  
   Create a symlink to ensure it's in the right place:

   ```bash
   ln -s ~/.steam/sdk64/linux64/steamclient.so ~/.steam/sdk64/steamclient.so
   ```
   
   > You might already have this in the correct place. If so, don't worry about this step.

Following these steps should allow Steam API initialization to succeed without missing libraries or entry point errors.

---

# Run Formatting
```bash
dotnet format Godot-Peer-2-Peer-Steam-CSharp.csproj
```
