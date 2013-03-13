using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using BattleSiteE.GameScreens;
using BattleSiteE.Manager;
using BattleSiteE.GameObjects.Managers;

namespace BattleSiteE.GameObjects
{
    public enum Bearing {NORTH, SOUTH, EAST, WEST, NONE }

    public class Tank : GameObject
    {
        Texture2D tanktexture;

        private static Rectangle tankright = new Rectangle  (0, 0, 64, 64);
        private static Rectangle tankdown = new Rectangle   (64, 0, 64, 64);
        private static Rectangle tankup = new Rectangle     (128, 0, 64, 64);
        private static Rectangle tankleft = new Rectangle  (192, 0, 64, 64);

        private static Rectangle gunright = new Rectangle   (0, 64, 64, 64);
        private static Rectangle gundown = new Rectangle    (64, 64, 64, 64);
        private static Rectangle gunup = new Rectangle      (128, 64, 64, 64);
        private static Rectangle gunleft = new Rectangle    (192, 64, 64, 64);

        private static Rectangle treadhorizantal = new Rectangle(0, 128, 64, 64);
        private static Rectangle treadvertical = new Rectangle(64, 128, 64, 64);

        private static Rectangle[] explosionFramesHorizantal = {
                                                         new Rectangle     (0, 192, 64, 64),
                                                         new Rectangle     (64, 192, 64, 64),
                                                         new Rectangle     (128, 192, 64, 64),
                                                         new Rectangle     (192, 192, 64, 64),
                                                         new Rectangle     (256, 192, 64, 64)
                                                     };
        private static Rectangle[] explosionFramesVertical = {
                                                         new Rectangle     (0, 256, 64, 64),
                                                         new Rectangle     (64, 256, 64, 64),
                                                         new Rectangle     (128, 256, 64, 64),
                                                         new Rectangle     (192, 256, 64, 64),
                                                         new Rectangle     (256, 256, 64, 64)
                                                     };

        private Vector2 position;
        private Bearing bearing = Bearing.NONE;
        private Bearing targetBearing = Bearing.NONE;

        // MOVEMENT CONSTS
        private const float stepSize = 2.0f;
        private const float turningrad = 4.0f;
        private const float bulletv = 10.0f;


        private Color tint;
        private PlayerIndex controllingIndex;
        public PlayerIndex ControllingIndex { get { return controllingIndex; } }

        private float gunAnimationProgress = 0.0f;
        private float gunAnimationDelta = 0.1f;

        private float spawningProgress = 0.0f;



        public Tank(Color tint, int x, int y, Bearing b, PlayerIndex controllingIndex)
        {
            this.tanktexture = null;
            this.tint = tint;
            this.position = new Vector2(x, y);
            this.bearing = b;
            this.controllingIndex = controllingIndex;
        }

        public void SetTexture(Texture2D tt)
        {
            this.tanktexture = tt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.tanktexture == null) return;

            Rectangle t_tread = treadhorizantal;
            Rectangle t_tank = tankright;
            Rectangle t_gun = gunright;
            float g_offset_x = 0;
            float g_offset_y = 0;
            float e_offset_x = 0;
            float e_offset_y = 0;
            SpriteEffects e_effect = SpriteEffects.None;

            Rectangle[] explosionFrames = explosionFramesHorizantal;
                

            switch (bearing)
            {
                case Bearing.EAST:
                    t_tread = treadhorizantal;
                    t_tank = tankright;
                    t_gun = gunright;
                    g_offset_x = -(gunAnimationProgress * 10);
                    e_offset_x = 28;
                    explosionFrames = explosionFramesHorizantal;
                    break;
                case Bearing.WEST:
                    t_tread = treadhorizantal;
                    t_tank = tankleft;
                    t_gun = gunleft;
                    g_offset_x = +(gunAnimationProgress * 10);
                    e_offset_x = -28;
                    explosionFrames = explosionFramesHorizantal;
                    e_effect = SpriteEffects.FlipHorizontally;
                    break;
                case Bearing.NORTH:
                    t_tread = treadvertical;
                    t_tank = tankup;
                    t_gun = gunup;
                    e_offset_y = -28;
                    g_offset_y = +(gunAnimationProgress * 10);
                    explosionFrames = explosionFramesVertical;
                    e_effect = SpriteEffects.FlipVertically;
                    break;
                case Bearing.SOUTH:
                    t_tread = treadvertical;
                    t_tank = tankdown;
                    t_gun = gundown;
                    e_offset_y = 28;
                    g_offset_y = -(gunAnimationProgress * 10);
                    explosionFrames = explosionFramesVertical;
                    break;
            }

            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tread, Color.White * spawningProgress);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tank, tint * spawningProgress);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32 + g_offset_x), (int)(position.Y - 32 + g_offset_y), 64, 64), t_gun, tint * spawningProgress);

            // should we be showing a firing sprite?
            if (gunAnimationProgress > 0.001f)
            {
                                

                int index = (int)Math.Floor((1.0f - gunAnimationProgress) / 0.2f);
                
                spriteBatch.Draw(
                    tanktexture,
                    new Rectangle((int)(position.X - 32 + g_offset_x + e_offset_x), (int)(position.Y - 32 + g_offset_y + e_offset_y), 64, 64),
                    explosionFrames[index],
                    tint,
                    0.0f,
                    new Vector2(0, 0),
                    e_effect,
                    0);


            }

            

        }

        public virtual void Update()
        {

            // GUN ANIMATIONS
            if (gunAnimationProgress > 0.001f)
            {
                gunAnimationProgress -= gunAnimationDelta;
                if (gunAnimationProgress < 0.001f) gunAnimationProgress = 0.0f;
            }

            // SPAWNING fade in
            if (spawningProgress < 1.0f)
            {
                spawningProgress += 0.03f;
                return;
            }

            // CHECK FOR direction change
            if (targetBearing != Bearing.NONE)
            {

                // FIRST get current mask
                Rectangle r = getCollisionMask();

                // THEN work out movement diffs
                float tx = r.X;
                float ty = r.Y;

                switch (targetBearing)
                {
                    case Bearing.NORTH:
                        ty -= stepSize;
                        break;
                    case Bearing.SOUTH:
                        ty += stepSize;
                        break;
                    case Bearing.EAST:
                        tx += stepSize;
                        break;
                    case Bearing.WEST:
                        tx -= stepSize;
                        break;
                }

                //apply movement diffs
                r.X = (int)tx;
                r.Y = (int)ty;

                // IF the new mask position does not collide, apply the movement diffs to the position


                if (!WallManager.Instance.collides(r) && !(TankManager.Instance.tankCollisionWithTank(r, this)) )
                {
                    position.X = tx + r.Width / 2;
                    position.Y = ty + r.Height / 2;
                    bearing = targetBearing;
                }
                else
                {
                    // help with alignment
                    int ax = (int)Math.Round(tx / 32) * 32;
                    int ay = (int)Math.Round(ty / 32) * 32;

                    if (Math.Sqrt(Math.Pow(ax - tx, 2) + Math.Pow(ay - ty, 2)) < 10)
                    {
                        tx = ax;
                        ty = ay;

                        //apply movement diffs
                        r.X = (int)tx;
                        r.Y = (int)ty;

                        if (!WallManager.Instance.collides(r) && !(TankManager.Instance.tankCollisionWithTank(r, this)))
                        {
                            position.X = tx + r.Width / 2;
                            position.Y = ty + r.Height / 2;
                        }

                        bearing = targetBearing;
                    }

                }
            }
        }

        public bool canFire()
        {
            return !(gunAnimationProgress > 0.00f);
        }

        public Bullet Fire()
        {
            gunAnimationProgress = 1.0f;
            return new Bullet(this, position, bearing, bulletv);

        }

        /**
         * Collision mask, 1px samller than the borders just as the sprite.
         **/
        public override Rectangle getCollisionMask()
        {
            return new Rectangle((int)(position.X-31), (int)(position.Y-31), 62, 62);
        }

        public Vector2 alignPointToGrid(float x, float y, int gridSize)
        {
            return new Vector2 ((int)Math.Round(x/gridSize)*gridSize, (int)Math.Round(y/gridSize)*gridSize);
        }

        /**
         * Attempt to change direction of the tank. This just sets the target bearing, Update() does the actual collision detection
         * and movement whilst Draw() uses the actual bearing variable to draw the correct sprite.
         */
        public void attemptDirection(Bearing b)
        {
            targetBearing = b;
        }

        public void handleInput(Manager.InputController inputController)
        {
            // first reset target bearing
            targetBearing = Bearing.NONE;

            if (inputController.isKeyPressed(GameKey.UP, ControllingIndex)) targetBearing = Bearing.NORTH;
            else if (inputController.isKeyPressed(GameKey.DOWN, ControllingIndex)) targetBearing = Bearing.SOUTH;
            else if (inputController.isKeyPressed(GameKey.LEFT, ControllingIndex)) targetBearing = Bearing.WEST;
            else if (inputController.isKeyPressed(GameKey.RIGHT, ControllingIndex)) targetBearing = Bearing.EAST;
            if (inputController.isKeyPressed(GameKey.FIRE, ControllingIndex))
            {
                if (canFire())
                {
                    BulletManager.Instance.addBullet(Fire());
                }
            }



        }
    }
}
