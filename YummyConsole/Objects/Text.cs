using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using YummyConsole.Helpers;

namespace YummyConsole
{
    public class Text : Drawable
    {
		#region Fields

		private string _text;
        public Color? foregroundColor = Color.GREY;
        public Color? backgroundColor = null;

		/// <summary>
		/// If greater than zero, the text tries to wrap around to fit the width. Also used in <see cref="alignHorizontal"/>. Default: 0
		/// </summary>
	    public int maxWidth;
		/// <summary>
		/// If greater than zero, the text will only draw this many rows. Also used in <see cref="alignVertical"/>. Default: 0
		/// </summary>
	    public int maxHeight;

		/// <summary>
		/// Horizontal alignment of text. Default: <see cref="Horizontal.Left"/>
		/// </summary>
        public Horizontal alignHorizontal = Horizontal.Left;
		/// <summary>
		/// Vertical alignment of text. Default: <see cref="Vertical.TopBottom"/>
		/// </summary>
        public Vertical alignVertical = Vertical.TopBottom;

	    #endregion

	    #region Constructors
		
		public Text(string text, Drawable parent = null) : base(parent)
        {
            this._text = text;
        }

        public Text(Drawable parent = null) : base(parent)
        {
            this._text = string.Empty;
        }

	    #endregion

		/// <summary>
		/// Bounding box for text. Wrapper property for <see cref="Drawable.Position"/>, <see cref="maxWidth"/> and <see cref="maxHeight"/>.
		/// </summary>
	    public Rect BBox
	    {
		    get => new Rect(ApproxPosition.x, ApproxPosition.y, maxWidth, maxHeight);
		    set
		    {
			    Position = value.TopLeft;
			    maxWidth = value.width;
			    maxHeight = value.height;
		    }
	    }

	    public string text
	    {
		    get => _text ?? string.Empty;
		    set => _text = (value ?? string.Empty).FixNewLines();
	    }

	    protected override void Draw()
        {
			Yummy.ForegroundColor = foregroundColor;
			Yummy.BackgroundColor = backgroundColor;
			
	        string[] allLines = maxWidth <= 0 ? _text.Split('\n') : _text.WordWrap(maxWidth);
	        int numLines = allLines.Length;

			Vector2 pos = Position;
			float left = pos.x;
	        float top = pos.y;
			
			for (int i = 0; i < numLines; i++)
			{
				string line = allLines[i];

				// Calculate x pos
				switch (alignHorizontal)
				{
					case Horizontal.Center:
						if (maxWidth <= 0) pos.x = left - line.Length * 0.5f;
						else pos.x = left + maxWidth - line.Length * 0.5f;
						break;

					case Horizontal.Right:
						if (maxWidth <= 0) pos.x = left - line.Length;
						else pos.x = left + maxWidth - line.Length;
						break;
				}

				// Calculate y pos
				switch (alignVertical)
				{
					case Vertical.Center:
						if (maxHeight <= 0) pos.y = top - numLines * 0.5f;
						else pos.y = top + maxHeight - numLines * 0.5f;
						break;

					case Vertical.TopBottom when numLines > maxHeight && maxHeight > 0:
					case Vertical.BottomTop when numLines <= maxHeight:
					case Vertical.Bottom:
						if (maxHeight <= 0) pos.y = top - numLines + i;
						else pos.y = top + maxHeight - numLines + i;
						break;

					case Vertical.TopBottom when numLines <= maxHeight:
					case Vertical.BottomTop when numLines > maxHeight && maxHeight > 0:
					case Vertical.Top:
					default:
						pos.y = top + i;
						break;
				}
				
				Yummy.SetCursorPosition((Point)pos);
				Yummy.Write(line);
			}
		}

        protected override void Update()
		{}

        public enum Horizontal
        {
			/// <summary>Align to the left.</summary>
            Left,
	        /// <summary>Align to center.</summary>
			Center,
	        /// <summary>Align to the right.</summary>
			Right
		}

	    public enum Vertical
	    {
			/// <summary>Align to top.</summary>
		    Top,
			/// <summary>Align to center.</summary>
			Center,
			/// <summary>Align to bottom.</summary>
			Bottom,
		
			/// <summary>Align to top, but align to bottom when number of lines exceed bbox height. (Common console behaviour)</summary>
			TopBottom,
			/// <summary>Align to bottom, but align to top when number of lines exceed bbox height.</summary>
			BottomTop,
		}
    }
}
