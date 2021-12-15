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

namespace Missile_Command
{
    class EnemyMissle
    {
        public static Texture2D rocketpic, explosionpic, trailpic;
        public double vx, vy; // missle velocity
        public int speed;
        public double x, y; //missle position
        public float angle; //rotation in radians with respect to +x axis
        public Rectangle drect, traildrect; //source and destination rectangles;
        public Texture2D pic;
        public Boolean exploded;
        int explosionspeed;
        public static Rectangle srect = new Rectangle(0, 0, 200, 200);
        public static Vector2 origin = new Vector2(srect.Width / 2, srect.Height / 2);
        public static Vector2 trailorigin;
        static Random rn = new Random();

        public EnemyMissle(int xtarget, int ytarget, int screenwidth)
        {
            //stuff for exploding
            exploded = false;
            explosionspeed = 1;

            //position
            y = 0;
            x = rn.Next(screenwidth);

            //calculate velocity + setup angle
            //random y velocity, then calculate x velocity to reach target
            vy = rn.Next(500, 2000) / 1000.0;
            vx = vy / (ytarget - y) * (xtarget - x);
            //calculate angle based on velocity
            if (vx <= 0)
            {
                angle = (float)(Math.Atan(vy / vx) + Math.PI);
            }
            else
            {
                angle = (float)(Math.Atan(vy / vx));
            }
            speed = 1 + (int)Math.Sqrt(vx * vx + vy * vy);


            //setup stuff for drawing
            drect = new Rectangle((int)x, (int)y, 20, 20);
            traildrect = new Rectangle(drect.X, drect.Y, 100, 8);            
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
                traildrect.Width-=speed; //decrease trail size after explosion based on velocity
                if (drect.Width <= 0)
                    return true;
            }
            else
            {
                if (y > 420)
                {
                    startexploding();
                }

                //update position
                x += vx;
                y += vy;
                drect.X = (int)x;
                drect.Y = (int)y;
                traildrect.X = drect.X;
                traildrect.Y = drect.Y;
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

        public Boolean circleintersects(Rectangle input)
        {
            Boolean output = false;
            int radius = drect.Width / 2;
            double distance1 = Math.Sqrt(Math.Pow(input.X - x, 2) + Math.Pow(input.Y - y, 2));
            double distance2 = Math.Sqrt(Math.Pow(input.X + input.Width - x, 2) + Math.Pow(input.Y + input.Height - y, 2));
            output = distance1 <= radius || distance2 <= radius;
            if (output) { startexploding(); }
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
    }
}