using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace BattleSiteE.GameObjects.Managers
{
    public class ScoreManager
    {

        private static ScoreManager instance;
        public static ScoreManager Instance
        {
            get
            {
                if (instance == null) instance = new ScoreManager();
                return instance;
            }
        }

        private int[] orderedids;
        private int[] points;
        private int[] deaths;
        private int[] shotsfired;


        private SoundEffect coinSound;
        private SoundEffect skullSound;

        private static Rectangle main = new Rectangle(0, 0, 128, 720);
        private static Rectangle header = new Rectangle(128, 0, 128, 50);
        private static Rectangle[] playerRects = new Rectangle[]{
            new Rectangle(128, 50, 128, 88),
            new Rectangle(128, 138, 128, 88),
            new Rectangle(128, 226, 128, 88),
            new Rectangle(128, 314, 128, 88)
        };

        private SpriteFont scorefont;
        private Texture2D textureInGame;

        private int winningScore;
        private bool winned = false;
        public bool Winned
        {
            get
            {
                return winned;
            }
        }

        public ScoreManager()
        {
            clear();
        }

        public void setSounds(SoundEffect coin, SoundEffect skull)
        {
            coinSound = coin;
            skullSound = skull;
        }

        public void clear()
        {
            orderedids = new int[] { 0,1,2,3 };
            points = new int[] { 0, 0, 0, 0 };
            deaths = new int[] { 0, 0, 0, 0 };
            shotsfired = new int[] { 0, 0, 0, 0 };
            winned = false;
        }

        public void setInGameTexture(Texture2D t)
        {
            textureInGame = t;
        }

        public void setFont(SpriteFont sf)
        {
            scorefont = sf;
        }

        public void addPoints(int index, int p)
        {
            coinSound.Play();
            points[index] += p;
            sorti(orderedids);
            if (points[index] >= winningScore)
            {
                winned = true;
            }
        }

        public void addDeath(int index)
        {
            skullSound.Play();
            deaths[index] += 1;
        }

        public void addShotFired(int index)
        {
            shotsfired[index] += 1;
        }

        public void drawInGame(SpriteBatch sb)
        {
            sb.Draw(textureInGame, new Microsoft.Xna.Framework.Rectangle(1280 - 128, 0, 128, 720), main, Color.White);

            Rectangle current = new Rectangle(1280 - 128, 35 + 71*(orderedids.Count()-1), 128, 88);
            

            for (int i = orderedids.Count()-1; i >=0 ; i--)
            {
                Rectangle src = playerRects[orderedids[i]];

                sb.Draw(textureInGame, current, src, Color.White);

                String score = points[orderedids[i]] + " / " + deaths[orderedids[i]];
                int w = (int)scorefont.MeasureString(score).X;
                sb.DrawString(scorefont, score, new Vector2(current.X+64-w/2, current.Y+15), Color.White);
                

                current.Y -= 71;
            }


            

            sb.Draw(textureInGame, new Microsoft.Xna.Framework.Rectangle(1280 - 128, 0, 128, 50), header, Color.White);
        }

        // SWAP sort
        public void movei(int[] unsorted, int root)
        {
            // done
            if (root >= 3) return;

            for (int i = root+1; i < orderedids.Length; i++)
            {
                if (points[unsorted[i]] > points[unsorted[root]])
                {
                    int a = unsorted[i];
                    unsorted[i] = unsorted[root];
                    unsorted[root] = a;
                }
                if (points[unsorted[i]] == points[unsorted[root]])
                {
                    if (unsorted[i] < unsorted[root])
                    {
                        int a = unsorted[i];
                        unsorted[i] = unsorted[root];
                        unsorted[root] = a;
                    }
                }
            }
        }

        // swap sort on each number
        public void sorti(int[] unsorted)
        {
            for (int i = 0; i < 3; i++)
            {
                movei(unsorted, i);
            }
        }


        // create orderids for N player
        public void zeroscores(int n)
        {
            if (n == 1) orderedids = new int[] {0 };
            else if (n == 2) orderedids = new int[] { 0, 1 };
            else if (n == 3) orderedids = new int[] { 0,1,2 };
            else orderedids = new int[] { 0,1,2,3 };
        }

        public void setTargetScore(int winningScore)
        {
            this.winningScore = winningScore;
        }

        public int[] getOrderedIds()
        {
            return orderedids;
        }

        public int getPoints(int index)
        {
            return points[index];
        }
        public int getShots(int index)
        {
            return shotsfired[index];
        }
        public int getDeaths(int index)
        {
            return deaths[index];
        }
    }
}
