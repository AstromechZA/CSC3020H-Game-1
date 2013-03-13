using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleSiteE.GameObjects
{
    public class BulletManager
    {
        private static BulletManager instance;
        public static BulletManager Instance
        {
            get
            {
                if (instance == null) instance = new BulletManager();
                return instance;
            }
        }

        private List<Bullet> bullets;
        private Texture2D bulletTex;

        public BulletManager()
        {
            bullets = new List<Bullet>();
        }

        public void addBullet(Bullet p)
        {
            p.setTexture(bulletTex);
            bullets.Add(p);
        }

        public void setTexture(Texture2D t)
        {
            bulletTex = t;
        }

        public void updateBullets()
        {
            foreach (Bullet b in bullets)
            {
                b.update();
            }
        }

        public Bullet getCollidingBullet(Rectangle r)
        {
            foreach (Bullet b in bullets)
            {
                Rectangle other = b.getCollisionMask();
                if (other.Intersects(r)) return b;
            }
            return null;
        }

        public void drawBullets(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                b.draw(sb);
            }
        }

        public Bullet getCollidingBulletWithBullet(Rectangle r, Bullet bullet)
        {
            foreach (Bullet b in bullets)
            {
                if (bullet == b) continue;
                Rectangle other = b.getCollisionMask();
                if (other.Intersects(r)) return b;
            }
            return null;
        }
    }
}
