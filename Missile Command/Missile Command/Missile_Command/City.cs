using Microsoft.Xna.Framework;

namespace Missile_Command
{
    class City
    {
        public Rectangle rectangle;
        public bool isDestroyed;

        public City(Rectangle rectangle, bool isDestroyed)
        {
            this.rectangle = rectangle;
            this.isDestroyed = isDestroyed;
        }
    }
}