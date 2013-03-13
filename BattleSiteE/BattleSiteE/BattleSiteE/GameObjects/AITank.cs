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

        public AITank(int x, int y)
        {
            position = new Vector2(x, y);
            bearing = randomBearing();
            nextTurnTime = DateTime.Now;
        }

        public override void Update()
        {
            if (spawnState == SpawnState.SPAWNING)
            {
                spawnProgress += 0.01f;
                if (spawnProgress >= 1.0f) spawnState = SpawnState.SPAWNED;
            }
            else if (spawnState == SpawnState.SPAWNED)
            {

                // FIRST get current mask
                Rectangle r = getCollisionMask();

                // THEN work out movement diffs
                float tx = r.X;
                float ty = r.Y;

                switch (bearing)
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

                if (!WallManager.Instance.collides(r) && !(TankManager.Instance.tankCollisionWithTank(r, this)))
                {
                    position.X = tx + r.Width / 2;
                    position.Y = ty + r.Height / 2;


                    if (DateTime.Now > nextTurnTime && isAtIntersection(position))
                    {

                        bearing = randomBearing();
                        Random rand = new Random();
                        nextTurnTime = DateTime.Now + TimeSpan.FromSeconds(rand.Next(2, 10));
                    }
                }
                else
                {
                    bearing = randomBearing();
                }




            }
            else // state == despawning
            {

            }
        }

        private bool isAtIntersection(Vector2 p)
        {
            float tx = p.X -31;
            float ty = p.Y -31;

            //check alignment
            int ax = (int)Math.Round(tx / 32) * 32;
            int ay = (int)Math.Round(ty / 32) * 32;

            return(Math.Sqrt(Math.Pow(ax - tx, 2) + Math.Pow(ay - ty, 2)) < 10);

        }

    }
}
