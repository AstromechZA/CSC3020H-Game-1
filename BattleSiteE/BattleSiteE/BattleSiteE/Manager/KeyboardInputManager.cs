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

        //Method for checking if a given key was just pressed
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));            
        }

        public override bool isMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }

        public override bool isMenuUp()
        {
            return IsNewKeyPress(Keys.Up);
        }

        public override bool isMenuRight()
        {
            return IsNewKeyPress(Keys.Right);
        }

        public override bool isMenuLeft()
        {
            return IsNewKeyPress(Keys.Left);
        }

        public override bool isMenuSelect()
        {
            return IsNewKeyPress(Keys.Enter) || IsNewKeyPress(Keys.Space);
        }

        public override bool isMenuBack()
        {
            return IsNewKeyPress(Keys.Escape) || IsNewKeyPress(Keys.Back);
        }

        public override bool isControllerUp(PlayerIndex index)
        {
            switch( index )
            {
                case PlayerIndex.One:
                    return CurrentKeyboardState.IsKeyDown(Keys.W);
                case PlayerIndex.Two:
                    return CurrentKeyboardState.IsKeyDown(Keys.Up);
                default:
                    return false;
            }
        }

        public override bool isControllerRight(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentKeyboardState.IsKeyDown(Keys.D);
                case PlayerIndex.Two:
                    return CurrentKeyboardState.IsKeyDown(Keys.Right);
                default:
                    return false;
            }
        }

        public override bool isControllerDown(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentKeyboardState.IsKeyDown(Keys.S);
                case PlayerIndex.Two:
                    return CurrentKeyboardState.IsKeyDown(Keys.Down);
                default:
                    return false;
            }
        }

        public override bool isControllerLeft(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentKeyboardState.IsKeyDown(Keys.A);
                case PlayerIndex.Two:
                    return CurrentKeyboardState.IsKeyDown(Keys.Left);
                default:
                    return false;
            }
        }

        public override bool isFiring(PlayerIndex index)
        {
            switch (index)
            {
                case PlayerIndex.One:
                    return CurrentKeyboardState.IsKeyDown(Keys.Space);
                case PlayerIndex.Two:
                    return CurrentKeyboardState.IsKeyDown(Keys.Enter);
                default:
                    return false;
            }
        }

    }
}
