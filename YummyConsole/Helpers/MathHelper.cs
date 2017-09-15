using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole.Helpers
{
    public static class MathHelper
    {

        public const float PI = 3.14159265358979f;
        public const float Deg2Rad = PI / 180f;
        public const float Rad2Deg = 180f / PI;

        public static float SmoothDamp(float value, float a, float b)
        {
            if (a > b)
            {
                float old_min = a;
                a = b;
                b = old_min;
                value = b + a - value;
            }
            float delta = b - a;
            return SmoothDamp01((value - a) / delta) * delta + a;
        }

        public static float SmoothDamp01(float value)
        {
            return (float) (-0.5 * (Math.Cos(Math.PI * value) - 1));
        }

		/// <summary>
		/// Linearly interpolates between two float values.
		/// The parameter <paramref name="t"/> is clamped to the range [0, 1].
		/// <para>
		/// <paramref name="t"/>=0 returns <paramref name="a"/>.
		/// <paramref name="t"/>=1 returns <paramref name="b"/>.
		/// <paramref name="t"/>=0.5 returns the point midpoint between <paramref name="a"/> and <paramref name="b"/>.
		/// </para>
		/// </summary>
		public static float Lerp(float a, float b, float t)
        {
            t = Clamp01(t);
            return b * t + a * (1 - t);
        }

	    /// <summary>
	    /// Linearly interpolates between two float values.
	    /// If <paramref name="t"/> is less than zero or greater than one it will result in a return value outside the range <paramref name="a"/> to <paramref name="b"/>.
	    /// <para>
	    /// <paramref name="t"/>=0 returns <paramref name="a"/>.
	    /// <paramref name="t"/>=1 returns <paramref name="b"/>.
	    /// <paramref name="t"/>=0.5 returns the point midpoint between <paramref name="a"/> and <paramref name="b"/>.
	    /// </para>
	    /// </summary>
		public static float LerpUnclamped(float a, float b, float t)
        {
            return b * t + a * (1 - t);
        }

        public static int Clamp(int value, int min, int max)
        {
            return value > max ? max : (value < min ? min : value);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value > max ? max : (value < min ? min : value);
        }

        public static int Clamp01(int value)
        {
            return value > 1 ? 1 : (value < 0 ? 0 : value);
        }

        public static float Clamp01(float value)
        {
            return value > 1 ? 1 : (value < 0 ? 0 : value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from <paramref name="min"/> (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The lower bound. (INCLUSIVE)</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            value = (value - min) % range;
            return value < 0 ? (max + 1 + value) : (min + value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from <paramref name="min"/> (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The lower bound. (INCLUSIVE)</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static float Wrap(float value, float min, float max)
        {
            float range = max - min + 1;
            value = (value - min) % range;
            return value < 0 ? (max + 1 + value) : (min + value);
        }

        /// <summary>
        /// Wraps a value in a specified interval from 0 (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static int Wrap(int value, int max)
        {
            int range = max + 1;
            value = value % range;
            return value < 0 ? (max + 1 + value) : value;
        }

        /// <summary>
        /// Wraps a value in a specified interval from 0 (INCLUSIVE) to <paramref name="max"/> (INCLUSIVE).
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="max">The upper bound. (INCLUSIVE)</param>
        public static float Wrap(float value, float max)
        {
            float range = max + 1;
            value = value % range;
            return value < 0 ? (max + 1 + value) : value;
        }

	    /// <summary>
	    /// Compares two numbers to be equal while allowing a custom threshold.
	    /// </summary>
	    public static bool Approximately(float a, float b, float threshold)
	    {
		    return Math.Abs(a - b) <= Math.Abs(threshold);
	    }

	}
}
