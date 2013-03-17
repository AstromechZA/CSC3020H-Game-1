using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BattleSiteE.GameObjects.Managers;

namespace BattleSiteE.GameScreens
{
    public class EndGameScreen : GameScreen
    {

        ContentManager contentMan;

        Rectangle pixel;
        Texture2D plainBlack;
        Texture2D screenTex;
        SpriteFont stufffont;

        private static Rectangle screen = new Rectangle(0, 0, 926, 418);
        private static Rectangle subblock = new Rectangle(0, 546, 400, 50);
        private static Rectangle[] pnums = new Rectangle[]{
            new Rectangle(0,418,100,128),
            new Rectangle(100,418,100,128),
            new Rectangle(200,418,100,128),
            new Rectangle(300,418,100,128)
        };

        private static Rectangle abutton = (Environment.OSVersion.Platform == PlatformID.Win32NT)?new Rectangle(400, 418, 64, 64):new Rectangle(464, 418, 64, 64);
        public EndGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);

        }

        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            pixel = new Rectangle(0, 0, 1, 1);
            plainBlack = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            plainBlack.SetData<Color>(new Color[] { new Color(248, 248, 181) });

            screenTex = contentMan.Load<Texture2D>("endgame");

            stufffont = contentMan.Load<SpriteFont>("menufont");
            
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            //calculate transision effects
            Color whitetextcolor = Color.White * TransitionAlpha;
            Color graytextcolor = Color.Gray * TransitionAlpha;
            Color blacktexcolor = Color.White * (TransitionAlpha);
            
            sb.Begin();

            //background
            sb.Draw(plainBlack, new Rectangle(0, 0, 1280, 720), pixel, Color.White * TransitionAlpha * 0.6f);

            Rectangle panelR = new Rectangle((1280 - 926) / 2, (720 - 418) / 2, 926, 418);

            sb.Draw(screenTex, panelR, screen, Color.White);

            int tx = panelR.X + 263;
            int ty = panelR.Y + 148;

            int[] ids = ScoreManager.Instance.getOrderedIds();

            foreach (int id in ids)
            {
                Color c = TankManager.tankColours[id];
                sb.Draw(screenTex, new Rectangle(tx, ty, 400, 50), subblock, c);
                sb.DrawString(stufffont, (id+1) + ":", new Vector2(tx + 10, ty + 10), Color.White);
                sb.DrawString(stufffont, ScoreManager.Instance.getPoints(id) + "" , new Vector2(tx + 125, ty + 10), Color.White);
                sb.DrawString(stufffont, ScoreManager.Instance.getShots(id) + "", new Vector2(tx + 218, ty + 10), Color.White);
                sb.DrawString(stufffont, ScoreManager.Instance.getDeaths(id) + "", new Vector2(tx + 325, ty + 10), Color.White);
                

                ty += 52;
            }

            sb.Draw(screenTex, new Rectangle(panelR.X + 456, panelR.Y+5, 100, 128), pnums[ids[0]], Color.White);

            sb.Draw(screenTex, new Rectangle(panelR.X + 685, panelR.Y + 340, 64, 64), abutton, Color.White);

            sb.End();
        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isKeyDown(GameKey.SELECT, null))
            {
                ScreenManager.ExitAll();
                MusicManager.Instance.stop(3000);
                ScreenManager.AddScreen(new MenuBackgroundScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
                return;
            }
        }

    }
}
