using Godot;
using Godot.NativeInterop;
using GodotPeer2PeerSteamCSharp.Games;
using GodotPeer2PeerSteamCSharp.Modules.Input;
using Steamworks.Data;

namespace GodotPeer2PeerSteamCSharp.Autoload;

public partial class InputManager : Node
{
   public static IInputHandler CurrentInputHandler;
   private static InputReceiver _inputReceiver;
   
   public override void _Ready()
   {
     _inputReceiver = new InputReceiver();
     AddChild(_inputReceiver);
   }
   
   public void SetInputHandler(IInputHandler handler)
   {
       CurrentInputHandler = handler;
       StartReceivingInput();
   }
   
   public void StartReceivingInput()
   {
       _inputReceiver.TurnOn();
       SetProcess(true);
   }
   
    public void StopReceivingInput()
    {
         _inputReceiver.TurnOff();
         SetProcess(false);
    }
}
