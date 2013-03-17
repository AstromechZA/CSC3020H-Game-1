using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using BattleSiteE.GameObjects.Managers;
using BattleSiteE.GameObjects.WallTypes;

namespace BattleSiteE.GameObjects
{
    public class Bullet
    {
        private Vector2 position;
        private Bearing bearing;
        private Vector2 velocity;

        private Texture2D explosiontexture;
        private Texture2D texture;
        private TankBase parentTank;

        private float explosionProgress = 0.0f;
        

        public bool exploding;
        public bool markedForDeletion = false;



        Rectangle[] directionSprites = {
                                new Rectangle(0, 0, 32, 32),
                                new Rectangle(0, 32, 32, 32),
                                new Rectangle(32, 0, 32, 32),
                                new Rectangle(32, 32, 32, 32)
                              };

        Rectangle[] explosionSprites = {
                                new Rectangle(0, 0, 32, 32),
                                new Rectangle(32, 0, 32, 32),
                                new Rectangle(64, 0, 32, 32),
                                new Rectangle(96, 0, 32, 32),
                                new Rectangle(128, 0, 32, 32)
                              };




        public Bullet(TankBase firer, Vector2 firedFrom, Bearing b, float bulletv)
        {
            position = new Vector2(firedFrom.X, firedFrom.Y);
            bearing = b;
            parentTank = firer;

            exploding = false;

            if (bearing == Bearing.NORTH) this.velocity = new Vector2(0, -bulletv);
            else if (bearing == Bearing.SOUTH) this.velocity = new Vector2(0, bulletv);
            else if (bearing == Bearing.EAST) this.velocity = new Vector2(bulletv,0);
            else if (bearing == Bearing.WEST) this.velocity = new Vector2(-bulletv,0);
        }


        public void Update(GameTime gametime)
        {


            if (!exploding)
            {
                // update position
                position.X = position.X + velocity.X;
                position.Y = position.Y + velocity.Y;

                Rectangle r = getCollisionMask();

                // if collide with wall, just remove it
                WallBase rwc = WallManager.Instance.getCollidingWall(r);
                if (rwc != null)
                {
                    exploding = true;
                    WallManager.Instance.damage(r);
                }

                // if it collides with a tank
                TankBase rtc = TankManager.Instance.getCollidingTank(r);
                if (rtc != null && rtc != parentTank)
                {
                    if (rtc.GetType() == typeof(AITank))
                    {
                        if (this.parentTank.GetType() == typeof(PlayerTank))
                        {
                            ((AITank)rtc).damage();
                            ScoreManager.Instance.addPoints((int)(((PlayerTank)this.parentTank).controllingIndex),1);
                        }
                    }

                    if (rtc.GetType() == typeof(PlayerTank))
                    {
                        if (this.parentTank.GetType() == typeof(AITank))
                        {
                            ((PlayerTank)rtc).damage();
                            ScoreManager.Instance.addPoints((int)(((PlayerTank)rtc).controllingIndex),-1);
                            ScoreManager.Instance.addDeath((int)(((PlayerTank)rtc).controllingIndex));
                        }
                        else if (this.parentTank.GetType() == typeof(PlayerTank))
                        {
                            ((PlayerTank)rtc).damage();
                            ScoreManager.Instance.addPoints((int)(((PlayerTank)this.parentTank).controllingIndex),2);
                            ScoreManager.Instance.addPoints((int)(((PlayerTank)rtc).controllingIndex), -1);
                            ScoreManager.Instance.addDeath((int)(((PlayerTank)rtc).controllingIndex));
                        }
                    } 
                    exploding = true;
                }

                // if it collides with a bullet
                Bullet rbc = BulletManager.Instance.getCollidingBulletWithBullet(r, this);
                if (rbc != null)
                {
                    rbc.exploding = true;
                    exploding = true;
                }

            }
            else
            {
                // EXPLODIIIINGGGG
                if (explosionProgress == 0.0f)
                {
                    BulletManager.Instance.requestImpactSound();
                }

                explosionProgress += 0.1f;
                if (explosionProgress > 1.0f) markedForDeletion = true;

            }



        }

        public Rectangle getCollisionMask()
        {
            if (exploding) return Rectangle.Empty;

            if (bearing == Bearing.NORTH) return new Rectangle((int)position.X - 2, (int)position.Y - 5, 4, 10);
            else if (bearing == Bearing.SOUTH) return new Rectangle((int)position.X - 2, (int)position.Y - 5, 4, 10);
            else if (bearing == Bearing.EAST) return new Rectangle((int)position.X - 5, (int)position.Y - 2, 10, 4);
            else if (bearing == Bearing.WEST) return new Rectangle((int)position.X - 5, (int)position.Y - 2, 10, 4);
            else return Rectangle.Empty;
        }

        public void draw(SpriteBatch sb)
        {
            if (!exploding)
            {
                sb.Draw(texture, new Rectangle((int)position.X - 16, (int)position.Y - 16, 32, 32), directionSprites[(int)bearing], Color.White);
            }
            else
            {
                sb.Draw(explosiontexture, new Rectangle((int)position.X - 16, (int)position.Y - 16, 32, 32), explosionSprites[(int)(explosionProgress*5)], Color.White);
            }
        }

        public void setTextures(Texture2D bulletTex, Texture2D expTex)
        {
            texture = bulletTex;
            explosiontexture = expTex;
        }
    }
}
