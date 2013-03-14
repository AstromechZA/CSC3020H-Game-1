using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BattleSiteE.Manager
{
    public enum State
    {
        TransitioningOn,
        Active,
        TransitioningOff,
        Hidden
    }

    public abstract class GameScreen
    {
        //properties
        ScreenManager screenManager;

        //transitions
        protected State state = State.TransitioningOn;
        public bool isExiting = false;
        

        TimeSpan transitionOnTime = TimeSpan.Zero;
        TimeSpan transitionOffTime = TimeSpan.Zero;
        float transitionProgress = 1;                              // 0 = none, 1 = full

        //getters
        public State State
        {
            get { return state; }
        }

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        public float TransitionAlpha
        {
            get { return 1f-transitionProgress; }
        }

        public bool isActive
        {
            get { return (state == State.TransitioningOn || state == State.Active); }
        }


        // methods

        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float delta;

            if (time == TimeSpan.Zero)
                delta = 1;
            else
                delta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            transitionProgress += delta * direction;

            if (
                (direction < 0) && (transitionProgress <= 0) ||
                (direction > 0) && (transitionProgress >= 1)                
                )
            {
                transitionProgress = MathHelper.Clamp(transitionProgress, 0, 1);
                return false;
            }

            return true;

        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool coveredByOtherScreen) 
        {

            // if we are busy exiting
            if (isExiting)
            { 
                // must transition off
                state = State.TransitioningOff;

                //if the transition is complete. remove the screen
                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            // if another screen is coming up that covers this one, then this one must transition off
            else if (coveredByOtherScreen)
            {
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    state = State.TransitioningOff;
                }
                else
                {
                    state = State.Hidden;
                }
            }
            // Otherwise the screen should transition on and become active.
            else
            {
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    state = State.TransitioningOn;
                }
                else
                {
                    state = State.Active;
                }
            }
        
        }

        public virtual void HandleInput() { }

        public virtual void Draw(GameTime gameTime) { }

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }


    }
}
