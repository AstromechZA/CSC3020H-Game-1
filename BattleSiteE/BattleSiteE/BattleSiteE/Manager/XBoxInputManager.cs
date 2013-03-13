using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using BattleSiteE.Manager;

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
        public const int MaxInputs = 2;

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

        public override bool isKeyDown(GameKey k, PlayerIndex? player)
        {
            return false;
        }

        public override bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            return false;
        }

        public override bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            return false;
        }


       


    }
}
