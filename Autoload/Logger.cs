using Godot;
using System;
using System.IO;
using System.Runtime.CompilerServices;

public enum LogType
{
    Game,
    Network,
    Error
}

public partial class Logger : Node
{
    private static string logDir;
    private static string gameLogPath;
    private static string netLogPath;
    private static string errLogPath;

    public override void _Ready()
    {
        InitLogPaths();
        EnsureLogsExist();

        Game("=== Game Log Started ===");
        Network("=== Network Log Started ===");
        Error("=== Error Log Started ===");
    }

    private static void InitLogPaths()
    {
        #if DEBUG
                logDir = Path.Combine(ProjectSettings.GlobalizePath("res://"), "Logs");
        #else
            logDir = ProjectSettings.GlobalizePath("user://Logs");
        #endif

        gameLogPath = Path.Combine(logDir, "game_log.txt");
        netLogPath  = Path.Combine(logDir, "network_log.txt");
        errLogPath  = Path.Combine(logDir, "error_log.txt");
    }

    private static void EnsureLogsExist()
    {
        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);

        CreateFileIfMissing(gameLogPath);
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
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string location = $"{Path.GetFileName(file)}:{line} ({member})";
        string formatted = $"[{timestamp}] [{location}] {message}";

        string path = type switch
        {
            LogType.Game => gameLogPath,
            LogType.Network => netLogPath,
            LogType.Error => errLogPath,
            _ => gameLogPath
        };

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
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") =>
        LogInternal(LogType.Game, msg, file, line, member);

    public static void Network(string msg,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") =>
        LogInternal(LogType.Network, msg, file, line, member);

    public static void Error(string msg,
        [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") =>
        LogInternal(LogType.Error, msg, file, line, member);
}
