using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Missile_Command
{
    class Airplane
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public bool isDestroyed;

        public Airplane(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            isDestroyed = false;
        }
    }
}