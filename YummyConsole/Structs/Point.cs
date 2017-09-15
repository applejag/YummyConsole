using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole
{
    public struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(0, -1).
        /// </summary>
        public static Point Up => new Point(0, -1);
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(0, 1).
        /// </summary>
        public static Point Down => new Point(0, 1);
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(-1, 0).
        /// </summary>
        public static Point Left => new Point(-1, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(1, 0).
        /// </summary>
        public static Point Right => new Point(1, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(0, 0).
        /// </summary>
        public static Point Zero => new Point(0, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Point"/>(1, 1).
        /// </summary>
        public static Point One => new Point(1, 1);

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public static Point operator -(Point point1, Point point2)
        {
            return new Point(point1.x - point2.x, point1.y - point2.y);
        }

	    public static Point operator -(Point point)
	    {
		    return new Point(-point.x, -point.y);
	    }

        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.x + point2.x, point1.y + point2.y);
        }

        public static Point operator *(Point point, int multiplier)
        {
            return new Point(point.x * multiplier, point.y * multiplier);
        }

        public static Point operator *(int multiplier, Point point)
        {
            return new Point(point.x * multiplier, point.y * multiplier);
        }

        public static Point operator *(Point point, float multiplier)
        {
            return new Point((int)(point.x * multiplier), (int)(point.y * multiplier));
        }

        public static Point operator *(float multiplier, Point point)
        {
            return new Point((int)(point.x * multiplier), (int)(point.y * multiplier));
        }

        public static Point operator %(Point point, int modulus)
        {
            return new Point(point.x % modulus, point.y % modulus);
        }

        public static Point operator /(Point point, int divider)
        {
            return new Point(point.x / divider, point.y / divider);
        }

        public static Point operator /(int divider, Point point)
        {
            return new Point(divider / point.x, divider / point.y);
        }

        public static Point operator /(Point point, float divider)
        {
            return new Point((int)(point.x / divider), (int)(point.y / divider));
        }

        public static Point operator /(float divider, Point point)
        {
            return new Point((int)(divider / point.x), (int)(divider / point.y));
        }
    }
}
