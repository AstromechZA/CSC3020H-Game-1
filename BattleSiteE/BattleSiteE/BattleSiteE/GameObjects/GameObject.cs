using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
{
    public abstract class GameObject
    {
        public GameObject()
        {


        }

        public abstract Rectangle getCollisionMask();

    }
}
