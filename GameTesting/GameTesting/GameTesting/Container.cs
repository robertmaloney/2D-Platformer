using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameTesting
{
    public class Container
    {
        public int itemCount
        {
            get
            {
                if (items != null)
                    return items.Count();
                else
                    return 0;
            }
        }

        public int Size { get; set; }
        public List<int> items { get; set; }
        public Vector2 Position { get; set; }

        public Container()
        {
            Size = 0;
            items = new List<int>();
            Position = default(Vector2);
        }

        public Container(int size, Vector2 pos)
        {
            Size = size;
            items = new List<int>();
            Position = pos;
        }

        public void Sort()
        {

        }
    }
}
