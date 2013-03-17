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
        // SINGLETON
        private static BulletManager instance;
        public static BulletManager Instance
        {
            get
            {
                if (instance == null) instance = new BulletManager();
                return instance;
            }
        }

        // all bullets in the game
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
            p.setTextures(bulletTex, explosionTex);         // set preloaded textures
            bullets.Add(p);
        }

        public void setTexture(Texture2D t, Texture2D et)
        {
            bulletTex = t;
            explosionTex = et;
        }

        public void updateBullets(GameTime gametime)
        {
            // for each bullet: update: remove if dead
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
            // get any bullet that collides with the given rectangle
            foreach (Bullet b in bullets)
            {
                Rectangle other = b.getCollisionMask();
                if (other.Intersects(r)) return b;
            }
            return null;
        }

        // Draw all the bullet
        public void drawBullets(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                b.draw(sb);
            }
        }

        // Get any bullet that is colliding with a rectangle and is not this bullet
        public Bullet getCollidingBulletWithBullet(Rectangle r, Bullet bullet)
        {
            foreach (Bullet b in bullets)
            {
                // if this, ignore
                if (bullet == b) continue;
                Rectangle other = b.getCollisionMask();
                if (other.Intersects(r)) return b;
            }
            return null;
        }

        // reinit
        public void clear()
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
