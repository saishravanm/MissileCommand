using Microsoft.Xna.Framework;

namespace MissileCommand
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