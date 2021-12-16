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

namespace MissileCommand
{
    class Crosshair
    {
        public Rectangle rect;
        public Texture2D text;
        public Crosshair(Rectangle r, Texture2D t)
        {
            rect = r;
            text = t;
        }
        public void update(int width, int height, int x, int y)
        {
            rect.X = x;
            rect.Y = y;
            if(rect.X < 0)
            {
                rect.X = 0;
            }
            if (rect.Y < 0)
            {
                rect.Y = 0;
            }
            if(rect.X + rect.Width > width)
            {
                rect.X = width - rect.Width;
            }
            if (rect.Y + rect.Height > height - 90)
            {
                rect.Y = height - rect.Height - 90;
            }
        }
        public void Draw(SpriteBatch sb, GameTime g)
        {
            sb.Draw(text, rect, Color.White);
        }
    }
}
