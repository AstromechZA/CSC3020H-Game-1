using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BattleSiteE.GameObjects;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects.Managers
{
    public class TankManager
    {
        private static TankManager instance;
        public static TankManager Instance
        {
            get
            {
                if (instance == null) instance = new TankManager();
                return instance;
            }
        }


        Texture2D tanktexture;
        List<TankBase> controlledTanks = new List<TankBase>();

        private List<Point> playerSpawnPoints = new List<Point>();
        private List<Point> AISpawnPoints = new List<Point>();

        public TankManager()
        {

        }

        public void addTank(TankBase newTank)
        {
            newTank.set_texture(tanktexture);
            controlledTanks.Add(newTank);
        }
        
        public void setTankTexture(Texture2D tex)
        {
            tanktexture = tex;
            foreach (TankBase t in controlledTanks) t.set_texture(tanktexture);
        }

        public void drawTanks(SpriteBatch sb)
        {
            foreach (TankBase t in controlledTanks) t.Draw(sb);
        }

        public List<TankBase> get_tankList()
        {
            return controlledTanks;
        }

        public void updateTanks(GameTime gametime)
        {
            foreach (TankBase t in controlledTanks) t.Update(gametime); 
        }

        public TankBase getCollidingTank(Rectangle collisionMask)
        {
            foreach (TankBase other in controlledTanks)
            {
                Rectangle otherMask = other.getCollisionMask();

                if (collisionMask.Intersects(otherMask)) return other;
            }
            return null;
        }

        public bool tankCollisionWithTank(Rectangle collisionMask, TankBase self)
        {
            foreach (TankBase other in controlledTanks)
            {
                if (self == other) continue;

                Rectangle otherMask = other.getCollisionMask();
                if (collisionMask.Intersects(otherMask)) return true;
            }
            return false;
        }

        internal void clear()
        {
            instance = new TankManager();
        }

        public void loadSpawnPoints(Texture2D t)
        {
            Color[] color1D = new Color[t.Width * t.Height];
            t.GetData(color1D);

            Color[,] colors2D = new Color[t.Height, t.Width];

            for (int y = 0; y < t.Height; y++)
                for (int x = 0; x < t.Width; x++)                
                    colors2D[y, x] = color1D[y * t.Width + x];

                

            int my = t.Height;
            int mx = t.Width;

            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    if (colors2D[y, x] == Color.Blue)
                    {
                        playerSpawnPoints.Add(new Point(x*32, y*32));
                    }
                    else if (colors2D[y, x] == Color.Red)
                    {
                        AISpawnPoints.Add(new Point(x*32, y*32));
                    }

                }
            }
        }
    }
}
