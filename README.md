# Godot Steam Lobby & Relay Server Multiplayer (C#)

This is a lightweight demo of a **Steam-integrated multiplayer lobby and relay-hosted server system**, built in **Godot 4 (C#)** using the **Steamworks SDK** and **Facepunch.Steamworks** wrapper.

Players can:
- Create or join a Steam lobby
- See who’s connected
- Send messages between players
- Transition into a synced mini-game using SteamNetworkingSockets

> ⚠️ This is a **work-in-progress** example project. Not production-ready, but useful for learning and prototyping.

---

## How to use
1. Download the Steamworks SDK from [here](https://partner.steamgames.com/)
2. Extract the `steam_api.dll` and `steam_api64.dll` files to the root of the project
3. Download the Facepunch Steamworks module from the releases page [here](https://github.com/Facepunch/Facepunch.Steamworks/releases)
4. Extract the `Facepunch.Steamworks.Win64.dll` into the `DLL` folder.