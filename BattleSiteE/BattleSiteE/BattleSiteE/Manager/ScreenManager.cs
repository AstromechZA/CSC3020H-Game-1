using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using BattleSiteE.GameObjects.Managers;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;



namespace BattleSiteE.Manager
{
    public class ScreenManager: DrawableGameComponent
    {
        
        // Variables
        private List<GameScreen> activeScreens = new List<GameScreen>();
        private List<GameScreen> updateScreens = new List<GameScreen>();

        private InputController inputController;

        private SpriteBatch spriteBatch;
        private SpriteFont fpsfont;

        bool isInitialised = false;

        //fps calculation
        int framerate = 0;
        int framecount = 0;
        TimeSpan elapsed = TimeSpan.Zero;

        // Getters
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public InputController InputController
        {
            get { return inputController; }
        }

        // Methods

        public ScreenManager(Game game)
            : base(game)
        {
            inputController = new InputController();
        }

        public override void Initialize()
        {
            base.Initialize();
            isInitialised = true;
        }

        /*
         * Content loading and unloading 
         */
        #region Content Load & Unload
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            fpsfont = content.Load<SpriteFont>("fpsfont");


            MusicManager.Instance.addTrack("menumusic", content.Load<Song>("menu"));

            MusicManager.Instance.addTrack("gamemusic", content.Load<Song>("viking"));

            foreach (GameScreen screen in activeScreens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen screen in activeScreens)
            {
                screen.UnloadContent();
            }
        }
        #endregion

        /*
         * Update and draw
         */
        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            if (activeScreens.Count == 0) Game.Exit();

            //framerate
            elapsed += gameTime.ElapsedGameTime;
            if (elapsed > TimeSpan.FromSeconds(1))
            {
                elapsed -= TimeSpan.FromSeconds(1);
                framerate = framecount;
                framecount = 0;
            }
            MusicManager.Instance.Update();

            // Keep a copy of the screens being updated this run, so as to not get confused.
            updateScreens.Clear();
            foreach (GameScreen screen in activeScreens) updateScreens.Add(screen);
            
            // Only allow the top most active screen to handle input
            bool inputActive = Game.IsActive;

            // While we still need to update some screens
            while (updateScreens.Count > 0)
            {
                // Pop screen off stack, we operate top down, so that any additional screens are put on into the foreground.
                GameScreen currentScreen = updateScreens[updateScreens.Count - 1];
                updateScreens.RemoveAt(updateScreens.Count - 1);


                inputController.Update();

                if (inputActive)
                {
                    currentScreen.HandleInput();
                    inputActive = false;
                }


                currentScreen.Update(gameTime, false);

            }


        }

        public override void Draw(GameTime gameTime)
        {
            framecount++;

            foreach (GameScreen screen in activeScreens)
            {
                if (screen.State == State.Hidden) continue;
                screen.Draw(gameTime);
            }

            spriteBatch.Begin();            
            spriteBatch.DrawString(fpsfont, ""+ framerate, new Vector2(4, 4), Color.White);
            spriteBatch.End();

        }



        #endregion

        /*
         * Add and Remove Screens
         */
        #region Add and Remove

        public void AddScreen(GameScreen screen)
        {
            Debug.WriteLine("ScreenManager:AddScreen:" + screen.GetType().ToString());
            screen.ScreenManager = this;

            if (isInitialised) 
                screen.LoadContent();

            activeScreens.Add(screen);
        }

        // Remove instantly
        public void RemoveScreen(GameScreen screen)
        {
            Debug.WriteLine("ScreenManager:RemoveScreen:" + screen.GetType().ToString());
            if (isInitialised)
                screen.UnloadContent();

            activeScreens.Remove(screen);
            updateScreens.Remove(screen);
        }

        public void ExitAll()
        {
            foreach (GameScreen s in activeScreens )
            {
                s.ExitScreen();
            }
        }

        #endregion

        //kill all
        public void FinishGame()
        {
            List<GameScreen> cs = new List<GameScreen>();
            foreach (GameScreen screen in activeScreens) cs.Insert(0, screen);
            foreach (GameScreen screen in cs) screen.ExitScreen();
            
        }


    }
}
