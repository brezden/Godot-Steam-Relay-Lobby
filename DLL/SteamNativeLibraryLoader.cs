using Godot;
using System;
using System.IO;
using System.Runtime.InteropServices;

public static class SteamNativeLibraryLoader
{
    [DllImport("kernel32", SetLastError = true)]
    private static extern bool SetDllDirectory(string lpPathName);

    public static void LoadSteamLibrary()
    {
        string gameDir = AppDomain.CurrentDomain.BaseDirectory;
        string libraryPath = Path.Combine(gameDir, GetSteamLibraryName());
        
        Console.WriteLine($"Attempting to load Steam API from: {libraryPath}");
        
        if (!File.Exists(libraryPath))
        {
            Console.WriteLine($"Steam API library not found at: {libraryPath}");
            
            // Check the project directory (useful when running from the editor)
            string projectDir = Directory.GetCurrentDirectory();
            libraryPath = Path.Combine(projectDir, GetSteamLibraryName());
            
            Console.WriteLine($"Trying alternative path: {libraryPath}");
            
            if (!File.Exists(libraryPath))
            {
                Console.WriteLine($"Steam API library not found at alternative path: {libraryPath}");
                throw new FileNotFoundException($"Could not find Steam API library: {GetSteamLibraryName()}");
            }
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SetDllDirectory(Path.GetDirectoryName(libraryPath));
        }
        
        Console.WriteLine($"Steam API library location confirmed: {libraryPath}");
    }
    
    private static string GetSteamLibraryName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return System.Environment.Is64BitProcess ? "steam_api64.dll" : "steam_api.dll";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "libsteam_api.so";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "libsteam_api.dylib";
        }
        
        throw new PlatformNotSupportedException("Unsupported platform for Steam integration");
    }
}