using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole.Helpers
{
    public static class RandomHelper
    {
        private static readonly Random randomizer = new Random();

        /// <summary>
        /// Random boolean value (true, false)
        /// </summary>
        public static bool Boolean => randomizer.Next(2) == 0;

        /// <summary>
        /// Random value between 0.0 (inclusive) and 1.0 (exclusive)
        /// </summary>
        public static float Float => (float)randomizer.NextDouble();

        /// <summary>
        /// Random value between 0.0 (inclusive) and 1.0 (exclusive)
        /// </summary>
        public static double Double => randomizer.NextDouble();

        /// <summary>
        /// Random <seealso cref="int"/> between 0 (inclusive) and <paramref name="upper"/> (exclusive)
        /// </summary>
        /// <param name="upper">The higher boundary. Exclusive.</param>
        public static int Range(int upper)
        {
            return randomizer.Next(upper);
        }

        /// <summary>
        /// Random <seealso cref="int"/> between <paramref name="lower"/> (inclusive) and <paramref name="upper"/> (exclusive)
        /// </summary>
        /// <param name="lower">The lower boundary. Inclusive.</param>
        /// <param name="upper">The upper boundary. Exclusive</param>
        public static int Range(int lower, int upper)
        {
            return randomizer.Next(lower, upper);
        }

        /// <summary>
        /// Random <seealso cref="float"/> between 0 (inclusive) and <paramref name="upper"/> (exclusive)
        /// </summary>
        /// <param name="upper">The higher boundary. Exclusive.</param>
        public static float Range(float upper)
        {
            return (float)randomizer.NextDouble() * upper;
        }

        /// <summary>
        /// Random <seealso cref="float"/> between <paramref name="lower"/> (inclusive) and <paramref name="upper"/> (exclusive)
        /// </summary>
        /// <param name="lower">The lower boundary. Inclusive.</param>
        /// <param name="upper">The upper boundary. Exclusive</param>
        public static float Range(float lower, float upper)
        {
            return (float)randomizer.NextDouble() * (upper - lower) + lower;
        }

		/// <summary>
		/// Returns one of the parameters at random.
		/// </summary>
	    public static T Choose<T>(params T[] options)
		{
			if (options == null || options.Length == 0)
				throw new ArgumentNullException(nameof(options), "You need to give at least one argument!");

			return options[Range(options.Length)];
		}

    }
}
