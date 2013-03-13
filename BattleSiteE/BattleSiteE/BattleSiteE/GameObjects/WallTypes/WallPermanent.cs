using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects.WallTypes
{
    public class WallPermanent : WallBase
    {
        private static Rectangle sr = new Rectangle(1 * 32, 0 * 32, 32, 32);        

        public WallPermanent() : base()
        {

        }

       
        public override Rectangle get_sprite_rectangle()
        {
            return sr;
        }
    }
}
