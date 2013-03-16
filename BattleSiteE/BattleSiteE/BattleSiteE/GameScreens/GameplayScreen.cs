using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using BattleSiteE.GameObjects;
using BattleSiteE.GameObjects.Managers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace BattleSiteE.GameScreens
{
    class GameplayScreen : GameScreen
    {

        ContentManager contentMan;

        Texture2D gamelayout;

        private bool gameStarted = false;
        private bool firstupdate = true;

        private static Rectangle countdown1 = new Rectangle(0, 0, 256, 256);
        private static Rectangle countdown2 = new Rectangle(256, 0, 256, 256);
        private static Rectangle countdown3 = new Rectangle(512, 0, 256, 256);
        private static Rectangle countdowntanks = new Rectangle(0, 256, 256*3, 256);
        private Texture2D countdownTex;
        private float countdown;



        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(1);

            //create singletons
            WallManager w = WallManager.Instance;
            w.clear();

            TankManager tm = TankManager.Instance;
            tm.clear();

            BulletManager bm = BulletManager.Instance;
            bm.clear();

            MusicManager.Instance.startTrack("gamemusic", 1000);

            ScoreManager.Instance.clear();
           

            countdown = 4.0f;
        }

        

       
            
            
        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            gamelayout = contentMan.Load<Texture2D>("map1");

            countdownTex = contentMan.Load<Texture2D>("countdown");

            ScoreManager.Instance.setInGameTexture(contentMan.Load<Texture2D>("scores"));
            ScoreManager.Instance.setFont(contentMan.Load<SpriteFont>("scorefont"));
            ScoreManager.Instance.setSounds(contentMan.Load<SoundEffect>("coin"), contentMan.Load<SoundEffect>("skull"));

            TankManager.Instance.setTankTexture(contentMan.Load<Texture2D>("tanks"));
            TankManager.Instance.loadSpawnPoints(gamelayout);
            
            WallManager.Instance.setTextureMap(contentMan.Load<Texture2D>("minitileset"));
            WallManager.Instance.makeWalls(gamelayout);

            BulletManager.Instance.setTexture(contentMan.Load<Texture2D>("bullets"), contentMan.Load<Texture2D>("bulletex"));
            BulletManager.Instance.setFireSound( contentMan.Load<SoundEffect>("tank") );
            BulletManager.Instance.setImpactSound(contentMan.Load<SoundEffect>("impact"));


        }

        public override void UnloadContent()
        {
            contentMan.Unload();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            sb.Begin();
            
            WallManager.Instance.draw(sb, TransitionAlpha);

            TankManager.Instance.drawTanks(sb);

            BulletManager.Instance.drawBullets(sb);

            ScoreManager.Instance.drawInGame(sb);

            if (!gameStarted && countdown > 0)
            {
                float progress = 1 - (countdown % 1);
                int w = (int)(progress * 3000);
                int h = (int)(progress * 3000);

                int c = 640 - w/2;
                int c2 = 640 - (w*2) / 2;
                int cy = 360 - h / 2;

                if (countdown > 3)
                {
                    sb.Draw(countdownTex, new Rectangle(c, cy, w, h), countdown3, Color.White * (0.8f-progress));
                }
                else if (countdown > 2)
                {
                    sb.Draw(countdownTex, new Rectangle(c, cy, w, h), countdown2, Color.White * (0.8f - progress));
                }
                else if (countdown > 1)
                {
                    sb.Draw(countdownTex, new Rectangle(c, cy, w, h), countdown1, Color.White * (0.8f - progress));
                }
                else
                {
                    sb.Draw(countdownTex, new Rectangle(c2, cy, w * 2, h), countdowntanks, Color.White * (0.8f - progress));
                }
            }

            

            sb.End();



        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isKeyDown(GameKey.BACK, null))
            {
                ScreenManager.ExitAll();
                MusicManager.Instance.stop(3000);
                ScreenManager.AddScreen(new MenuBackgroundScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }

            if (gameStarted)
            {
                foreach (TankBase t in TankManager.Instance.get_tankList())
                {
                    if (t.GetType() == typeof(PlayerTank)) ((PlayerTank)t).handleInput(ScreenManager.InputController);
                }
            }

            
        }

        public override void Update(GameTime gameTime, bool coveredByOtherScreen)
        {
            if (firstupdate)
            {
                TankManager.Instance.spawnPlayers();
                firstupdate = false;
            }

            if (!gameStarted)
            {
                if (countdown > 0)
                {
                    countdown -= 0.03f;
                }
                else
                {
                    gameStarted = true;
                    TankManager.Instance.spawnAI(4);
                }

            }


            TankManager.Instance.updateTanks(gameTime);
            BulletManager.Instance.updateBullets(gameTime);
            

            ScoreManager.Instance.updateInGame();

            base.Update(gameTime, coveredByOtherScreen);
        }



    }
}
