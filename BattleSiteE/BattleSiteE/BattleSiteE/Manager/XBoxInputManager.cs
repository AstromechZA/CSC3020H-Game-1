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
                if(LastGamepadStates[(int)(player.Value)].IsButtonUp(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonDown(bb))
                {
                    return true;
                }
                else
                {
                    bool l = getAxisForAction(LastGamepadStates[(int)(player.Value)], bb);
                    bool c = getAxisForAction(CurrentGamepadStates[(int)(player.Value)], bb);
                    return ((!l) && c);
                }                
            }
            else
            {
                return isKeyDown(k, PlayerIndex.One) || isKeyDown(k, PlayerIndex.Two);
            }

            
        }

        private bool getAxisForAction(GamePadState gamePadState, Buttons bb)
        {
            if (bb == Buttons.DPadDown)
            {
                return (gamePadState.ThumbSticks.Left.Y < -0.3);
            }
            if (bb == Buttons.DPadLeft)
            {
                return (gamePadState.ThumbSticks.Left.X < -0.3);
            }
            if (bb == Buttons.DPadRight)
            {
                return (gamePadState.ThumbSticks.Left.X > 0.3);
            }
            if (bb == Buttons.DPadUp)
            {
                return (gamePadState.ThumbSticks.Left.Y > 0.3);
            }
            return false;
        }


        public override bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            Buttons bb = getButtonForAction(k);

            if (player.HasValue)
            {
                if (LastGamepadStates[(int)(player.Value)].IsButtonDown(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonDown(bb))
                {
                    return true;
                }
                else
                {
                    bool l = getAxisForAction(LastGamepadStates[(int)(player.Value)], bb);
                    bool c = getAxisForAction(CurrentGamepadStates[(int)(player.Value)], bb);
                    return (l && c);
                }
            }
            else
            {
                return isKeyDown(k, PlayerIndex.One) || isKeyDown(k, PlayerIndex.Two);
            }

        }

        public override bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            Buttons bb = getButtonForAction(k);

            if (player.HasValue)
            {
                if (LastGamepadStates[(int)(player.Value)].IsButtonDown(bb) && CurrentGamepadStates[(int)(player.Value)].IsButtonUp(bb))
                {
                    return true;
                }
                else
                {
                    bool l = getAxisForAction(LastGamepadStates[(int)(player.Value)], bb);
                    bool c = getAxisForAction(CurrentGamepadStates[(int)(player.Value)], bb);
                    return (l && (!c));
                }
            }
            else
            {
                return isKeyDown(k, PlayerIndex.One) || isKeyDown(k, PlayerIndex.Two);
            }
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
