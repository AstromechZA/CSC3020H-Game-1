using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.GameObjects.Managers;

namespace BattleSiteE.GameScreens
{
    class PauseConfirmationScreen : ConfirmationScreen
    {
        GameplayScreen parent;

        public PauseConfirmationScreen(GameplayScreen gs) : base("ARE YOU SURE YOU WANT TO EXIT?", "YES", "NO") 
        {
            parent = gs;
        }

        public override void confirm()
        {
            ScreenManager.ExitAll();
            MusicManager.Instance.stop(3000);
            ScreenManager.AddScreen(new MenuBackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        public override void back()
        {
            parent.unpause();
            base.back();
        }
    }
}
