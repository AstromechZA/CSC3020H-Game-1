using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace BattleSiteE.Manager
{

    public enum GameKey { UP, DOWN, LEFT, RIGHT, SELECT, BACK, FIRE };

    public abstract class InputManager
    {

        public InputManager()
        {

        }

        public virtual void Update() { }

        public abstract bool isKeyPressed(GameKey k, PlayerIndex? player);
        public abstract bool isKeyDown(GameKey k, PlayerIndex? player);
        public abstract bool isKeyUp(GameKey k, PlayerIndex? player);

    }

    public class InputController
    {

        InputManager im;

        public InputController()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            if (pid == PlatformID.Win32NT)
            {
                im = new KeyboardInputManager();
            }
            else
            {
                im = new XBoxInputManager();
            }
        }

        /**
         * Update the state of the InputManager,
         * This updates all keyboard and gamepad states
         */
        public void Update()
        {
            im.Update();
        }

        /**
         * Global interactions
         * For things like Menu's
         */

        public bool isKeyPressed(GameKey k, PlayerIndex? player)
        {
            return im.isKeyPressed(k, player);
        }

        public bool isKeyDown(GameKey k, PlayerIndex? player)
        {
            return im.isKeyDown(k, player);
        }

        public bool isKeyUp(GameKey k, PlayerIndex? player)
        {
            return im.isKeyUp(k, player);
        }


        

    }


}
