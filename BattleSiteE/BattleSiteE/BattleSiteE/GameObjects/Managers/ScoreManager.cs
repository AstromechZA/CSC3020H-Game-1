using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleSiteE.GameObjects.Managers
{
    public class ScoreManager
    {

        private static ScoreManager instance;
        public static ScoreManager Instance
        {
            get
            {
                if (instance == null) instance = new ScoreManager();
                return instance;
            }
        }

        public int[] points;
        public int[] deaths;
        public int[] shotsfired;

        public ScoreManager()
        {
            clear();
        }

        public void clear()
        {
            points = new int[] { 0, 0, 0, 0 };
            deaths = new int[] { 0, 0, 0, 0 };
            shotsfired = new int[] { 0, 0, 0, 0 };
        }

        

    }
}
