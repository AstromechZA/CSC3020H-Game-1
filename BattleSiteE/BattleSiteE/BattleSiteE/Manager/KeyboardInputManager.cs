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
            Keys kk = getKeyForAction(k, player);
            return (CurrentKeyboardState.IsKeyDown(kk) && LastKeyboardState.IsKeyUp(kk));
        }

        public override bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            Keys kk = getKeyForAction(k, player);
            return (CurrentKeyboardState.IsKeyDown(kk) && LastKeyboardState.IsKeyDown(kk));
        }

        public override bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            Keys kk = getKeyForAction(k, player);
            return (CurrentKeyboardState.IsKeyUp(kk) && LastKeyboardState.IsKeyDown(kk));
        }

        public Keys getKeyForAction(GameKey k, PlayerIndex? player)
        {
            if (k == GameKey.BACK) return Keys.Escape;
            if (k == GameKey.SELECT) return Keys.Enter;

            if (k == GameKey.UP)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.W;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Up;
                return Keys.Up;
            }
            if (k == GameKey.DOWN)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.S;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Down;
                return Keys.Down;
            }
            if (k == GameKey.LEFT)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.A;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Left;
                return Keys.Left;
            }
            if (k == GameKey.RIGHT)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.D;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Right;
                return Keys.Right;
            }
            if (k == GameKey.FIRE)
            {
                if (player.HasValue && player == PlayerIndex.One) return Keys.Space;
                if (player.HasValue && player == PlayerIndex.Two) return Keys.Enter;
                return Keys.Enter;
            }
            return Keys.F16;

        }


    }
}
