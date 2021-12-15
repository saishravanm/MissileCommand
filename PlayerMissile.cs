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
    class PlayerMissile
    {
        public double vx, vy; // missle velocity
        public double x, y; //missle position
        public float angle; //rotation in radians with respect to +x axis
        public Rectangle drect; //source and destination rectangles;
        public Texture2D pic;
        public Boolean exploded;
        int explosionspeed;
        public static Rectangle srect = new Rectangle(0,0,200,200);
        public Vector2 origin;
        static Random rn = new Random();
        public static Texture2D rocketpic, explosionpic;
        public int xT;
        public int yT;
        public PlayerMissile(int xtarget, int ytarget, int screenwidth, Rectangle r, Texture2D t1, Texture2D t2)
        {
            //stuff for exploding
            exploded = false;
            explosionspeed = 1;
            drect = r;
            origin = new Vector2(srect.Width / 2, srect.Height / 2);
            xT = xtarget;
            yT = ytarget;
            Console.WriteLine(xtarget + ", " + ytarget);
            rocketpic = t1; explosionpic = t2;
            //position
            x = r.X;
            y = r.Y;
            //calculate velocity + setup angle
            //random y velocity, then calculate x velocity to reach target
            vy = -3;
            vx = (vy / (ytarget - y)) * (xtarget - x);
            //calculate angle based on velocity
            //if (vx < 0)
            //{
            //    angle = (float)(Math.Atan(vy / vx) + Math.PI);
            //}
            //else if(vx == 0)
            //{
            //    angle = 0;
            //}
            //else
            //{
            //    angle = (float)(Math.Atan(vy / vx));
            //}
            angle = (float)(Math.Atan2(vy, vx));
            //setup stuff for drawing
            drect = r;
            pic = rocketpic;
        }

        //update method returns whether the main method should delete the object
        public Boolean update()
        {
            if (exploded)
            {
                if (drect.Width > 50 && explosionspeed > 0)
                {
                    explosionspeed *= -1;
                }
                drect.Width += explosionspeed;
                drect.Height = drect.Width;
                if (drect.Width <= 0)
                    return true;
            }
            else
            {
                //if (drect.X == xT && drect.Y == yT)
                //{
                //    startexploding();
                //}
                if (Math.Abs(drect.Y - yT) < 5)
                {
                    startexploding();
                }

                //update position
                x += vx;
                y += vy;
                drect.X = (int)x;
                drect.Y = (int)y;
            }
            return false;
        }

        //check if object intersects a rectangle
        public Boolean intersects(Rectangle input)
        {
            Boolean output = false;
            int radius = drect.Width / 2;
            //double distance = Math.Sqrt(Math.Pow(input.X - origin.X, 2) + Math.Pow(input.Y - origin.Y, 2));
            output = input.Contains(drect.X + radius, drect.Y) || input.Contains(drect.X - radius, drect.Y) || input.Contains(drect.X, drect.Y + radius) || input.Contains(drect.X, drect.Y - radius);
            if (output) { startexploding(); }
            return output;
        }

        public Boolean[] intersects(Rectangle[] input)
        {
            Boolean[] output = new Boolean[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = intersects(input[i]);
            }
            return output;
        }

        public Boolean[] intersects(City[] input)
        {
            Boolean[] output = new Boolean[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = intersects(input[i].rectangle);

            }
            return output;
        }

        public void startexploding()
        {
            if (!exploded)
            {
                pic = explosionpic;
                exploded = true;
                drect.Width = 10;
                drect.Height = drect.Width;
            }
        }
        public void Draw(SpriteBatch sb, GameTime g)
        {
            sb.Draw(pic, drect, srect, Color.White, angle, origin, new SpriteEffects(), 0);
        }
    }
}

