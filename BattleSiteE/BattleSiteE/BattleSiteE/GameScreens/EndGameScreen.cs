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
    class EndGameScreen : GameScreen
    {

        ContentManager contentMan;

        Rectangle pixel;
        Texture2D plainBlack;

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


                        

            sb.End();
        }

        public override void HandleInput()
        {
            
        }

    }
}
