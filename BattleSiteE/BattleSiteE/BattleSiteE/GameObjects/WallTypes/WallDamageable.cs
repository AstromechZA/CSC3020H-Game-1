using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects.WallTypes
{
    public class WallDamageable : WallBase
    {
        private static Rectangle sr = new Rectangle(0 * 32, 0 * 32, 32, 32);
        private int health = 2;
        private int initialhealth = 2;

        public WallDamageable() : base()
        {

        }

        public override Rectangle get_sprite_rectangle()
        {
            return sr;
        }



        public bool damage(int p)
        {
            health -= p;
            return (health <= 0);
        }

        public bool isDamaged()
        {
            return health < initialhealth;
        }



    }
}
