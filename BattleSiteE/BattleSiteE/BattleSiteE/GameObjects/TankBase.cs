using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleSiteE.GameObjects
{
    public enum Bearing { NORTH, SOUTH, EAST, WEST, NONE }
    public enum SpawnState { SPAWNING, SPAWNED, UNSPAWNING }

    public abstract class TankBase : GameObject
    {
        # region TANK TEXTURE
        protected Texture2D tanktexture;

        protected static Rectangle tankright = new Rectangle(0, 0, 64, 64);
        protected static Rectangle tankdown = new Rectangle(64, 0, 64, 64);
        protected static Rectangle tankup = new Rectangle(128, 0, 64, 64);
        protected static Rectangle tankleft = new Rectangle(192, 0, 64, 64);

        protected static Rectangle gunright = new Rectangle(0, 64, 64, 64);
        protected static Rectangle gundown = new Rectangle(64, 64, 64, 64);
        protected static Rectangle gunup = new Rectangle(128, 64, 64, 64);
        protected static Rectangle gunleft = new Rectangle(192, 64, 64, 64);

        protected static Rectangle treadhorizantal = new Rectangle(0, 128, 64, 64);
        protected static Rectangle treadvertical = new Rectangle(64, 128, 64, 64);

        protected static Rectangle[] gunFireFramesHorizantal = {
                                                         new Rectangle     (0, 192, 64, 64),
                                                         new Rectangle     (64, 192, 64, 64),
                                                         new Rectangle     (128, 192, 64, 64),
                                                         new Rectangle     (192, 192, 64, 64),
                                                         new Rectangle     (256, 192, 64, 64)
                                                     };
        protected static Rectangle[] gunFireFramesVertical = {
                                                         new Rectangle     (0, 256, 64, 64),
                                                         new Rectangle     (64, 256, 64, 64),
                                                         new Rectangle     (128, 256, 64, 64),
                                                         new Rectangle     (192, 256, 64, 64),
                                                         new Rectangle     (256, 256, 64, 64)
                                                     };
        protected static Rectangle[] deathFrames = {
                                                         new Rectangle     (0, 320, 64, 64),
                                                         new Rectangle     (64, 320, 64, 64),
                                                         new Rectangle     (128, 320, 64, 64),
                                                         new Rectangle     (192, 320, 64, 64),
                                                         new Rectangle     (256, 320, 64, 64)
                                                     };



        #endregion


        protected const float stepSize = 3.5f;

        public Vector2 position;
        protected Bearing bearing;
        protected SpawnState spawnState = SpawnState.SPAWNING;
        protected Color tint = Color.White;
        protected float spawnProgress = 0.0f;

        protected float gunAnimationProgress = 0.0f;

        public abstract void Update(GameTime gametime);

        public void set_texture(Texture2D t)
        {
            tanktexture = t;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (tanktexture == null) return;

            Rectangle t_tread = treadhorizantal;
            Rectangle t_tank = tankright;
            Rectangle t_gun = gunright;
            float g_offset_x = 0;
            float g_offset_y = 0;
            float e_offset_x = 0;
            float e_offset_y = 0;
            SpriteEffects e_effect = SpriteEffects.None;

            Rectangle[] explosionFrames = gunFireFramesHorizantal;


            switch (bearing)
            {
                case Bearing.EAST:
                    t_tread = treadhorizantal;
                    t_tank = tankright;
                    t_gun = gunright;
                    g_offset_x = -(gunAnimationProgress * 10);
                    e_offset_x = 28;
                    explosionFrames = gunFireFramesHorizantal;
                    break;
                case Bearing.WEST:
                    t_tread = treadhorizantal;
                    t_tank = tankleft;
                    t_gun = gunleft;
                    g_offset_x = +(gunAnimationProgress * 10);
                    e_offset_x = -28;
                    explosionFrames = gunFireFramesHorizantal;
                    e_effect = SpriteEffects.FlipHorizontally;
                    break;
                case Bearing.NORTH:
                    t_tread = treadvertical;
                    t_tank = tankup;
                    t_gun = gunup;
                    e_offset_y = -28;
                    g_offset_y = +(gunAnimationProgress * 10);
                    explosionFrames = gunFireFramesVertical;
                    e_effect = SpriteEffects.FlipVertically;
                    break;
                case Bearing.SOUTH:
                    t_tread = treadvertical;
                    t_tank = tankdown;
                    t_gun = gundown;
                    e_offset_y = 28;
                    g_offset_y = -(gunAnimationProgress * 10);
                    explosionFrames = gunFireFramesVertical;
                    break;
            }

            float alphamul = 1.0f;
            if (spawnState != SpawnState.SPAWNED)
            {
                alphamul *= (float)(Math.Pow(Math.Sin(spawnProgress * 20), 2)) * spawnProgress + spawnProgress / 2;
            }

            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tread, Color.White * alphamul);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), t_tank, tint * alphamul);
            spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32 + g_offset_x), (int)(position.Y - 32 + g_offset_y), 64, 64), t_gun, tint * alphamul);

            // should we be showing a firing sprite?
            if (gunAnimationProgress > 0.001f)
            {


                int index = (int)Math.Floor((1.0f - gunAnimationProgress) / 0.2f);

                spriteBatch.Draw(
                    tanktexture,
                    new Rectangle((int)(position.X - 32 + g_offset_x + e_offset_x), (int)(position.Y - 32 + g_offset_y + e_offset_y), 64, 64),
                    explosionFrames[index],
                    Color.White,
                    0.0f,
                    new Vector2(0, 0),
                    e_effect,
                    0);


            }

            if (spawnState == SpawnState.UNSPAWNING)
            {
                spriteBatch.Draw(tanktexture, new Rectangle((int)(position.X - 32), (int)(position.Y - 32), 64, 64), deathFrames[(int)(Math.Floor(spawnProgress * 4.5))], Color.White);
            }




        }

        public override Rectangle getCollisionMask()
        {
            if (spawnState == SpawnState.UNSPAWNING) return Rectangle.Empty;
            return new Rectangle((int)(position.X - 31), (int)(position.Y - 31), 62, 62);
        }

        

    }
}
