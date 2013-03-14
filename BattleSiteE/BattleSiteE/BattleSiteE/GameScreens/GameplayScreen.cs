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

        SoundEffect cannonShot;

        
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

        }

       
            
            
        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            gamelayout = contentMan.Load<Texture2D>("map1");


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
            
            sb.End();



        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isKeyDown(GameKey.BACK, null))
            {
                ScreenManager.ExitAll();
                ScreenManager.AddScreen(new MenuBackgroundScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }

            foreach (TankBase t in TankManager.Instance.get_tankList())
            {
                if (t.GetType() == typeof(PlayerTank)) ((PlayerTank)t).handleInput(ScreenManager.InputController);
            }

            
        }

        public override void Update(GameTime gameTime, bool coveredByOtherScreen)
        {

            if (!gameStarted && state == State.TransitioningOn && TransitionAlpha > 0.7)
            {
                TankManager.Instance.addTank(new PlayerTank(Color.LightGreen, 64, 64, Bearing.EAST, PlayerIndex.One));
                TankManager.Instance.addTank(new PlayerTank(Color.LightSkyBlue, 64, 128, Bearing.EAST, PlayerIndex.Two));

                //TankManager.Instance.addTank(new AITank(1000, 64));



                gameStarted = true;
            }

            if (gameStarted)
            {
                TankManager.Instance.updateTanks(gameTime);
                BulletManager.Instance.updateBullets(gameTime);
            }

            base.Update(gameTime, coveredByOtherScreen);
        }



    }
}
