using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using BattleSiteE.Manager;
using System.Diagnostics;

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
            Buttons bb = getButtonForAction(k);

            if (player.HasValue)
            {
                return (LastGamepadStates[(int)(player.Value)].IsButtonUp(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonDown(bb));    
            }

            return (LastGamepadStates[0].IsButtonUp(bb) && CurrentGamepadStates[0].IsButtonDown(bb)) || (LastGamepadStates[1].IsButtonUp(bb) && CurrentGamepadStates[1].IsButtonDown(bb));
        }

        public override bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            Buttons bb = getButtonForAction(k);

            if (player.HasValue)
            {
                return (LastGamepadStates[(int)(player.Value)].IsButtonDown(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonDown(bb));
            }

            return (LastGamepadStates[0].IsButtonDown(bb) && CurrentGamepadStates[0].IsButtonDown(bb)) || (LastGamepadStates[1].IsButtonDown(bb) && CurrentGamepadStates[1].IsButtonDown(bb));
        }

        public override bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            Buttons bb = getButtonForAction(k);

            if (player.HasValue)
            {
                return (LastGamepadStates[(int)(player.Value)].IsButtonDown(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonUp(bb));
            }

            return (LastGamepadStates[0].IsButtonDown(bb) && CurrentGamepadStates[0].IsButtonUp(bb)) || (LastGamepadStates[1].IsButtonDown(bb) && CurrentGamepadStates[1].IsButtonUp(bb));
        }

        public Buttons getButtonForAction(GameKey k)
        {
            switch (k)
            {
                case GameKey.BACK:
                    return Buttons.B;
                case GameKey.SELECT:
                    return Buttons.A;
                case GameKey.FIRE:
                    return Buttons.RightTrigger;
                case GameKey.UP:
                    return Buttons.DPadUp;
                case GameKey.DOWN:
                    return Buttons.DPadDown;
                case GameKey.LEFT:
                    return Buttons.DPadLeft;
                case GameKey.RIGHT:
                    return Buttons.DPadRight;
                default:
                    return Buttons.BigButton;
            }
        }
       


    }
}
