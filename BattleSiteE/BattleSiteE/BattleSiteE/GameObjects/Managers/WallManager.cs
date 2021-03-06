﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleSiteE.GameObjects.WallTypes;
using System.Diagnostics;

namespace BattleSiteE.GameObjects.Managers
{

    

    public class WallManager
    {
        // SINGLETON
        private static WallManager instance;
        public static WallManager Instance
        {
            get
            {
                if (instance == null) instance = new WallManager();
                return instance;
            }
        }

        private Texture2D wallTextures;

        private static Rectangle blank = new Rectangle(32, 32, 32, 32);

        private static Rectangle damageRect = new Rectangle(0, 32, 32, 32);
        
        private WallBase[,] wallmap = new WallBase[1, 1] { { null } };


        public WallManager()
        {

                       

        }

        // READ TEXTURE and find out where to put them walls
        public void makeWalls(Texture2D t)
        {
            wallmap = new WallBase[t.Height, t.Width];

            // convert 1d array into 2d
            Color[] color1D = new Color[t.Width * t.Height];
            t.GetData(color1D);

            Color[,] colors2D = new Color[t.Height, t.Width];
            
            for (int y = 0; y < t.Height; y++)
                for (int x = 0; x < t.Width; x++)
                {
                    colors2D[y, x] = color1D[y * t.Width + x];

                }
                        
            int my = wallmap.GetLength(0);
            int mx = wallmap.GetLength(1);

            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    // solid walls (unbreakable)
                    if (colors2D[y, x] == Color.Black)
                    {
                        wallmap[y, x] = new WallPermanent();
                    }
                    // breakable walls
                    else if (colors2D[y, x] == Color.Gray)
                    {
                        wallmap[y, x] = new WallDamageable();
                    }
                    else
                    {
                        wallmap[y, x] = null;
                    }

                }
            }




        }

        public bool collides(Vector2 v)
        {
            Point c = gridIndex(v);
            return (wallmap[c.Y, c.X] != null);
        }

        public bool collides(Point p)
        {
            Point c = gridIndex(p);
            return (wallmap[c.Y, c.X] != null);
        }

        // DOES the given rectangle collide with any blocks
        public bool collides(Rectangle r)
        {
            Point tl = gridIndex(r.Left, r.Top);

            if (tl.X > wallmap.GetLength(1)) return false;
            if (tl.Y > wallmap.GetLength(0)) return false;

            Point br = gridIndex(r.Right, r.Bottom);

            if (br.X < 0) return false;
            if (br.Y < 0) return false;

            int sx = (int)MathHelper.Clamp(tl.X, 0, wallmap.GetLength(1)-1);
            int sy = (int)MathHelper.Clamp(tl.Y, 0, wallmap.GetLength(0)-1);

            int ex = (int)MathHelper.Clamp(br.X, 0, wallmap.GetLength(1)-1);
            int ey = (int)MathHelper.Clamp(br.Y, 0, wallmap.GetLength(0)-1);

            // loop
            for (int y = sy; y <= ey; y++)
            {
                for (int x = sx; x <= ex; x++)
                {
                    if (wallmap[y, x] != null) return true;
                }
            }

            return false;
        }


        public WallBase getCollidingWall(Vector2 v)
        {
            Point c = gridIndex(v);

            if (c.X > wallmap.GetLength(1)) return null;
            if (c.Y > wallmap.GetLength(0)) return null;
            if (c.X < 0) return null;
            if (c.Y < 0) return null;

            return wallmap[c.Y, c.X];
        }

        public WallBase getCollidingWall(Rectangle r)
        {
            Point tl = gridIndex(r.Left, r.Top);

            if (tl.X > wallmap.GetLength(1)) return null;
            if (tl.Y > wallmap.GetLength(0)) return null;

            Point br = gridIndex(r.Right, r.Bottom);

            if (br.X < 0) return null;
            if (br.Y < 0) return null;

            int sx = (int)MathHelper.Clamp(tl.X, 0, wallmap.GetLength(1) - 1);
            int sy = (int)MathHelper.Clamp(tl.Y, 0, wallmap.GetLength(0) - 1);

            int ex = (int)MathHelper.Clamp(br.X, 0, wallmap.GetLength(1) - 1);
            int ey = (int)MathHelper.Clamp(br.Y, 0, wallmap.GetLength(0) - 1);

            for (int y = tl.Y; y <= br.Y; y++)
            {
                for (int x = tl.X; x <= br.X; x++)
                {
                    if (wallmap[y, x] != null) return wallmap[y, x];
                }
            }

            return null;
        }

        private Point gridIndex(Point p)
        {
            return new Point((int)(p.X/32), (int)(p.Y/32));
        }

        private Point gridIndex(Vector2 v)
        {
            return new Point((int)(v.X / 32), (int)(v.Y / 32));
        }

        private Point gridIndex(int x, int y)
        {
            return new Point((int)(x / 32), (int)(y / 32));
        }

        public void setTextureMap(Texture2D t)
        {
            wallTextures = t;
        }


        public void draw(SpriteBatch sb, float transalpha)
        {
            int my = wallmap.GetLength(0);
            int mx = wallmap.GetLength(1);


            //build tile set data
            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    // BECAUSE TERNARY
                    Rectangle r = (wallmap[y, x] != null)? wallmap[y,x].get_sprite_rectangle(): blank;

                    // if not fully transitioned, draw awesome grid transform action things
                    if (transalpha < 0.99f)
                    {
                        Rectangle dst = new Rectangle(x * 32 + (int)(((x%5)*32)*(1-transalpha)), (int)(y * 32 * transalpha + (y%3)*y*(1-transalpha)), 32, 32);
                        sb.Draw(wallTextures, dst, r, Color.White * transalpha * transalpha);
                    }
                    else
                    {

                        Rectangle dst = new Rectangle(x * 32, y * 32, 32, 32);
                        sb.Draw(wallTextures, dst, r, Color.White);
                    }

                    // if wall damagable draw cracked thing if damaged
                    if (wallmap[y,x] != null && wallmap[y, x].GetType() == typeof(WallDamageable))
                    {
                        WallDamageable wd = (WallDamageable)wallmap[y, x];
                        if (wd.isDamaged())
                        {
                            Rectangle dst = new Rectangle(x * 32, y * 32, 32, 32);
                            sb.Draw(wallTextures, dst, damageRect, Color.White);                    
                        }
                    }

                }
            }
        }

        // Damage any walls that are in/under the given Rectangle
        public void damage(Rectangle r)
        {
            
            Point tl = gridIndex(r.Left, r.Top);
            Point br = gridIndex(r.Right, r.Bottom);

            for (int y = tl.Y; y <= br.Y; y++)
            {
                for (int x = tl.X; x <= br.X; x++)
                {
                    // wall obj
                    WallBase wb = wallmap[y, x];
                    if (wb != null)
                        if (wb.GetType() == typeof(WallDamageable))
                        {
                            WallDamageable wd = (WallDamageable)wallmap[y, x];
                            if(wd.damage(1)) wallmap[y, x] = null;
                        }
                    
                }
            }

        }

        internal void clear()
        {
        }
    }
}
