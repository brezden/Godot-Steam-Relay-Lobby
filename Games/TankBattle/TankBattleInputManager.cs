﻿namespace GodotPeer2PeerSteamCSharp.Games.TankBattle;
using GodotPeer2PeerSteamCSharp.Games;
using Steamworks.Data;

public class TankBattleInputManager : IInputManager
{
    public SendType InputDefaultSendType => SendType.Unreliable;

    public void ProcessPositionalInput(int playerIndex, float x, float y)
    {
        Logger.Game($"Player {playerIndex} moved to position ({x}, {y})");
    }

    public void ProcessActionInput(int playerIndex, string action)
    {
        Logger.Game($"Player {playerIndex} performed action: {action}");
    }
}
