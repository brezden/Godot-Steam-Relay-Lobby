using Godot;

public partial class VersionLabel : Label
{
    public override void _Ready()
    {
        string v = "0.0.0";
        const string path = "res://VERSION";

        if (FileAccess.FileExists(path))
        {
            using var f = FileAccess.Open(path, FileAccess.ModeFlags.Read);
            v = f.GetAsText().Trim();   // trims \n/\r\n
        }

        Text = $"vPreRelease";
    }
}