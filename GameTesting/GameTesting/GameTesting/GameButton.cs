using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class GameButton
    {
        private bool Hovering;
        public bool Enabled { get; set; }
        public Vector2 Position {get; set;}
        public string Contents { get; set; }
        public SpriteFont Font { get; set; }
        public bool Hover {
            get
            {
                return Hovering;
            }
            set
            {
                if ( value != Hovering )
                Position = new Vector2(Position.X + ((value) ? -(Font.MeasureString(Contents).X / 20) : (Font.MeasureString(Contents).X / 20)), 
                                       Position.Y + ((value) ? -(Font.MeasureString(Contents).Y / 20) : (Font.MeasureString(Contents).Y / 20)));
                Hovering = value;
            }
        }

        public GameButton(Vector2 pos, string text, bool en, SpriteFont sf )
        {
            Position = pos;
            Contents = text;
            Enabled = en;
            Font = sf;
        }

    }
}
