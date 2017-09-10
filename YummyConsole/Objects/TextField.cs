using System;

namespace YummyConsole
{
    public class TextField : Text
    {
        public event Action<TextField> Submitted;
        public event Action<TextField> Changed;
        public bool selected = true;

        public TextField(string text, Drawable parent = null) : base(text, parent)
        {
            ZDepth = -5;
        }

        public TextField(Drawable parent = null) : base(parent)
        {}

        protected override void Update()
        {
            base.Update();

            if (!selected) return;

            bool changed = false;

            if ((Input.InputString?.Length ?? 0) > 0)
            {
                text += Input.InputString;
                changed = true;
            }

            if (Input.GetKeyDown(ConsoleKey.Backspace) && text.Length > 0)
            {
                text = text.Substring(0, Math.Max(text.Length - 2, 0));
                changed = true;
            }

            if (changed)
                Changed?.Invoke(this);

            if (Input.GetKeyDown(ConsoleKey.Enter))
            {
                Submitted?.Invoke(this);
                text = string.Empty;
            }
        }

        protected override void Draw()
        {
            base.Draw();

            Yummy.CursorVisible = selected;
            Yummy.SetCursorBlinkPosition(ApproxPosition + Point.Right * text.Length);
        }
    }
}