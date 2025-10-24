using Godot;
using GodotPeer2PeerSteamCSharp.Modules.Input;

namespace GodotPeer2PeerSteamCSharp.Autoload;

public partial class InputManager : Node
{
   public static IInputHandler CurrentInputHandler;
   private static InputReceiver _inputReceiver;
   
   public static InputManager Instance
   {
       get;
       private set;
   }
   
   public override void _Ready()
   {
       Instance = this;
       
      SetProcess(false); 
       
     _inputReceiver = new InputReceiver();
     AddChild(_inputReceiver);
   }
   
   public void SetInputHandler(IInputHandler handler)
   {
       CurrentInputHandler = handler;
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
