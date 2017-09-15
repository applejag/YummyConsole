using System;

namespace YummyConsole
{
    public class TextField : Text
    {
	    public delegate void TextFieldEvent(TextField textField, string text);

        public event TextFieldEvent Submitted;
        public event TextFieldEvent Changed;
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
	        string newText = text;

            if ((Input.InputString?.Length ?? 0) > 0)
            {
                newText += Input.InputString;
                changed = true;
            }

            if (Input.GetKeyDown(ConsoleKey.Backspace) && newText.Length > 0)
            {
	            newText = newText.Substring(0, Math.Max(newText.Length - 2, 0));
                changed = true;
            }

            if (changed)
                Changed?.Invoke(this, newText);

            if (Input.GetKeyDown(ConsoleKey.Enter))
            {
                Submitted?.Invoke(this, newText);
                newText = string.Empty;
            }
	        this.text = newText;
        }

        protected override void Draw()
        {
            base.Draw();

            Yummy.CursorVisible = selected;
            Yummy.SetCursorBlinkPosition(ApproxPosition + Point.Right * text.Length);
        }
    }
}