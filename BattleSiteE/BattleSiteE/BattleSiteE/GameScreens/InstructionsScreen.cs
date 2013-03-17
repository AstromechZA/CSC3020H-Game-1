using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BattleSiteE.GameScreens
{
    class InstructionsScreen : GameScreen
    {

        ContentManager contentMan;
        SpriteFont menuFont;
        Texture2D dialogtexture;
        Texture2D plainBlack;
        Rectangle maindialog = new Rectangle(0, 0, 604, 200);
        Rectangle btn_selected = new Rectangle(0, 201, 302, 29);
        Rectangle pixel = new Rectangle(0, 0, 1, 1);

        static String text = "1. First player to reach 25 points wins! \n"+
                            "2. You get 2 points for killing another player. \n"+
                            "3. You get 1 point for killing a bot.\n"+
                            "";

        public InstructionsScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
        }

        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            dialogtexture = contentMan.Load<Texture2D>("dialogparts");
            menuFont = contentMan.Load<SpriteFont>("menufont");
            plainBlack = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
            plainBlack.SetData<Color>(new Color[] {Color.Black});
            
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            
            sb.Begin();

            //background
            sb.Draw(plainBlack, new Rectangle(0, 0, 1280, 720), pixel, Color.White * TransitionAlpha * 0.6f);

            //dialog
            sb.Draw(dialogtexture, new Rectangle(338, 260, 604, 200), maindialog, Color.White * TransitionAlpha);

            //dialog text
            sb.DrawString(menuFont, text, new Vector2(350, 270), Color.Black *  TransitionAlpha);

            sb.Draw(dialogtexture, new Rectangle(337+151, 462, 302, 29), btn_selected, Color.White * TransitionAlpha);
            sb.DrawString(menuFont, "OK", new Vector2(353 + 151, 465), Color.White * TransitionAlpha);

            sb.End();
        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isKeyDown(GameKey.BACK, null) || ScreenManager.InputController.isKeyDown(GameKey.SELECT, null))
            {
                this.ExitScreen();
            }
        }
    }
}
