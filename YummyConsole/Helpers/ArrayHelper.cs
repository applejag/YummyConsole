using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YummyConsole.Helpers
{
    public static class ArrayHelper
    {

        private static readonly System.Random randomGen = new System.Random();

        private static void TheShuffler<T>(IList<T> array, System.Random randomGen)
        {
            int n = array.Count;
            while (n > 1)
            {
                n--;
                int k = randomGen.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        /// <summary>
        /// Shuffles the existing array using the Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> array, int seed)
        {
            TheShuffler(array, new System.Random(seed));
        }

        /// <summary>
        /// Shuffles the existing array using the Fisher-Yates algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> array)
        {
            TheShuffler(array, randomGen);
        }

        /// <summary>
        /// Returns a new string that is shuffled using the Fisher-Yates algorithm.
        /// </summary>
        public static string Shuffle(this string str, int seed)
        {
            char[] array = str.ToCharArray();
            array.Shuffle(seed);
            return new string(array);
        }

        /// <summary>
        /// Returns a new string that is shuffled using the Fisher-Yates algorithm.
        /// </summary>
        public static string Shuffle(this string str)
        {
            char[] array = str.ToCharArray();
            array.Shuffle();
            return new string(array);
        }

		/// <summary>
		/// Returns a new array that's a portion copied from the original array.
		/// Copies from '<paramref name="startIndex"/>' til the end of the <paramref name="array"/>.
		/// </summary>
		/// <param name="array">The array to copy from.</param>
		/// <param name="startIndex">The index of the first element to copy.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static T[] SubArray<T>(this T[] array, int startIndex)
	    {
		    if (array == null)
			    throw new ArgumentNullException(nameof(array), "Array cannot be null!");
			if (startIndex < 0 || startIndex >= array.Length)
				throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, "Index must be larger than or equal to zero and less than the length of the array!");

		    int length = array.Length - startIndex;

		    if (length == 0)
			    return new T[0];

			var result = new T[length];
			Array.Copy(array, startIndex, result, 0, length);
		    return result;
	    }

		/// <summary>
		/// Returns a new array that's a portion copied from the original array.
		/// Copies '<paramref name="length"/>' number of elements from the <paramref name="array"/> starting at '<paramref name="startIndex"/>'.
		/// </summary>
		/// <param name="array">The array to copy from.</param>
		/// <param name="startIndex">The index of the first element to copy.</param>
		/// <param name="length">The number of elements to copy.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static T[] SubArray<T>(this T[] array, int startIndex, int length)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array), "Array cannot be null!");
			if (startIndex < 0 || startIndex >= array.Length)
				throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, "Index must be larger than or equal to zero and less than the length of the array!");
			if (length < 0)
				throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be larger than or equal to zero!");

			if (length == 0)
				return new T[0];

			if (length > array.Length - startIndex)
				length = array.Length - startIndex;

			var result = new T[length];
			Array.Copy(array, startIndex, result, 0, length);
		    return result;
	    }

	    public static string ToPrettyString (this IList array)
	    {
		    if (array == null) return "null";

			var sb = new StringBuilder();
		    int count = array.Count;
			
			sb.Append($"arr({count}) [");

		    for (int i = 0; i < count; i++)
		    {
			    object item = array[i];
			    sb.Append($"{i}: ");

			    if (item is null)
				    sb.Append("null");
				else if (item is IList list)
				    sb.Append(list.ToPrettyString());
			    else if (item is string s)
				    sb.Append($"\"{s.EscapeCharacterLiterals()}\"");
				else if (item is char c)
					sb.Append($"'{c.EscapeCharacterLiterals()}'");
				else
				    sb.Append(item);

			    if (i != count - 1)
				    sb.Append(", ");
		    }

		    sb.Append(']');
		    return sb.ToString();
		}

	}
}
