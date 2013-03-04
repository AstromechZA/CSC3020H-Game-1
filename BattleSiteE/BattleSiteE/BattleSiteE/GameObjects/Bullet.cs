using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
{
    class Bullet
    {
        public Vector2 position;
        public Bearing direction;
        public Vector2 velocity;

        public Bullet(float x, float y, Bearing d, float dx, float dy)
        {
            position = new Vector2(x, y);
            direction = d;
            velocity = new Vector2(dx, dy);
        }
    }
}
