using System;
using System.IO;
using System.Runtime.CompilerServices;
using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Lobby;

public enum LogType
{
    Game,
    Lobby,
    Network,
    Error
}

public static class Logger
{
    private static string logDir;
    private static string gameLogPath;
    private static string lobbyLogPath;
    private static string netLogPath;
    private static string errLogPath;

    static Logger()
    {
        InitLogPaths();
        EnsureLogsExist();
    }

    private static void InitLogPaths()
    {
#if DEBUG
        logDir = Path.Combine(ProjectSettings.GlobalizePath("res://"), "Logs");
#else
            logDir = Path.Combine(ProjectSettings.GlobalizePath("user://"), "Logs");
#endif

        gameLogPath = Path.Combine(logDir, "game_log.txt");
        lobbyLogPath = Path.Combine(logDir, "lobby_log.txt");
        netLogPath = Path.Combine(logDir, "network_log.txt");
        errLogPath = Path.Combine(logDir, "error_log.txt");
    }

    private static void EnsureLogsExist()
    {
        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);

        CreateFileIfMissing(gameLogPath);
        CreateFileIfMissing(lobbyLogPath);
        CreateFileIfMissing(netLogPath);
        CreateFileIfMissing(errLogPath);
    }

    private static void CreateFileIfMissing(string path)
    {
        if (!File.Exists(path))
            File.WriteAllText(path, ""); // Create empty file
    }

    private static void LogInternal(
        LogType type,
        string message,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var location = $"{Path.GetFileName(file)}:{line} ({member})";
        var formatted = $"[{timestamp}] [{location}] {message}";

        var path = type switch
        {
            LogType.Game => gameLogPath,
            LogType.Lobby => lobbyLogPath,
            LogType.Network => netLogPath,
            LogType.Error => errLogPath,
            _ => gameLogPath
        };

        switch (type)
        {
            case LogType.Error:
                GD.PrintErr(formatted);
                break;
            default:
                GD.Print(formatted);
                break;
        }

        try
        {
            File.AppendAllText(path, formatted + "\n");
        }
        catch (Exception e)
        {
            GD.PrintErr("Logger failed to write to file: " + e.Message);
        }
    }

    public static void Game(string msg,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        LogInternal(LogType.Game, msg, file, line, member);
    }
    
    public static void Lobby(string msg, bool visualLog = false,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        if (visualLog)
        {
            EventBus.Lobby.OnLobbyLog(msg);
        }
        LogInternal(LogType.Lobby, msg, file, line, member);
    }

    public static void Network(string msg,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        LogInternal(LogType.Network, msg, file, line, member);
    }

    public static void Error(string msg,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        LogInternal(LogType.Error, msg, file, line, member);
    }
}
