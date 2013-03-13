using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects.WallTypes
{
    public abstract class WallBase
    {
        public WallBase()
        {
            
        }

        public abstract Rectangle get_sprite_rectangle();
    }
}
