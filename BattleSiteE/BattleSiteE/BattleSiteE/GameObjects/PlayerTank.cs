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

    public class PlayerTank : TankBase
    {
        

        private Bearing targetBearing = Bearing.NONE;

        // MOVEMENT CONSTS
        private const float bulletv = 10.0f;
        private TimeSpan timebtwFire = TimeSpan.FromSeconds(1);

        private PlayerIndex controllingIndex;
        public PlayerIndex ControllingIndex { get { return controllingIndex; } }

        private DateTime lastfire = DateTime.Now;
        

        public PlayerTank(Color tint, int x, int y, Bearing b, PlayerIndex controllingIndex)
        {
            this.tint = tint;
            this.position = new Vector2(x, y);
            this.bearing = b;
            this.controllingIndex = controllingIndex;
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
                // GUN ANIMATIONS
                if (gunAnimationProgress > 0.001f)
                {
                    gunAnimationProgress -= 0.1f;
                    if (gunAnimationProgress < 0.001f) gunAnimationProgress = 0.0f;
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


                    if (!WallManager.Instance.collides(r) && !(TankManager.Instance.tankCollisionWithTank(r, this)))
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
            else // state == despawning
            {

            }
        }

        public bool canFire()
        {
            return ((DateTime.Now - lastfire) > timebtwFire) && !(gunAnimationProgress > 0.00f);
        }

        public Bullet Fire()
        {
            gunAnimationProgress = 1.0f;
            lastfire = DateTime.Now;
            return new Bullet(this, position, bearing, bulletv);

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
            if (spawnState == SpawnState.SPAWNED)
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
}
