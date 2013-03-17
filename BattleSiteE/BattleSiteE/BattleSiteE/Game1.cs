using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BattleSiteE.GameScreens;
using BattleSiteE.Manager;


namespace BattleSiteE
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // 720p
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            
            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";

            // CREATE Screen manager component
            screenManager = new ScreenManager(this);

            Components.Add(screenManager); 

            // Add main menu screens
            screenManager.AddScreen(new MenuBackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            base.Draw(gameTime);
        }
    }
}
