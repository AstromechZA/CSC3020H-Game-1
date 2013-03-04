using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.Manager
{   

    public class InputManager
    {

        public InputManager()
        {

        }

        public virtual void Update() { }
        public virtual bool isMenuDown() { return false; }
        public virtual bool isMenuUp() { return false; }
        public virtual bool isMenuRight() { return false; }
        public virtual bool isMenuLeft() { return false; }
        public virtual bool isMenuSelect() { return false; }
        public virtual bool isMenuBack() { return false; }

        public virtual bool isControllerUp(PlayerIndex index) { return false; }
        public virtual bool isControllerRight(PlayerIndex index) { return false; }
        public virtual bool isControllerDown(PlayerIndex index) { return false; }
        public virtual bool isControllerLeft(PlayerIndex index) { return false; }
        public virtual bool isFiring(PlayerIndex index) { return false; }

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

        public bool isMenuDown()
        {
            return im.isMenuDown();
        }

        public bool isMenuUp()
        {
            return im.isMenuUp();
        }

        public bool isMenuRight()
        {
            return im.isMenuRight();
        }

        public bool isMenuLeft()
        {
            return im.isMenuLeft();
        }

        public bool isMenuSelect()
        {
            return im.isMenuSelect();
        }

        public bool isMenuBack()
        {
            return im.isMenuBack();
        }

        public bool isControllerUp(PlayerIndex index) 
        { 
            return im.isControllerUp(index); 
        }

        public bool isControllerRight(PlayerIndex index) 
        { 
            return im.isControllerRight(index); 
        }

        public bool isControllerDown(PlayerIndex index) 
        { 
            return im.isControllerDown(index); 
        }

        public bool isControllerLeft(PlayerIndex index) 
        { 
            return im.isControllerLeft(index); 
        }

        public bool isFiring(PlayerIndex index)
        {
            return im.isFiring(index);
        }

        

    }


}
