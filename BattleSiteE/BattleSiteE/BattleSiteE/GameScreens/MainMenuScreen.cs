using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleSiteE.Manager;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace BattleSiteE.GameScreens
{
    class MainMenuScreen : GameScreen
    {
        ContentManager contentMan;
        Texture2D titleTexture;
        Texture2D menuPartsTextures;
        SpriteFont menuFont;

        String[] mainMenuItems = {
                                    "SINGLEPLAYER",
                                    "MULTIPLAYER",
                                    "HIGH SCORES",
                                    "EXIT"
                                };
        int selectedItem = 0;

        private static Rectangle menuTopRect = new Rectangle(0, 0, 300, 11);
        private static Rectangle menuButtonSelRect = new Rectangle(0, 11, 300, 25);
        private static Rectangle menuButtonNormRect = new Rectangle(0, 36, 300, 25);
        private static Rectangle menuBottomRect = new Rectangle(0, 89, 300, 11);

        public MainMenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (contentMan == null)
                contentMan = new ContentManager(ScreenManager.Game.Services, "Content");

            titleTexture = contentMan.Load<Texture2D>("gamehead");
            menuPartsTextures = contentMan.Load<Texture2D>("menuparts");
            menuFont = contentMan.Load<SpriteFont>("menufont");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch sb = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            
            sb.Begin();

            sb.Draw(titleTexture,
                new Microsoft.Xna.Framework.Rectangle(20, 20, 510, 197),
                Color.White * TransitionAlpha);

            Point menutopleft = new Point(125, 240);

            sb.Draw(menuPartsTextures, new Rectangle(menutopleft.X, menutopleft.Y - 11, 300, 11), menuTopRect, Color.White * (TransitionAlpha));

            for (int i = 0; i < mainMenuItems.Length; i++)
            {
                 if (i == selectedItem)
                {
                    sb.Draw(menuPartsTextures,
                        new Microsoft.Xna.Framework.Rectangle(menutopleft.X, menutopleft.Y + i * 25, 300, 25),
                        menuButtonSelRect,
                        Color.White * (TransitionAlpha));

                    sb.DrawString(menuFont,
                        mainMenuItems[i],
                        new Vector2(menutopleft.X + 35, menutopleft.Y + i * 25),
                        Color.White * TransitionAlpha * TransitionAlpha);
                }
                else
                {
                    sb.Draw(menuPartsTextures,
                        new Microsoft.Xna.Framework.Rectangle(menutopleft.X, menutopleft.Y + i * 25, 300, 25),
                        menuButtonNormRect,
                        Color.White * (TransitionAlpha));

                    sb.DrawString(menuFont,
                        mainMenuItems[i],
                        new Vector2(menutopleft.X + 35, menutopleft.Y + i * 25),
                        Color.Gray * TransitionAlpha * TransitionAlpha);
                }

                 
                
            }
            sb.Draw(menuPartsTextures,
                    new Microsoft.Xna.Framework.Rectangle(menutopleft.X, menutopleft.Y + mainMenuItems.Length * 25, 300, 11),
                    menuBottomRect,
                    Color.White * (TransitionAlpha));


            
            sb.End();
        }

        public override void HandleInput()
        {
            if (ScreenManager.InputController.isKeyDown(GameKey.DOWN, null))
            {
                selectedItem += 1;
                selectedItem = (int)MathHelper.Clamp(selectedItem, 0, mainMenuItems.Length - 1);
            }
            if (ScreenManager.InputController.isKeyDown(GameKey.UP, null))
            {
                selectedItem -= 1;
                selectedItem = (int)MathHelper.Clamp(selectedItem, 0, mainMenuItems.Length - 1);
            }
            if (ScreenManager.InputController.isKeyDown(GameKey.SELECT, null))
            {
                if (selectedItem == 0)
                {
                    ScreenManager.ExitAll();
                    ScreenManager.AddScreen(new GameplayScreen());                    
                }
                else if (selectedItem == 3) ScreenManager.AddScreen(new ExitConfirmationScreen());
            }

        }


    }
}
