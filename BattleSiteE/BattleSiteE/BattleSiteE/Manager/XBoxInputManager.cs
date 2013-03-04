using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BattleSiteE.Manager
{
    /**
     * Input manager for the game and various screens
     * Call Update() from one of the screens
     * 
     * This updates the state of all the controllers:
     * On Xbox:
     * 4 chatpads
     *  OR
     * 4 xbox
     **/
    public class XBoxInputManager : InputManager
    {
        public const int MaxInputs = 4;

        public readonly GamePadState[] CurrentGamepadStates;

        public readonly GamePadState[] LastGamepadStates;

        public XBoxInputManager()
        {
            CurrentGamepadStates = new GamePadState[MaxInputs];
            LastGamepadStates = new GamePadState[MaxInputs];
        }

        public override void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastGamepadStates[i] = CurrentGamepadStates[i];

                CurrentGamepadStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }
        
        //Method for checking if a given button was just pressed
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                playerIndex = controllingPlayer.Value;
                int i = (int)playerIndex;

                return (CurrentGamepadStates[i].IsButtonDown(button) && CurrentGamepadStates[i].IsButtonUp(button));
            }
            else
            {
                return (
                    IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex)
                    );
            }
        }

        public override bool isMenuDown()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.DPadDown, null, out n); 
        }

        public override bool isMenuUp()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.DPadUp, null, out n); 
        }

        public override bool isMenuRight()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.DPadRight, null, out n);
        }

        public override bool isMenuLeft()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.DPadLeft, null, out n);
        }

        public override bool isMenuSelect()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.A, null, out n); 
        }

        public override bool isMenuBack()
        {
            PlayerIndex n;
            return IsNewButtonPress(Buttons.B, null, out n); 
        }

        public override bool isControllerUp(PlayerIndex index)
        {

            int i = (int)index;
            return CurrentGamepadStates[i].IsButtonDown(Buttons.DPadUp);
        }

        public override bool isControllerRight(PlayerIndex index)
        {
            int i = (int)index;
            return CurrentGamepadStates[i].IsButtonDown(Buttons.DPadRight);
        }

        public override bool isControllerDown(PlayerIndex index)
        {
            int i = (int)index;
            return CurrentGamepadStates[i].IsButtonDown(Buttons.DPadDown);
        }

        public override bool isControllerLeft(PlayerIndex index)
        {
            int i = (int)index;
            return CurrentGamepadStates[i].IsButtonDown(Buttons.DPadLeft);
        }

        public override bool isFiring(Microsoft.Xna.Framework.PlayerIndex index)
        {
            int i = (int)index;
            return CurrentGamepadStates[i].IsButtonDown(Buttons.RightTrigger);
        }


    }
}
