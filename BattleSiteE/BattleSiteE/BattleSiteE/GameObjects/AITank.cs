using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
{
    class AITank : Tank
    {
        public AITank(int x, int y, Bearing b)
            : base(Color.White, x, y, b, PlayerIndex.Four)
        {
            // nothing to see here
        }

        public override void Update(int[,] collisionGrid)
        {
            

        }

    }
}
