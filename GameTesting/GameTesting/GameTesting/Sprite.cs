using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public int pixelCount
        {
            get
            {
                return Texture.Width * Texture.Height;
            }
        }
        public Vector2 Position { get; set; }
        public bool camRelative { get; set; }
        public ModRect Rectangle
        {
            get
            {
                return new ModRect((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height, 0x0);
            }
        }

        public Sprite()
        {
            Texture = null;
            Position = default(Vector2);
            camRelative = false;
        }

        public Sprite(Texture2D t, Vector2 p, bool cam)
        {
            Texture = t;
            Position = p;
            camRelative = cam;
        }

        public void DestroyAt( int x, int y, int radius )
        {
            Color[] tData = new Color[Texture.Width*Texture.Height];
            Texture.GetData(tData);
                for (int i = x - radius; i < x + radius; i++)
                    for (int j = y - radius; j < y + radius; j++)
                        if (i < (Position.X + Texture.Width) && j < (Position.Y + Texture.Height) && i >= Position.X && j >= Position.Y)
                            if (Math.Sqrt(Math.Pow((i - x), 2.0) + Math.Pow((j - y), 2.0)) <= radius)
                                tData[(int)(i - Position.X) + (int)(j - Position.Y) * Texture.Width] = Color.Transparent;

            bool empty = true;
            for (int i = 0; i < Texture.Width * Texture.Height; i++)
                if (tData[i].A != 0)
                {
                    empty = false;
                    tData[i] = Color.Red;
                }
            if (empty)
                Main.collidableSprites.Remove(this);
            else
                Texture.SetData(tData);
        }
    }
}
