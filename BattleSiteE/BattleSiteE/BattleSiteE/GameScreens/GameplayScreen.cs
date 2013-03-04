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

namespace BattleSiteE.GameScreens
{
    class GameplayScreen : GameScreen
    {

        ContentManager contentMan;
        Texture2D greentiles;

        #region TANK TEXTURE
        
        Texture2D tanktexture;

        #endregion
        

        int[,] wallmap = new int[11,20] 
        {
            {1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,},
            {1,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,1,},
            {1,0,1,1,0,0,1,1,1,1, 1,1,1,1,0,0,1,1,0,1,},
            {1,0,1,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,1,0,1,},
            {1,0,0,0,1,1,1,0,0,1, 1,0,0,1,1,1,0,0,0,1,},
            {1,0,0,0,1,0,0,0,0,0, 0,0,0,0,0,1,0,0,0,1,},
            {1,0,1,0,0,0,1,0,1,1, 1,1,0,1,0,0,0,1,0,1,},
            {1,1,1,0,1,0,1,0,0,0, 0,0,0,1,0,1,0,1,1,1,},
            {1,0,1,0,1,0,1,0,1,0, 0,1,0,1,0,1,0,1,0,1,},
            {1,0,0,0,0,0,0,0,1,0, 0,1,0,0,0,0,0,0,0,1,},
            {1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,},
        };

        List<Tank> controlledTanks = new List<Tank>();
        List<Tank> botTanks = new List<Tank>();
        BulletController bcontroller = new BulletController();
        
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(1);

            // Add a new tank
            //  :   params: Color, center position, bearing direction, player index
            controlledTanks.Add(new Tank(Color.LightGoldenrodYellow, 96, 96, Bearing.WEST, PlayerIndex.One));
            controlledTanks.Add(new Tank(Color.LightGreen, 96, 160, Bearing.EAST, PlayerIndex.Two));

            
        }

       
            
            
        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");


            tanktexture = contentMan.Load<Texture2D>("tanks");

            foreach (Tank t in controlledTanks) t.SetTexture(tanktexture);
            foreach (Tank t in botTanks) t.SetTexture(tanktexture);

            bcontroller.setTexture(contentMan.Load<Texture2D>("bullets"));
            greentiles = contentMan.Load<Texture2D>("greentileset");

            #region WALLBUILDING
            int my = wallmap.GetLength(0);
            int mx = wallmap.GetLength(1);

            //build tile set data
            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    if (wallmap[y, x] == 0)
                    {
                        continue;
                    }
                    int tot = 0;
                    if ((y < my - 1) && wallmap[y + 1, x] > 0) tot += 1;
                    if ((y>0) && wallmap[y-1, x] > 0)       tot+=2;
                    if ((x < mx - 1) && wallmap[y, x + 1] > 0) tot += 4;
                    if ((x>0) && wallmap[y, x-1] > 0)       tot+=8;
                    wallmap[y, x] = tot;
                }
            }
            #endregion


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
            int my = wallmap.GetLength(0);
            int mx = wallmap.GetLength(1);

            //draw tile set
            for (int y = 0; y < my; y++)
            {
                for (int x = 0; x < mx; x++)
                {
                    int ty = (int)Math.Floor(wallmap[y, x] / 4.0);
                    int tx = wallmap[y, x] % 4;

                    Rectangle src = new Rectangle(tx*64, ty*64, 64, 64);
                    Rectangle dst = new Rectangle(x * 64, y * 64, 64, 64);
                    sb.Draw(greentiles, dst, src, Color.White);
                }
            }

            bcontroller.draw(sb);

            // draw tank
            foreach (Tank t in controlledTanks) t.Draw(sb);
            foreach (Tank t in botTanks) t.Draw(sb);

            sb.End();

            
            


        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isMenuBack())
            {
                ScreenManager.ExitAll();
                ScreenManager.AddScreen(new MenuBackgroundScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }

            // check registered controlled tanks

            foreach (Tank t in controlledTanks)
            {
                // for each controlled tank, get the player index, 
                if (ScreenManager.InputController.isControllerUp(t.ControllingIndex))
                {
                    t.SetNextTargetBearing(Bearing.NORTH);
                }
                if (ScreenManager.InputController.isControllerRight(t.ControllingIndex))
                {
                    t.SetNextTargetBearing(Bearing.EAST);
                }
                if (ScreenManager.InputController.isControllerDown(t.ControllingIndex))
                {
                    t.SetNextTargetBearing(Bearing.SOUTH);
                }
                if (ScreenManager.InputController.isControllerLeft(t.ControllingIndex))
                {
                    t.SetNextTargetBearing(Bearing.WEST);
                }
                if (ScreenManager.InputController.isFiring(t.ControllingIndex))
                {
                    if (t.canFire())
                    {
                        bcontroller.addBullet(t.Fire());                        
                    } 
                }
            }

            // look up the state of the control set for player index i
            // if direction is pressed, set targetdirection

            
        }

        public override void Update(GameTime gameTime, bool coveredByOtherScreen)
        {
            if (!this.isExiting)
            {
                // update tank control movement using collision map 
                foreach (Tank t in controlledTanks) t.Update(wallmap);
                foreach (Tank t in botTanks) t.Update(wallmap);
                bcontroller.update(wallmap, 64);
            }

            base.Update(gameTime, coveredByOtherScreen);
        }



    }
}
