using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BattleSiteE.GameScreens
{
    class ExitConfirmationScreen : ConfirmationScreen
    {

        public ExitConfirmationScreen() : base("ARE YOU SURE YOU WANT TO EXIT?","YES","NO") { }

        public override void confirm()
        {
            Debug.WriteLine("FINISH");
            ScreenManager.FinishGame();
        }

    }
}
