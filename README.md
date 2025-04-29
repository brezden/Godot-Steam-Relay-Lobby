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

2. **Fix `libsteam_api.so` Location**

   .NET expects `libsteam_api.so` to be found in a specific location.  
   You can create a symbolic link to point to the correct version:

   ```bash
   sudo ln -sf $(pwd)/libsteam_api.so /usr/lib/dotnet/shared/Microsoft.NETCore.App/9.0.4/libsteam_api.so
   ```

   > Replace `$(pwd)/libsteam_api.so` with the absolute path if running this from a different directory.

3. **Fix `steamclient.so` Location**

   Facepunch.Steamworks expects `steamclient.so` to be directly under `~/.steam/sdk64/`.  
   Create a symlink to ensure it's in the right place:

   ```bash
   ln -s ~/.steam/sdk64/linux64/steamclient.so ~/.steam/sdk64/steamclient.so
   ```

Following these steps should allow Steam API initialization to succeed without missing libraries or entry point errors.
