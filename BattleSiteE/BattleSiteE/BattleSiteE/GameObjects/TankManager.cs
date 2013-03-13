using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BattleSiteE.GameObjects;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
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
        List<Tank> controlledTanks = new List<Tank>();

        public TankManager()
        {

        }

        public void addTank(Tank newTank)
        {
            newTank.SetTexture(tanktexture);
            controlledTanks.Add(newTank);
        }
        
        public void setTankTexture(Texture2D tex)
        {
            tanktexture = tex;
            foreach (Tank t in controlledTanks) t.SetTexture(tanktexture);
        }

        public void drawTanks(SpriteBatch sb)
        {
            foreach (Tank t in controlledTanks) t.Draw(sb);
        }

        public List<Tank> get_tankList()
        {
            return controlledTanks;
        }

        public void updateTanks()
        {
            foreach (Tank t in controlledTanks) t.Update(); 
        }

        public Tank getCollidingTank(Rectangle collisionMask)
        {
            foreach (Tank other in controlledTanks)
            {
                Rectangle otherMask = other.getCollisionMask();

                if (collisionMask.Intersects(otherMask)) return other;
            }
            return null;
        }

        public bool tankCollisionWithTank(Rectangle collisionMask, Tank self)
        {
            foreach (Tank other in controlledTanks)
            {
                if (self == other) continue;

                Rectangle otherMask = other.getCollisionMask();
                if (collisionMask.Intersects(otherMask)) return true;
            }
            return false;
        }
    }
}
