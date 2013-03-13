using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
{
    public class AITank
    {
        private Vector2 position;
        private Bearing bearing;

        public AITank(int x, int y)
        {
            position = new Vector2(x, y);
            bearing = randomBearing();
        }

        public void update()
        {
            throw new NotImplementedException();
        }

        public void draw()
        {
            throw new NotImplementedException();
        }

        public static Bearing randomBearing()
        {
            Random r = new Random();
            switch (r.Next(0,3))
            {
                case 0: return Bearing.NORTH;
                case 1: return Bearing.SOUTH;
                case 2: return Bearing.EAST;
                default: return Bearing.WEST;                
            }
        }
    }
}
