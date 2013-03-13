using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleSiteE.Other
{
    public class SpriteSheet
    {
        private Rectangle[] sprites;
        private Texture2D texture;

        public SpriteSheet(Texture2D t, int size)
        {
            texture = t;
            sprites = new Rectangle[size];
        }

        public Rectangle get_spriteBounds(int index)
        {
            return sprites[index];
        }

        public Texture2D get_spriteTexture()
        {
            return texture;
        }

        public void add_sprite_region(int index, Rectangle r)
        {
            sprites[index] = r;
        }

    }
}
