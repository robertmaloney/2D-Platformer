using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTesting
{
    public class GameMenu
    {
        public bool canDraw = false;
        public Vector2 Position { get; set; }
        private List<GameButton> Buttons { get; set; }
        private List<Sprite> Graphics { get; set; }
        private List<string> Strings { get; set; }
        private List<Vector2> StringPos { get; set; }

        public GameMenu()
        {
            Buttons = new List<GameButton>();
            Graphics = new List<Sprite>();
            Strings = new List<string>();
            StringPos = new List<Vector2>();
        }

        public GameMenu(List<GameButton> gb, List<Sprite> t2d, List<Vector2> v, List<string> s, List<Vector2> sV )
        {
            Graphics = t2d;
            if (s.Count == sV.Count)
            {
                Strings = s;
                StringPos = sV;
            }
            else
            {
                Strings = new List<string>();
                StringPos = new List<Vector2>();
            }
            Buttons = gb;
        }

        public void Add(GameButton g)
        {
            Buttons.Add(g);
        }

        public void Add(Sprite s)
        {
            Graphics.Add(s);
        }

        public void Add(Texture2D t, Vector2 v)
        {
            Sprite s = new Sprite(t, v, false);
            Graphics.Add(s);
        }

        public void Add(string s, Vector2 v)
        {
            Strings.Add(s);
            StringPos.Add(v);
        }

        public void Remove(GameButton g)
        {
            Buttons.Remove(g);
        }

        public void Remove(Sprite s, Vector2 v)
        {
            Graphics.Remove(s);
        }

        public void Remove(string s, Vector2 v)
        {
            Strings.Remove(s);
            StringPos.Remove(v);
        }

        public void RemoveAllStrings()
        {
            Strings = new List<string>();
            StringPos = new List<Vector2>();
        }

        public List<Sprite> getGraphics() { return Graphics; }
        public List<String> getStrings() { return Strings; }
        public List<Vector2> getStringPos() { return StringPos; }
    }
}
