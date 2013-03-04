using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BattleSiteE.GameObjects
{
    enum Bearing {NORTH, SOUTH, EAST, WEST, NONE }

    class Tank : GameObject
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
        private const float stepSize = 1.0f;

        private Color tint;
        private PlayerIndex controllingIndex;
        public PlayerIndex ControllingIndex { get { return controllingIndex; } }

        private float gunAnimationProgress = 0.0f;
        private float gunAnimationDelta = 0.1f;



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

            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tread, Color.White);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tank, tint);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32 + g_offset_x), (int)(position.Y - 32 + g_offset_y), 64, 64), t_gun, tint);

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

        public virtual void Update(int [,] collisionGrid)
        {

            // GUN ANIMATIONS
            if (gunAnimationProgress > 0.001f)
            {
                gunAnimationProgress -= gunAnimationDelta;
                if (gunAnimationProgress < 0.001f) gunAnimationProgress = 0.0f;
            }
            
            // IF TARGET DIRECTION IS NONE, NO BUTTONS HAVE BEEN PRESSED AND WE CANT BE MOVING
            if (targetBearing == Bearing.NONE) { }
            // OTHERWISE IF WE NEED TO CHANGE DIRECTION
            else if (bearing != targetBearing)
            {

                // IF WE ARE GOING TO REVERSE, DO IT ANYTIME
                if (isHorizantal(bearing) == isHorizantal(targetBearing))
                {
                    bearing = targetBearing;
                }
                // OTHERWISE WAIT FOR AN INTERSECTION
                else if (nearIntersection(position, 2.0f))
                {
                    Vector2 temp = new Vector2(position.X, position.Y); //COPY

                    switch (targetBearing)
                    {
                        case Bearing.NORTH:
                            temp.Y -= stepSize;
                            temp.Y -= 32;
                            break;
                        case Bearing.SOUTH:
                            temp.Y += stepSize;
                            temp.Y += 32;
                            break;
                        case Bearing.EAST:
                            temp.X += stepSize;
                            temp.X += 32;
                            break;
                        case Bearing.WEST:
                            temp.X -= stepSize;
                            temp.X -= 32;
                            break;
                    }

                    // CHECK COLLISION
                    if (isFreePosition(temp, collisionGrid, 64, true))
                    {
                        bearing = targetBearing;
                        position = alignToIntersection(position);
                    }
                }
                
            }

            // OTHERWISE IF WE ARE MOVING
            if (targetBearing != Bearing.NONE)
            {

                //FIRST CHECK IF IT IS POSSIBLE TO MOVE TO THE NEW POSITION
                // 1 - MAKE TEMPORY POSITION
                Vector2 temp = new Vector2(position.X, position.Y); //COPY

                // 2 - GET POINT ON BORDER OF SPRITE
                switch (bearing)
                {
                    case Bearing.NORTH:
                        temp.Y -= stepSize;
                        temp.Y -= 32;
                        break;
                    case Bearing.SOUTH:
                        temp.Y += stepSize;
                        temp.Y += 32;
                        break;
                    case Bearing.EAST:
                        temp.X += stepSize;
                        temp.X += 32;
                        break;
                    case Bearing.WEST:
                        temp.X -= stepSize;
                        temp.X -= 32;
                        break;
                }                
                
                // 3 - CHECK COLLISION
                if (isFreePosition(temp, collisionGrid, 64, true))
                {
                    //5 - MOVE IN DIRECTION
                    switch (bearing)
                    {
                        case Bearing.NORTH:
                            position.Y -= stepSize;
                            break;
                        case Bearing.SOUTH:
                            position.Y += stepSize;
                            break;
                        case Bearing.EAST:
                            position.X += stepSize;
                            break;
                        case Bearing.WEST:
                            position.X -= stepSize;
                            break;
                    }
                }
            }


        }

        public void SetNextTargetBearing(Bearing next)
        {
            if (targetBearing != next) targetBearing = next;
        }

        public Bullet Fire()
        {
            gunAnimationProgress = 1.0f;
            float bulletspeed = 5.0f;
            float dx, dy;
            switch(bearing)
            {
                case Bearing.EAST: dx = bulletspeed; dy = 0f; break;
                case Bearing.WEST: dx = -bulletspeed; dy = 0f; break;
                case Bearing.NORTH: dx = 0f; dy = -bulletspeed; break;
                case Bearing.SOUTH: dx = 0f; dy = bulletspeed; break;
                default: dx = 0; dy = 0; break;
            }

            return new Bullet(position.X, position.Y, bearing, dx, dy);
        }

        public bool canFire()
        {
            return !(gunAnimationProgress > 0.00f);
        }

        public override Rectangle getCollisionMask()
        {
            return new Rectangle((int)(position.X-31), (int)(position.Y-31), 62, 62);
        }



        private bool isHorizantal(Bearing b)
        {
            if (b == Bearing.WEST) return true;
            if (b == Bearing.EAST) return true;
            return false;
        }

        private bool nearIntersection(Vector2 p, float range)
        {
            if ((Math.Abs((p.X % 64) - 32) < range) && (Math.Abs((p.Y % 64) - 32) < range)) return true;
            return false;
        }

        private Vector2 alignToIntersection(Vector2 p)
        {
            // align vectors on 0-64-128
            Vector2 output = new Vector2(p.X-32, p.Y-32);

            output.X = (float)Math.Round(output.X / 64) * 64 + 32;
            output.Y = (float)Math.Round(output.Y / 64) * 64 + 32;

            return output;
        }

        private bool isFreePosition(Vector2 position, int [,] collisionGrid, int gridSize, bool collideOffMap)
        {
            //First get index
            int tX = (int)Math.Floor(position.X / gridSize);
            int tY = (int)Math.Floor(position.Y / gridSize);

            //Check range
            if (!((tX >= 0) && (tX < collisionGrid.GetLength(1)) && (tY >= 0) && (tY < collisionGrid.GetLength(0))))
            {
                return !collideOffMap;
            }

            // If in range then its fine if the value is 0 or less
            return !(collisionGrid[tY, tX] > 0);
        }


    }
}
