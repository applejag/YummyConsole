using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole.Helpers;

namespace YummyConsole
{
    public class Text : Drawable
    {
        private string[] _lines;
        private string _text;
        public string text {
            get => _text;
            set => _lines = StringHelper.SplitString(_text = value, maxWidth);
        }
        public int maxWidth;
        public Color? foregroundColor = Color.GREY;
        public Color? backgroundColor = null;

        public Alignment alignment = Alignment.Left;

        public Text(string text, Drawable parent = null) : base(parent)
        {
            this.text = text;
        }

        public Text(Drawable parent = null) : base(parent)
        {
            this.text = string.Empty;
        }

        protected override void Draw()
        {
            Yummy.ForegroundColor = foregroundColor;
            Yummy.BackgroundColor = backgroundColor;

            Vector2 pos = Position;
            float x = pos.x;

            foreach (string part in _lines)
            {
                if (alignment == Alignment.Center)
                    pos.x = x - part.Length * 0.5f;
                else if (alignment == Alignment.Right)
                    pos.x = x - part.Length;

                Yummy.SetCursorPosition((Point)pos);
                Yummy.Write(part);

                pos.y += 1;
            }
        }

        protected override void Update()
        {
            //throw new NotImplementedException();
        }

        public enum Alignment
        {
            Left, Center, Right
        }
    }
}
