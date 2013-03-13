using System;
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
        private WallBase[,] wallmap = new WallBase[1, 1] { { null } };


        public WallManager()
        {

                       

        }

        public void makeWalls(Texture2D t)
        {
            wallmap = new WallBase[t.Height, t.Width];

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
                    if (colors2D[y, x] == Color.Black)
                    {
                        wallmap[y, x] = new WallPermanent();
                    }
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

        
        public bool collides(Rectangle r)
        {
            Point tl = gridIndex(r.Left, r.Top);
            Point br = gridIndex(r.Right, r.Bottom);

            for (int y = tl.Y; y <= br.Y; y++)
            {
                for (int x = tl.X; x <= br.X; x++)
                {
                    if (wallmap[y, x] != null) return true;
                }
            }

            return false;
        }


        public WallBase getCollidingWall(Vector2 v)
        {
            Point c = gridIndex(v);
            return wallmap[c.Y, c.X];
        }

        public WallBase getCollidingWall(Rectangle r)
        {
            Point tl = gridIndex(r.Left, r.Top);
            Point br = gridIndex(r.Right, r.Bottom);

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

            Rectangle blank = new Rectangle(32, 32, 32, 32);

            //build tile set data
            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    Rectangle r = blank;
                    if (wallmap[y, x] != null) r = wallmap[y,x].get_sprite_rectangle();

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
                }
            }
        }


        public void damage(Rectangle r)
        {
            
            Point tl = gridIndex(r.Left, r.Top);
            Point br = gridIndex(r.Right, r.Bottom);

            for (int y = tl.Y; y <= br.Y; y++)
            {
                for (int x = tl.X; x <= br.X; x++)
                {
                    WallBase wb = wallmap[y, x];
                    if (wb != null)
                        if (wb.GetType() == typeof(WallDamageable)) wallmap[y, x] = null;
                }
            }

        }
    }
}
