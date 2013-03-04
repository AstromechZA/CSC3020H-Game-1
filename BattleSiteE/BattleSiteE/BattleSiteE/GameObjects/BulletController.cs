using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleSiteE.GameObjects
{
    class BulletController
    {
        Texture2D bulletTex;
        List<Bullet> bullets = new List<Bullet>();
        Rectangle[] sprites = {
                                new Rectangle(0, 0, 32, 32),
                                new Rectangle(0, 32, 32, 32),
                                new Rectangle(32, 0, 32, 32),
                                new Rectangle(32, 32, 32, 32)
                              };

        public BulletController () { }

        public void setTexture(Texture2D t)
        {
            bulletTex = t;
        }

        public void createAddBullet(float x, float y, Bearing d, float dx, float dy)
        {
            bullets.Add(new Bullet(x, y, d, dx, dy));
        }

        public void addBullet(Bullet b)
        {
            bullets.Add(b);
        }

        public void update(int[,] collisionGrid, int gridSize)
        {
            List<int> forRemoval = new List<int>();

            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                float x = b.position.X + b.velocity.X;
                float y = b.position.Y + b.velocity.Y;

                b.position = new Vector2(x, y);

                //check new position
                Vector2 s = new Vector2(b.position.X, b.position.Y);
                switch (b.direction)
                {
                    case Bearing.EAST: s.X += 5; break;
                    case Bearing.WEST: s.X -= 5; break;
                    case Bearing.NORTH: s.Y -= 5; break;
                    case Bearing.SOUTH: s.Y += 5; break;
                }

                int tX = (int)Math.Floor(s.X / gridSize);
                int tY = (int)Math.Floor(s.Y / gridSize);

                if (!((tX >= 0) && (tX < collisionGrid.GetLength(1)) && (tY >= 0) && (tY < collisionGrid.GetLength(0))))
                {
                    continue;
                }

                if (collisionGrid[tY, tX] > 0) forRemoval.Add(i);

            }

            foreach (int i in forRemoval)
            {
                bullets.RemoveAt(i);
            }


        }

        public void draw(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                sb.Draw(bulletTex, new Rectangle((int)b.position.X-16, (int)b.position.Y-16, 32, 32), sprites[(int)b.direction], Color.White);
            }

        }

        



    }
}
