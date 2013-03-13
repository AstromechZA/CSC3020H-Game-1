using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BattleSiteE.GameObjects
{
    public class Bullet
    {
        private Vector2 position;
        private Bearing bearing;
        private Vector2 velocity;

        private Texture2D texture;
        private Tank parentTank;

        public bool markedForDeletion;

        Rectangle[] sprites = {
                                new Rectangle(0, 0, 32, 32),
                                new Rectangle(0, 32, 32, 32),
                                new Rectangle(32, 0, 32, 32),
                                new Rectangle(32, 32, 32, 32)
                              };


        public Bullet(Tank firer, Vector2 firedFrom, Bearing b, float bulletv)
        {
            position = new Vector2(firedFrom.X, firedFrom.Y);
            bearing = b;
            parentTank = firer;

            markedForDeletion = false;

            if (bearing == Bearing.NORTH) this.velocity = new Vector2(0, -bulletv);
            else if (bearing == Bearing.SOUTH) this.velocity = new Vector2(0, bulletv);
            else if (bearing == Bearing.EAST) this.velocity = new Vector2(bulletv,0);
            else if (bearing == Bearing.WEST) this.velocity = new Vector2(-bulletv,0);
        }


        public void update()
        {


            if (!markedForDeletion)
            {
                // update position
                position.X = position.X + velocity.X;
                position.Y = position.Y + velocity.Y;

                Rectangle r = getCollisionMask();

                // if collide with wall, just remove it
                if (WallManager.Instance.collides(r))
                {
                    markedForDeletion = true;
                }

                // if it collides with a tank
                Tank rtc = TankManager.Instance.getCollidingTank(r);
                if (rtc != null && rtc != parentTank)
                {
                    markedForDeletion = true;
                }

                // if it collides with a bullet
                Bullet rbc = BulletManager.Instance.getCollidingBulletWithBullet(r, this);
                if (rbc != null)
                {
                    markedForDeletion = true;
                }

            }
            else
            {
                // EXPLODIIIINGGGG

            }



        }

        public Rectangle getCollisionMask()
        {
            if (bearing == Bearing.NORTH) return new Rectangle((int)position.X - 3, (int)position.Y - 5, 6, 10);
            else if (bearing == Bearing.SOUTH) return new Rectangle((int)position.X - 3, (int)position.Y - 5, 6, 10);
            else if (bearing == Bearing.EAST) return new Rectangle((int)position.X - 5, (int)position.Y - 3, 10, 6);
            else if (bearing == Bearing.WEST) return new Rectangle((int)position.X - 5, (int)position.Y - 3, 10, 6);
            else return Rectangle.Empty;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X - 16, (int)position.Y - 16, 32, 32), sprites[(int)bearing], Color.White);
        }

        public void setTexture(Texture2D bulletTex)
        {
            texture = bulletTex;
        }
    }
}
