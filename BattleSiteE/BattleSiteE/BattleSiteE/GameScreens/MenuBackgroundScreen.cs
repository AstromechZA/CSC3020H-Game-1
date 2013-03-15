using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace BattleSiteE.GameScreens
{
    class MenuBackgroundScreen : GameScreen
    {
        ContentManager contentMan;
        Texture2D backgroundTexture;

        public MenuBackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void  LoadContent()
        {
            if (contentMan == null) 
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            if (pid == PlatformID.Win32NT)
            {
                backgroundTexture = contentMan.Load<Texture2D>("backgroundPC");
            }
            else
            {
                backgroundTexture = contentMan.Load<Texture2D>("backgroundXBOX");
            }

        }

        public override void  UnloadContent()
        {
 	         contentMan.Unload();
        }

        public override void Update(GameTime gameTime, bool coveredByOtherScreen)
        {
            base.Update(gameTime, coveredByOtherScreen);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            sb.Begin();

            sb.Draw(backgroundTexture, 
                new Microsoft.Xna.Framework.Rectangle(0, 0, viewport.Width, viewport.Height),
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            sb.End();
        }

        public override void HandleInput()
        {
           
        }

    }
}
