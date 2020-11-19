//Using di sistema
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//DurectX
using Microsoft.DirectX.DirectInput;
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;

namespace DesdinovaModelPipeline
{
    public class GameControllerState
    {
        public ButtonState Button1;
        public ButtonState Button2;
        public ButtonState Button3;
        public ButtonState Button4;
        public ButtonState Button5;
        public ButtonState Button6;
        public ButtonState Button7;
        public ButtonState Button8;
        public ButtonState Button9;
        public ButtonState Button10;
        public ButtonState Left;
        public ButtonState Right;
        public ButtonState Up;
        public ButtonState Down;
        public int AnalogY;
        public int AnalogX;
        public float AnalogRight;
        public float AnalogLeft;
        public float AnalogUp;
        public float AnalogDown;
        public int TotalButtons;
        public byte[] Buttons;

        public GameControllerState()
        {
            Button1 = ButtonState.Released;
            Button2 = ButtonState.Released;
            Button3 = ButtonState.Released;
            Button4 = ButtonState.Released;
            Button5 = ButtonState.Released;
            Button6 = ButtonState.Released;
            Button7 = ButtonState.Released;
            Button8 = ButtonState.Released;
            Button9 = ButtonState.Released;
            Button10 = ButtonState.Released;
            Left = ButtonState.Released;
            Right = ButtonState.Released;
            Up = ButtonState.Released;
            Down = ButtonState.Released;
            AnalogY = 0;
            AnalogX = 0;
            AnalogRight = 0.0f;
            AnalogLeft = 0.0f;
            AnalogUp = 0.0f;
            AnalogDown = 0.0f;
            TotalButtons = 0;
            Buttons = null;

        }
    }

    public class DirectInputGameController
    {
        Device device;
        public DirectInputGameController(Guid gamepadInstanceGuid)
        {
            this.device = new Device(gamepadInstanceGuid);
            this.device.SetDataFormat(DeviceDataFormat.Joystick);
            this.device.Acquire();
        }

        public GameControllerState GetState()
        {
            GameControllerState gc = new GameControllerState();
            gc.TotalButtons = device.Caps.NumberButtons;
            gc.Buttons = device.CurrentJoystickState.GetButtons();
            
            //X
            if (device.CurrentJoystickState.X == 0) gc.Left = ButtonState.Pressed;
            else gc.Left = ButtonState.Released;
            if (device.CurrentJoystickState.X == 65535) gc.Right = ButtonState.Pressed;
            else gc.Right = ButtonState.Released;

            //Y
            if (device.CurrentJoystickState.Y == 0) gc.Up = ButtonState.Pressed;
            else gc.Up = ButtonState.Released;
            if (device.CurrentJoystickState.Y == 65535) gc.Down = ButtonState.Pressed;
            else gc.Down = ButtonState.Released;

            //Z
            gc.AnalogX = device.CurrentJoystickState.Rz;    //posizione asse X del controller analogico
            gc.AnalogY = device.CurrentJoystickState.Z;     //posizione asse Y del controller analogico
            
            //Posizione del controlle analogico:
            //
            //            0
            //            |
            //            |
            //            |
            //            |31734   
            //  0 --------O-------- 65535
            //            |
            //            | 
            //            |
            //            |
            //          65535

            //X-Y Analogici
            gc.AnalogRight = 0.0f;
            gc.AnalogLeft = 0.0f;
            gc.AnalogUp = 0.0f;
            gc.AnalogDown = 0.0f;
            if ((gc.AnalogX >= 0)&&(gc.AnalogX < 31734))
            {
                gc.AnalogLeft = 1.0f - gc.AnalogX / 31734.0f;
                gc.AnalogRight = 0.0f;
            }
            if ((gc.AnalogX > 31734)&&(gc.AnalogX <= 65535))
            {
                gc.AnalogLeft = 0.0f;
                gc.AnalogRight = (gc.AnalogX - 31734.0f) / (65535.0f - 31734.0f);
            }
            if ((gc.AnalogY >= 0) && (gc.AnalogY < 31734))
            {
                gc.AnalogUp = 1.0f - gc.AnalogY / 31734.0f;
                gc.AnalogDown = 0.0f;
            }
            if ((gc.AnalogY > 31734) && (gc.AnalogY <= 65535))
            {
                gc.AnalogUp = 0.0f;
                gc.AnalogDown = (gc.AnalogY - 31734.0f) / (65535.0f - 31734.0f);
            }

            //Pulsanti
            if (gc.Buttons[0] != 0)    gc.Button1 = ButtonState.Pressed;
            else gc.Button1 = ButtonState.Released;
            if (gc.Buttons[1] != 0) gc.Button2 = ButtonState.Pressed;
            else gc.Button2 = ButtonState.Released;
            if (gc.Buttons[2] != 0) gc.Button3 = ButtonState.Pressed;
            else gc.Button3 = ButtonState.Released;
            if (gc.Buttons[3] != 0) gc.Button4 = ButtonState.Pressed;
            else gc.Button4 = ButtonState.Released;
            if (gc.Buttons[4] != 0) gc.Button5 = ButtonState.Pressed;
            else gc.Button5 = ButtonState.Released;
            if (gc.Buttons[5] != 0) gc.Button6 = ButtonState.Pressed;
            else gc.Button6 = ButtonState.Released;
            if (gc.Buttons[6] != 0) gc.Button7 = ButtonState.Pressed;
            else gc.Button7 = ButtonState.Released;
            if (gc.Buttons[7] != 0) gc.Button8 = ButtonState.Pressed;
            else gc.Button8 = ButtonState.Released;
            if (gc.Buttons[8] != 0) gc.Button9 = ButtonState.Pressed;
            else gc.Button9 = ButtonState.Released;
            if (gc.Buttons[9] != 0) gc.Button10 = ButtonState.Pressed;
            else gc.Button10 = ButtonState.Released;
 
            return gc;
        }
     }

    public class Input : EngineObject
    {
        //Comandi e stati
        private Microsoft.Xna.Framework.Input.KeyboardState oldState;

        //Controllers (by DirectInput)
        private List<DirectInputGameController> gamepads = null;

        //Controllers trovati
        private uint gameControllersCount = 0;
        public uint GameControllersCount
        {
            get { return gameControllersCount; }
        }

        //Creazione
        private bool gamepadCreated = false;

        private GameControllerState defaultControllerState = new GameControllerState();

        public Input ()
	    {
            IsCreated = true;   //E' sempre creato
            try
            {
                gamepads = new List<DirectInputGameController>();

                DeviceList devicesGamepad = Manager.GetDevices(Microsoft.DirectX.DirectInput.DeviceType.Gamepad, EnumDevicesFlags.AttachedOnly);
                DeviceList devicesJoystick = Manager.GetDevices(Microsoft.DirectX.DirectInput.DeviceType.Joystick, EnumDevicesFlags.AttachedOnly);

                gamepads = new List<DirectInputGameController>(devicesGamepad.Count + devicesJoystick.Count);
                foreach (DeviceInstance instance in devicesGamepad)
                {
                    DirectInputGameController item = new DirectInputGameController(instance.InstanceGuid);
                    gamepads.Add(item);
                }
                foreach (DeviceInstance instance in devicesJoystick)
                {
                    DirectInputGameController item = new DirectInputGameController(instance.InstanceGuid);
                    gamepads.Add(item);
                }
                gameControllersCount = (uint)gamepads.Count;

                //Creazione avvenuta
                gamepadCreated = true;
            }
            catch
            {
                //Creazione fallita (nessun supporto al gamepad)
                gamepadCreated = false;
            }
        }

	
        public void Update(GameTime gameTime)
        {          
        }


        public GameControllerState GetGamecontrollerState(int padIndex)
        {
            if (gamepadCreated)
            {
                if (gameControllersCount > 0)
                {
                    return gamepads[padIndex].GetState();
                }
                else
                {
                    return defaultControllerState;
                }
            }
            else
            {
                return defaultControllerState;
            }
        }

        public Microsoft.Xna.Framework.Input.MouseState GetMouseState()
        {
            return Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            if (IsCreated)
            {
                Microsoft.Xna.Framework.Input.KeyboardState newState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                if (newState[key] == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsKeyUp(Microsoft.Xna.Framework.Input.Keys key)
        {
            if (IsCreated)
            {
                Microsoft.Xna.Framework.Input.KeyboardState newState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
                //Se prima era giù e ora è su significa che è stato rilasciato
                if ((oldState[key] == Microsoft.Xna.Framework.Input.KeyState.Down) && (newState[key] == Microsoft.Xna.Framework.Input.KeyState.Up))
                {
                    oldState = newState;
                    return true;
                }

                //Salva lo stato corrente solo se è giù
                if (newState[key] == Microsoft.Xna.Framework.Input.KeyState.Down)
                {
                    oldState = newState;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        ~Input()
        {
            //Rilascia le risorse
        }
    }
}


/*Uso del gamepad
if (input.GetGamecontrollerState(0).Left == ButtonState.Pressed) player1.MoveBackward();
if (input.GetGamecontrollerState(0).Right == ButtonState.Pressed) player1.MoveForward();
if (input.GetGamecontrollerState(0).Up == ButtonState.Pressed) player1.MoveUp();
if (input.GetGamecontrollerState(0).Down == ButtonState.Pressed) player1.MoveDown();

if (input.GetGamecontrollerState(0).AnalogLeft > 0.0f) player1.MoveBackward(input.GetGamecontrollerState(0).AnalogLeft);
if (input.GetGamecontrollerState(0).AnalogRight > 0.0f) player1.MoveForward(input.GetGamecontrollerState(0).AnalogRight);
if (input.GetGamecontrollerState(0).AnalogUp > 0.0f) player1.MoveUp(input.GetGamecontrollerState(0).AnalogUp);
if (input.GetGamecontrollerState(0).AnalogDown > 0.0f) player1.MoveDown(input.GetGamecontrollerState(0).AnalogDown);
*/