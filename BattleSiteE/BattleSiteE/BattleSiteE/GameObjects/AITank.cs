using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BattleSiteE.GameObjects.Managers;

namespace BattleSiteE.GameObjects
{
    public class AITank : TankBase
    {

        private DateTime nextTurnTime;
        private DateTime lastFired;
        private DateTime nextFireQ;
        private TimeSpan timebtwnshots = TimeSpan.FromSeconds(2);
        private int initialhealth = 1;
        private int health = 1;
        public bool markedForDeletion = false;
        
        private static Random random = new Random();

        public AITank(int x, int y)
        {
            position = new Vector2(x, y);
            bearing = randomBearing(bearing);
            nextTurnTime = DateTime.Now;
            lastFired = DateTime.Now;
            nextFireQ = DateTime.Now + TimeSpan.FromSeconds(random.Next(3, 8)); 
            
        }

        public override void Update(GameTime gametime)
        {
            if (spawnState == SpawnState.SPAWNING)
            {
                spawnProgress += 0.03f;
                if (spawnProgress >= 1.0f) spawnState = SpawnState.SPAWNED;
            }
            else if (spawnState == SpawnState.SPAWNED)
            {

                // FIRST get current mask
                Rectangle r = getCollisionMask();

                // THEN work out movement diffs
                float tx = r.X;
                float ty = r.Y;

                float dx = 0;
                float dy = 0;

                switch (bearing)
                {
                    case Bearing.NORTH:
                        dy -= stepSize;
                        break;
                    case Bearing.SOUTH:
                        dy += stepSize;
                        break;
                    case Bearing.EAST:
                        dx += stepSize;
                        break;
                    case Bearing.WEST:
                        dx -= stepSize;
                        break;
                }

                tx += dx;
                ty += dy;

                //apply movement diffs
                r.X = (int)tx;
                r.Y = (int)ty;

                // IF NO COLLISION with WALL or another TANK
                if (!WallManager.Instance.collides(r) && !(TankManager.Instance.tankCollisionWithTank(r, this)))
                {
                    position.X = tx + r.Width / 2;
                    position.Y = ty + r.Height / 2;


                    if (DateTime.Now > nextTurnTime && isAtIntersection(position, 10))
                    {

                        bearing = randomBearing(bearing);
                        nextTurnTime = DateTime.Now + TimeSpan.FromSeconds(random.Next(2, 10));
                    }
                }
                else
                {
                    bearing = randomBearing(bearing);
                }


                // FIRE CONTROL
                if ((DateTime.Now - lastFired) > timebtwnshots)
                {


                    // search for player tank in front
                    int rw = (dx != 0) ? 1000 : 5;
                    int rh = (dy != 0) ? 1000 : 5;
                    int rx = (int)position.X;
                    int ry = (int)position.Y;

                    if (dx < 0) rx -= rw;
                    if (dy < 0) ry -= rh;

                    Rectangle searchR = new Rectangle(rx, ry, rw, rh);

                    if (TankManager.Instance.tankCollisionWithTank(searchR, this))
                    {
                        BulletManager.Instance.addBullet(new Bullet(this, position, bearing, 10.0f));
                        lastFired = DateTime.Now;

                    }
                    else if (DateTime.Now > nextFireQ)
                    {
                        BulletManager.Instance.addBullet(new Bullet(this, position, bearing, 10.0f));
                        lastFired = DateTime.Now;
                        nextFireQ = DateTime.Now + TimeSpan.FromSeconds(random.Next(3, 8));
                    }

    

                    
                }



            }
            else // state == despawning
            {
                spawnProgress -= 0.075f;
                if (spawnProgress < 0.0f) markedForDeletion = true;


            }
        }

        private bool isAtIntersection(Vector2 p, float accuracy)
        {
            float tx = p.X -31;
            float ty = p.Y -31;

            //check alignment
            int ax = (int)Math.Round(tx / 32) * 32;
            int ay = (int)Math.Round(ty / 32) * 32;

            return (Math.Sqrt(Math.Pow(ax - tx, 2) + Math.Pow(ay - ty, 2)) < accuracy);

        }

        public static Bearing randomBearing( Bearing notbearing)
        {
            int i = random.Next(0, 4);
            Bearing b = Bearing.NONE;
            if (i == 0) b = Bearing.NORTH;
            else if (i == 1) b = Bearing.SOUTH;
            else if (i == 2) b = Bearing.EAST;
            else b = Bearing.WEST;

            if (b == notbearing) return randomBearing(notbearing);
            return b;

        }


        public void damage()
        {
            if (--health <= 0)
            {
                spawnProgress = 1.0f;
                spawnState = SpawnState.UNSPAWNING;
            }
        }
    }
}
