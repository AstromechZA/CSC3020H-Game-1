using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameObjects
{
    class CollisionManager
    {
        private List<GameObject>[,] collisionRegions;
        private int gridsize;

        public CollisionManager(int width, int height, int gridSize)
        {
            collisionRegions = new List<GameObject>[height, width];
            gridsize = gridSize;
        }

        // ADD GAME OBJECT TO COLLISION REGIONS
        public void addObject(GameObject go)
        {
            foreach ( List<GameObject> region in getOverlappingRegions(go))
            {
                region.Add(go);
            }
        }

        // REMOVE GAME OBJECT FROM COLLISION REGIONS
        public void removeObject(GameObject go)
        {
            foreach (List<GameObject> region in getOverlappingRegions(go))
            {
                region.Remove(go);
            }
        }

        public bool collides(Rectangle r)
        {
            int fromx = (int)(Math.Floor(r.Left / gridsize * 1.0d));
            int tox = (int)(Math.Ceiling(r.Right / gridsize * 1.0d));
            int fromy = (int)(Math.Floor(r.Top / gridsize * 1.0d));
            int toy = (int)(Math.Ceiling(r.Bottom / gridsize * 1.0d));

            if (fromx < 0) fromx = 0;
            if (tox > collisionRegions.GetLength(1)) tox = collisionRegions.GetLength(1);
            if (fromy < 0) fromy = 0;
            if (toy > collisionRegions.GetLength(0)) toy = collisionRegions.GetLength(0);

            for (int y = fromy; y <= toy; y++)
            {
                for (int x = fromx; x <= tox; x++)
                {
                    foreach (GameObject go in collisionRegions[y,x])
                    {
                        if (go.getCollisionMask().Intersects(r)) return true;
                    }
                }
            }
            return false;

        }

        public List<List<GameObject>> getOverlappingRegions(GameObject go)
        {            
            Rectangle r = go.getCollisionMask();
            return getOverlappingRegions(r);
        }

        public List<List<GameObject>> getOverlappingRegions(Rectangle r)
        {
            List<List<GameObject>> output = new List<List<GameObject>>();
            int fromx = (int)(Math.Floor(r.Left / gridsize * 1.0d));
            int tox = (int)(Math.Ceiling(r.Right / gridsize * 1.0d));
            int fromy = (int)(Math.Floor(r.Top / gridsize * 1.0d));
            int toy = (int)(Math.Ceiling(r.Bottom / gridsize * 1.0d));

            if (fromx < 0) fromx = 0;
            if (tox > collisionRegions.GetLength(1)) tox = collisionRegions.GetLength(1);
            if (fromy < 0) fromy = 0;
            if (toy > collisionRegions.GetLength(0)) toy = collisionRegions.GetLength(0);

            for (int y = fromy; y <= toy; y++)
			{
			    for (int x = fromx; x <= tox; x++)
			    {
			        output.Add(collisionRegions[y,x]);
			    }
			}
            return output;           
        }


    }
}
