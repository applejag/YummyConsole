using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyConsole.Helpers;

namespace YummyConsole
{
    public struct Vector2
    {
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(0, -1).
        /// </summary>
        public static Vector2 Up => new Vector2(0, -1);
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(0, 1).
        /// </summary>
        public static Vector2 Down => new Vector2(0, 1);
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(-1, 0).
        /// </summary>
        public static Vector2 Left => new Vector2(-1, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(1, 0).
        /// </summary>
        public static Vector2 Right => new Vector2(1, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(0, 0).
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);
        /// <summary>
        /// Shorthand for writing <seealso cref="Vector2"/>(1, 1).
        /// </summary>
        public static Vector2 One => new Vector2(1, 1);

        /// <summary>
        /// X component of the vector.
        /// </summary>
        public float x;
        /// <summary>
        /// Y component of the vector.
        /// </summary>
        public float y;

        /// <summary>
        /// Calculates the length of this vector.
        /// </summary>
        public float Magnitude => (float)Math.Sqrt(x * x + y * y);
        /// <summary>
        /// Calculates the squared length of this vector. (no <seealso cref="Math.Sqrt"/>, compared to <seealso cref="Magnitude"/>)
        /// </summary>
        public float SqrMagnitude => x * x + y * y;
        /// <summary>
        /// Calculates the vector with a magnitude of 1.
        /// </summary>
        public Vector2 Normalized => this / Magnitude;
		/// <summary>
		/// Angle of the vector in degrees. Value ranges from -180° to 180°.
		/// </summary>
		public float Degrees => (float)Math.Atan2(y, x) * MathHelper.Rad2Deg;
		/// <summary>
		/// Angle of the vector in radians. Value ranges from -π to π.
		/// </summary>
		public float Radians => (float)Math.Atan2(y, x);

		/// <summary>
		/// Construct a new vector with given <paramref name="x"/> and <paramref name="y"/> components.
		/// </summary>
		public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        #region Public functions
		/// <summary>
		/// Used for C# 7 tuples.
		/// </summary>
        public void Deconstruct(out float x, out float y)
        {
            x = this.x;
            y = this.y;
        }

        /// <summary>
        /// Makes this vector have a magnitude of 1.
        /// </summary>
        public void Normalize()
        {
            float magn = Magnitude;
            x /= magn;
            y /= magn;
        }

        /// <summary>
        /// Set <paramref name="x"/> and <paramref name="y"/> components of an existing <seealso cref="Vector2"/>.
        /// </summary>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
		#endregion

		#region Static functions

		/// <summary>
		/// Linearly interpolates between two vectors.
		/// If <paramref name="t"/> is less than zero or greater than one it will result in a return vector beyond the bounds of <paramref name="a"/> and <paramref name="b"/> in a linear fashion.
		/// <para>
		/// <paramref name="t"/>=0 returns <paramref name="a"/>.
		/// <paramref name="t"/>=1 returns <paramref name="b"/>.
		/// <paramref name="t"/>=0.5 returns the point midpoint between <paramref name="a"/> and <paramref name="b"/>
		/// </para>
		/// </summary>
		public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
        {
            return b * t + a * (1 - t);
        }

		/// <summary>
		/// Linearly interpolates between two vectors.
		/// The parameter <paramref name="t"/> is clamped to the range [0, 1].
		/// <para>
		/// <paramref name="t"/>=0 returns <paramref name="a"/>.
		/// <paramref name="t"/>=1 returns <paramref name="b"/>.
		/// <paramref name="t"/>=0.5 returns the point midpoint between <paramref name="a"/> and <paramref name="b"/>
		/// </para>
		/// </summary>
		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = MathHelper.Clamp01(t);
            return b * t + a * (1 - t);
        }

		/// <summary>
		/// Returns a new vector with its magnitude clamped to <paramref name="maxLength"/>.
		/// </summary>
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            if (vector.SqrMagnitude > maxLength * maxLength)
            {
                vector.Normalize();
                vector *= maxLength;
            }
            return vector;
        }

		/// <summary>
		/// Calculates the pythagoran distance between two vectors.
		/// </summary>
        public static float Distance(Vector2 vector1, Vector2 vector2)
        {
            float dx = vector1.x - vector2.x;
            float dy = vector2.y - vector2.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Returns a <seealso cref="Vector2"/> that is made from the largest components of two vectors.
        /// </summary>
        public static Vector2 Max(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(Math.Max(vector1.x, vector2.x), Math.Max(vector1.y, vector2.y));
        }
		
	    /// <summary>
	    /// Returns a <seealso cref="Vector2"/> that is made from the largest components of number of vectors.
	    /// </summary>
		/// <exception cref="ArgumentNullException">Thrown if there's no vectors amoung in the params array</exception>
	    public static Vector2 Max(params Vector2[] vectors)
	    {
		    if ((vectors?.Length ?? 0) < 1) throw new ArgumentNullException(nameof(vectors), "At least one parameter is required");

		    int length = vectors.Length;
		    Vector2 max = vectors[0];

		    for (int i = 1; i < length; i++)
		    {
				Vector2 v = vectors[i];
				if (v.x > max.x) max.x = v.x;
			    if (v.y > max.y) max.y = v.y;
		    }

		    return max;
	    }

		/// <summary>
		/// Returns a <seealso cref="float"/> that is the largest of the two components in the vector.
		/// </summary>
		public static float Max(Vector2 vector)
        {
            return Math.Max(vector.x, vector.y);
        }
		
        /// <summary>
        /// Returns a <seealso cref="Vector2"/> that is made from the smallest components of two vectors.
        /// </summary>
        public static Vector2 Min(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(Math.Min(vector1.x, vector2.x), Math.Min(vector1.y, vector2.y));
        }

	    /// <summary>
	    /// Returns a <seealso cref="Vector2"/> that is made from the smallest components of number of vectors.
	    /// </summary>
		/// <exception cref="ArgumentNullException">Thrown if there's no vectors amoung in the params array</exception>
	    public static Vector2 Min(params Vector2[] vectors)
	    {
		    if ((vectors?.Length ?? 0) < 1) throw new ArgumentNullException(nameof(vectors), "At least one parameter is required");

		    int length = vectors.Length;
		    Vector2 min = vectors[0];

		    for (int i = 1; i < length; i++)
		    {
				Vector2 v = vectors[i];
				if (v.x < min.x) min.x = v.x;
			    if (v.y < min.y) min.y = v.y;
		    }

		    return min;
	    }

		/// <summary>
		/// Returns a <seealso cref="float"/> that is the smallest of the two components in the vector.
		/// </summary>
		public static float Min(Vector2 vector)
        {
            return Math.Min(vector.x, vector.y);
		}

	    /// <summary>
	    /// Returns the average position calculated between two vectors.
	    /// </summary>
		public static Vector2 Average(Vector2 vector1, Vector2 vector2)
	    {
		    return (vector1 + vector2) * 0.5f;
	    }

		/// <summary>
		/// Returns the average position calculated between a number of vectors.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if there's no vectors amoung in the params array</exception>
	    public static Vector2 Average(params Vector2[] vectors)
		{
		    if ((vectors?.Length ?? 0) < 1) throw new ArgumentNullException(nameof(vectors), "At least one parameter is required");

			int length = vectors.Length;
			Vector2 sum = vectors[0];

			for (int i = 1; i < length; i++)
			{
				sum += vectors[i];
			}

			return sum / length;
		}

		/// <summary>
		/// Returns a new vector where each component has been multiplied together between the two vectors.
		/// </summary>
		public static Vector2 Scale(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x * vector2.x, vector1.y * vector2.y);
        }

		/// <summary>
		/// Creates a new vector from angle in radians with magnitude 1.
		/// </summary>
        public static Vector2 FromRadians(float rad)
        {
            return new Vector2((float)Math.Cos(rad), -(float)Math.Sin(rad));
        }

	    /// <summary>
	    /// Creates a new vector from angle in radians with custom magnitude.
	    /// </summary>
		public static Vector2 FromRadians(float rad, float magnitude)
        {
            return new Vector2((float)Math.Cos(rad), -(float)Math.Sin(rad)) * magnitude;
        }

	    /// <summary>
	    /// Creates a new vector from angle in degrees with magnitude 1.
	    /// </summary>
		public static Vector2 FromDegrees(float deg)
        {
            return FromRadians(deg * MathHelper.Deg2Rad);
        }

	    /// <summary>
	    /// Creates a new vector from angle in degrees with custom magnitude.
	    /// </summary>
		public static Vector2 FromDegrees(float deg, float magnitude)
        {
            return FromRadians(deg * MathHelper.Deg2Rad, magnitude);
        }
        #endregion

        #region Operators
        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.x - vec2.x, vec1.y - vec2.y);
        }

	    public static Vector2 operator -(Vector2 vector)
	    {
		    return new Vector2(-vector.x, -vector.y);
	    }

		public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.x + vec2.x, vec1.y + vec2.y);
        }

        public static Vector2 operator *(Vector2 Vector2, float multiplier)
        {
            return new Vector2(Vector2.x * multiplier, Vector2.y * multiplier);
        }

        public static Vector2 operator *(float multiplier, Vector2 Vector2)
        {
            return new Vector2(Vector2.x * multiplier, Vector2.y * multiplier);
        }

        public static Vector2 operator %(Vector2 Vector2, float modulus)
        {
            return new Vector2(Vector2.x % modulus, Vector2.y % modulus);
        }

        public static Vector2 operator /(Vector2 Vector2, float divider)
        {
            return new Vector2(Vector2.x / divider, Vector2.y / divider);
        }

        public static Vector2 operator /(float divider, Vector2 Vector2)
        {
            return new Vector2(divider / Vector2.x, divider / Vector2.y);
        }

        public static explicit operator Point(Vector2 vector)
        {
            return new Point((int)vector.x, (int)vector.y);
        }

        public static implicit operator Vector2(Point point)
        {
            return new Vector2(point.x, point.y);
        }
        #endregion
    }
}
