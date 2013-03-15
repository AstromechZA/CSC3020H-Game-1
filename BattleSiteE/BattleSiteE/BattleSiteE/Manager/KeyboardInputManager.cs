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
     * On windows:
     * Main Keyboard
     **/
    public class KeyboardInputManager : InputManager
    {
        public KeyboardState CurrentKeyboardState;

        public KeyboardState LastKeyboardState;

        public KeyboardInputManager()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
        }

        public override void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public override bool isKeyDown(GameKey k, PlayerIndex? player)
        {
            if (player.HasValue)
            {
                Keys kk = getKeyForAction(k, player);
                return (CurrentKeyboardState.IsKeyDown(kk) && LastKeyboardState.IsKeyUp(kk));
            }
            else
            {
                return isKeyDown(k, PlayerIndex.One) || isKeyDown(k, PlayerIndex.Two);
            }
        }

        public override bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            if (player.HasValue)
            {
                Keys kk = getKeyForAction(k, player);
                return (CurrentKeyboardState.IsKeyDown(kk) && LastKeyboardState.IsKeyDown(kk));
            }
            else
            {
                return isKeyPressed(k, PlayerIndex.One) || isKeyPressed(k, PlayerIndex.Two);
            }
        }

        public override bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            if (player.HasValue)
            {
                Keys kk = getKeyForAction(k, player);
                return (CurrentKeyboardState.IsKeyUp(kk) && LastKeyboardState.IsKeyDown(kk));
            }
            else
            {
                return isKeyUp(k, PlayerIndex.One) || isKeyUp(k, PlayerIndex.Two);
            }
        }

        public Keys getKeyForAction(GameKey k, PlayerIndex? player)
        {
            if (k == GameKey.BACK) return Keys.Escape;
            if (k == GameKey.SELECT) return Keys.Enter;

            if (k == GameKey.UP)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.W;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Up;
                if (player.HasValue && player == PlayerIndex.Three) return Keys.NumPad8;
                if (player.HasValue && player == PlayerIndex.Four) return Keys.I;
                return Keys.W;
            }
            if (k == GameKey.DOWN)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.S;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Down;
                if (player.HasValue && player == PlayerIndex.Three) return Keys.NumPad5;
                if (player.HasValue && player == PlayerIndex.Four) return Keys.K;
                return Keys.S;
            }
            if (k == GameKey.LEFT)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.A;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Left;
                if (player.HasValue && player == PlayerIndex.Three) return Keys.NumPad4;
                if (player.HasValue && player == PlayerIndex.Four) return Keys.J;
                return Keys.A;
            }
            if (k == GameKey.RIGHT)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.D;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Right;
                if (player.HasValue && player == PlayerIndex.Three) return Keys.NumPad4;
                if (player.HasValue && player == PlayerIndex.Four) return Keys.L;
                return Keys.D;
            }
            if (k == GameKey.FIRE)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.Space;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.NumPad0;
                if (player.HasValue && player == PlayerIndex.Three) return Keys.Enter;
                if (player.HasValue && player == PlayerIndex.Four) return Keys.OemSemicolon;
                return Keys.Space;
            }
            return Keys.F16;

        }


    }
}
