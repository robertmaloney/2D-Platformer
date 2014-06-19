using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class ModRect
    {
        public byte passable { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ModRect(int x, int y, int w, int h, byte p)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            passable = p;
        }

        public static Rectangle toRect(ModRect r)
        {
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }
    }
}
