using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public struct Rect
    {
        public int x;
        public int y;
        public int width;
        public int height;

        public Vector2 Center
        {
            get => new Vector2(CenterX, CenterY);
            set
            {
                CenterX = value.x;
                CenterY = value.y;
            }
        }

        public float CenterX
        {
            get => x + width * 0.5f;
            set => x = (int)(value - width * 0.5f);
        }

        public float CenterY
        {
            get => y + height * 0.5f;
            set => y = (int)(value - height * 0.5f);
        }

        public Point TopLeft
        {
            get => new Point(x, y);
            set => this = new Rect(value, BottomRight);
        }

        public Point TopRight
        {
            get => new Point(x + width - 1, y);
            set => this = new Rect(value, BottomLeft);
        }

        public Point BottomRight
        {
            get => new Point(x + width - 1, y + height - 1);
            set => this = new Rect(value, TopLeft);
        }

        public Point BottomLeft
        {
            get => new Point(x, y + height - 1);
            set => this = new Rect(value, TopRight);
        }

	    public int Top
	    {
		    get => y;
		    set {
				height += y - value;
			    y = value;
		    }
	    }

	    public int Bottom
	    {
		    get => y + height;
		    set => height = value - y;
	    }

	    public int Left
	    {
		    get => x;
		    set {
			    width += x - value;
			    x = value;
		    }
	    }

	    public int Right
	    {
		    get => x + width;
		    set => width = value - x;
	    }

	    /// <summary>
        /// Creates a rect from position and size
        /// </summary>
        public Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Creates a rect that covers two points
        /// </summary>
        public Rect(Point point1, Point point2)
        {
            this.x = Math.Min(point1.x, point2.x);
            this.y = Math.Min(point1.y, point2.y);
            this.width = Math.Abs(point1.x - point2.x) + 1;
            this.height = Math.Abs(point1.y - point2.y) + 1;
        }

        /// <summary>
        /// Creates a rect that covers all points
        /// </summary>
        public Rect(params Point[] points)
        {
            int length = points?.Length ?? 0;
            if (length == 0) throw new ArgumentException("You must supply at least one point!", nameof(points));

            Point min = points[0];
            Point max = points[0];

            for (int i = 1; i < length; i++)
            {
                if (points[i].x < min.x) min.x = points[i].x;
                if (points[i].y < min.y) min.y = points[i].y;
                if (points[i].x > max.x) max.x = points[i].x;
                if (points[i].y > max.y) max.y = points[i].y;
            }

            this.x = min.x;
            this.y = min.y;
            this.width = max.x - min.x + 1;
            this.height = max.y - min.y + 1;
        }

        /// <summary>
        /// Creates a rect that covers two rects
        /// </summary>
        public Rect(Rect rect1, Rect rect2)
        {
            this = new Rect(rect1.TopLeft, rect1.BottomRight, rect2.TopLeft, rect2.BottomRight);
        }


        /// <summary>
        /// Creates a rect that covers all rects
        /// </summary>
        public Rect(params Rect[] rects)
        {
            int length = rects?.Length ?? 0;
            if (length == 0) throw new ArgumentException("You must supply at least one rect!", nameof(rects));

            Point min = rects[0].TopLeft;
            Point max = rects[0].BottomRight;

            for (int i = 1; i < length; i++)
            {
                if (rects[i].x < min.x) min.x = rects[i].x;
                if (rects[i].y < min.y) min.y = rects[i].y;
                int maxX = rects[i].x + rects[i].width - 1;
                if (maxX > max.x) max.x = maxX;
                int maxY = rects[i].y + rects[i].height - 1;
                if (maxY > max.y) max.y = maxY;
            }

            this.x = min.x;
            this.y = min.y;
            this.width = max.x - min.x + 1;
            this.height = max.y - min.y + 1;
        }

        public bool IsColliding(Rect other)
        {
            return x < other.x + other.width
                && x + width > other.x
                && y < other.y + other.height
                && y + height > other.y;
        }

        public void Deconstruct(out int x, out int y, out int width, out int height)
        {
            x = this.x;
            y = this.y;
            width = this.width;
            height = this.height;
        }

        public void Deconstruct(out Point topLeft, out Point bottomRight)
        {
            topLeft = new Point(x, y);
            bottomRight = new Point(x + width - 1, y + height - 1);
        }
    }
}
