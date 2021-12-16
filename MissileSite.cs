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
    class MissileSite
    {
        public bool activated;
        public Rectangle rect;
        Texture2D text;
        public double distanceFromM;
        public List<PlayerMissile> missiles;
        public int missileIndex = 0;
        public bool drained = false;
        public MissileSite(Rectangle r, Texture2D t)
        {
            rect = r;
            text = t;
            missiles = new List<PlayerMissile>();
            activated = false;
        }
        public void addMissile(PlayerMissile m)
        {
            missiles.Add(m);
        }
        public PlayerMissile getMissile(int i)
        {
            return missiles[i];
        }
        public void fireMissile()
        {
            for(int i = 0; i < missiles.Count; i++)
            {
                if (missiles[i].update()) {
                    missiles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch sb, GameTime g)
        {
            sb.Draw(text, rect, Color.Brown);
        }
    }
}
