using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace BattleSiteE.GameObjects.Managers
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
        private Texture2D explosionTex;
        private SoundEffect fireSound;
        private SoundEffect impactSound;

        public BulletManager()
        {
            bullets = new List<Bullet>();
        }

        public void addBullet(Bullet p)
        {
            fireSound.Play(0.2f, 0.0f, 0.0f);
            p.setTextures(bulletTex, explosionTex);
            bullets.Add(p);
        }

        public void setTexture(Texture2D t, Texture2D et)
        {
            bulletTex = t;
            explosionTex = et;
        }

        public void updateBullets(GameTime gametime)
        {

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gametime);
                if (bullets[i].markedForDeletion)
                {
                    bullets.Remove(bullets[i]);
                }
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

        internal void clear()
        {
            instance = new BulletManager();
        }

        public void setFireSound(SoundEffect soundEffect)
        {
            fireSound = soundEffect;
        }

        public void setImpactSound(SoundEffect ims)
        {
            impactSound = ims;
            
        }

        public void requestImpactSound()
        {
            impactSound.Play(0.05f, 0.0f, 0.0f);
        }
    }
}
