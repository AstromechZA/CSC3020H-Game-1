using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BattleSiteE.GameObjects;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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

        private List<Point> spawnPoints = new List<Point>();
        private List<PlayerIndex> playerTanksToBeRespawned = new List<PlayerIndex>();
        private int aiTankQueue = 0;

        private Random random = new Random();

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

            for (int i = 0; i < controlledTanks.Count; i++)
            {
                TankBase tb = controlledTanks[i];
                tb.Update(gametime);

                if (tb.markedForDeletion)
                {
                    if (tb.GetType() == typeof(AITank))
                    {
                        controlledTanks.Remove(controlledTanks[i]);
                        aiTankQueue++;
                    }
                    else
                    {
                        playerTanksToBeRespawned.Add(((PlayerTank)tb).controllingIndex);
                        
                        controlledTanks.Remove(tb);
                    }
                }

            }

            while (aiTankQueue > 0)
            {
                int i = random.Next(0, spawnPoints.Count);
                Point p = spawnPoints[i];
                Rectangle rect = new Rectangle(p.X, p.Y, 64, 64);

                if (getCollidingTank(rect) != null) continue;

                addTank(new AITank(p.X+32,p.Y+32));
                aiTankQueue--;
            }

            while (playerTanksToBeRespawned.Count > 0)
            {
                Debug.WriteLine("player must be spawned!");
                int ri = random.Next(0, spawnPoints.Count);
                Point p = spawnPoints[ri];
                Rectangle rect = new Rectangle(p.X, p.Y, 64, 64);

                if (getCollidingTank(rect) != null) continue;

                PlayerIndex pi = playerTanksToBeRespawned[0];

                Color c;
                if (pi == PlayerIndex.One) c = Color.LightSteelBlue;
                else if (pi == PlayerIndex.Two) c = new Color(1.0f, 0.5f, 0.5f);
                else if (pi == PlayerIndex.Three) c = Color.LightGreen;
                else c = new Color(1.0f, 1.0f, 0.5f);

                PlayerTank pt = new PlayerTank(c, p.X + 32, p.Y + 32, Bearing.EAST, pi);
                addTank(pt);
                playerTanksToBeRespawned.RemoveAt(0);
            }




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
                    if (colors2D[y, x] == Color.Red)
                    {
                        spawnPoints.Add(new Point(x * 32, y * 32));
                    }

                }
            }
        }


        public void spawnPlayers()
        {


            playerTanksToBeRespawned.Add(PlayerIndex.One);
            playerTanksToBeRespawned.Add(PlayerIndex.Two);
            //playerTanksToBeRespawned.Add(PlayerIndex.Three);
            //playerTanksToBeRespawned.Add(PlayerIndex.Four);
        }

        public void spawnAI(int count = 1)
        {
            aiTankQueue += count;
        }
    }
}
