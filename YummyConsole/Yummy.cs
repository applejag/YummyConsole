using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using YummyConsole.External;

namespace YummyConsole
{
    public static class Yummy
    {
        #region Constants
        private const string ERR_NOT_INITIALIZED = "The drawing library has not yet been initialized! Please refer to the " + nameof(SetBufferSize) + " function";
        #endregion

        #region Private fields
        private static SafeFileHandle fileHandler;

        private static CharInfo[] buffer;
        private static int bufferSize;
        internal static SmallRect bufferRect;

        private static short bufferWidth;
        private static short bufferHeight;
        
        private static Color? currentFGColor = Color.DEFAULT_FOREGROUND;
        private static Color? currentBGColor = Color.DEFAULT_BACKGROUND;

        private static int cursorBlinkX;
        private static int cursorBlinkY;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the width of the drawing buffer
        /// </summary>
        public static int BufferWidth => bufferWidth;
        /// <summary>
        /// Gets the height of the drawing buffer
        /// </summary>
        public static int BufferHeight => bufferHeight;

        /// <summary>
        /// Gets or sets the cursors current visible state. Default: false
        /// </summary>
        public static bool CursorVisible { get; set; } = false;

        /// <summary>
        /// If true the buffer takes on the size of the console window. If false, it forces the window to be the size of the buffer. Default: true
        /// </summary>
        public static bool AdaptableSize { get; set; } = true;

        /// <summary>
        /// Get or sets the current cursor position on the x-axis
        /// </summary>
        public static int CursorX { get; set; }

        /// <summary>
        /// Gets or sets the current cursor position on the y-axis
        /// </summary>
        public static int CursorY { get; set; }

        /// <summary>
        /// Returns true if the current <see cref="CursorX"/> and <see cref="CursorY"/> position is inside the drawing window.
        /// </summary>
        public static bool CursorOnScreen => CursorX >= 0 && CursorX < BufferWidth && CursorY >= 0 && CursorY < BufferHeight;

        /// <summary>
        /// Gets or sets the current color attribute for foreground coloring.
        /// </summary>
        public static Color? ForegroundColor
        {
            get => currentFGColor;
            set => currentFGColor = value;
        }

        /// <summary>
        /// Gets or sets the current color attribute for background coloring.
        /// </summary>
        public static Color? BackgroundColor
        {
            get => currentBGColor;
            set => currentBGColor = value;
        }
        #endregion

        #region Drawing methods

        /// <summary>
        /// Writes a string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        public static void WriteWrap(string text)
        {
            int length = text.Length;

            for (int i = 0; i < length; i++)
            {
                if (CursorX >= BufferWidth)
                {
                    CursorX = 0;
                    CursorY++;
                }

                if (CursorOnScreen)
                {
                    int index = CursorY * bufferWidth + CursorX;
                    FillBufferPoint(index, text[i]);
                }

                CursorX++;
            }
        }

        /// <summary>
        /// Writes a formatted string of text at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/>, <seealso cref="Format"/></para>
        /// </summary>
        /// <param name="format">The string format to be used.</param>
        /// <param name="args">The list of arguments for the string formatting</param>
        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        /// <summary>
        /// Writes a single character at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        public static void Write(string text)
        {
            int length = text.Length;
            for (int i = 0; i < length; i++)
            {
                if (CursorOnScreen)
                {
                    int index = CursorY * bufferWidth + CursorX;
                    FillBufferPoint(index, text[i]);
                }

                CursorX++;
            }
        }
        
        /// <summary>
        /// Writes a single character at the curren cursor position, using the assigned color attribute.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="letter">The ASCII/Unicode character to be written.</param>
        public static void Write(char letter)
        {
            if (CursorOnScreen)
            {
                int index = CursorY * bufferWidth + CursorX;
                FillBufferPoint(index, letter);
            }

            CursorX++;
        }

        /// <summary>
        /// Writes a formatted string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line, similar to <see cref="WriteWrap"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/>, <seealso cref="Format"/></para>
        /// </summary>
        /// <param name="format">The string format to be used.</param>
        /// <param name="args">The list of arguments for the string formatting</param>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// Writes a string of text at the curren cursor position, using the assigned color attribute.
        /// Once at the end of the buffer width the cursor jumps to a new line, similar to <see cref="WriteWrap"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        /// <param name="text">The string of text to be written.</param>
        public static void WriteLine(string text)
        {
            WriteWrap(text);
            CursorY++;
            CursorX = 0;
        }

		/// <summary>
		/// Clears the entire drawing buffer.
		/// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
		/// </summary>
		public static void Clear()
        {
			SetCursorPosition(0, 0);

            Fill(' ');
        }

        /// <summary>
        /// Clears the current horizontal line and resets the cursor to the start of the line.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="CursorX"/>, <seealso cref="CursorY"/></para>
        /// </summary>
        public static void ClearLine()
        {
            int startAt = CursorY * bufferWidth;
            int stopAt = Math.Min(startAt + bufferWidth, bufferSize);

            for (int b = startAt; b < stopAt; b++)
            {
                FillBufferPoint(b, ' ');
            }

            CursorX = 0;
        }
        
        /// <summary>
        /// Fills in the entire drawing buffer using the assigned color attribute and the given <paramref name="letter"/>.
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void Fill(char letter)
        {
			for (int b = 0; b < bufferSize; b++)
            {
                FillBufferPoint(b, letter);
            }
        }
        
        /// <summary>
        /// Fills in a rectangle using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<see cref="Rect"/> <paramref name="rect"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="rect">The rectangle area to fill in</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillRect(Rect rect, char letter)
        {
			FillRect(rect.x, rect.y, rect.width, rect.height, letter);
        }

        /// <summary>
        /// Fills in a rectangle using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<paramref name="left"/>, <paramref name="top"/>, <paramref name="width"/>, <paramref name="height"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="left">The position of the rectangle from the left edge</param>
        /// <param name="top">The position of the rectangle from the top edge</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillRect(int left, int top, int width, int height, char letter)
        {
			int right = left + width;
            int bottom = top + height;
            for (int x = left; x < right; x++)
            {
                for (int y = top; y < bottom; y++)
                {
                    FillPoint(x, y, letter);
                }
            }
        }

        /// <summary>
        /// Set color and character for a point in the buffer directly.
        /// <para>See: <see cref="buffer"/></para>
        /// </summary>
        /// <param name="index">The index on the buffer array</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is outside the <see cref="buffer"/> array.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void FillBufferPoint(int index, char letter)
        {
            if (!currentBGColor.HasValue && letter == ' ') return;
            buffer[index].Attributes = GetFillColorForPoint(index);
            if (currentFGColor.HasValue)
                buffer[index].Char.UnicodeChar = letter;
        }

        /// <summary>
        /// Returns a combination of <see cref="currentFGColor"/>, <see cref="currentBGColor"/> and the current color at the said index.
        /// </summary>
        /// <param name="index">The index of the buffer array</param>
        internal static short GetFillColorForPoint(int index)
        {
            short value = buffer[index].Attributes;

            if (currentFGColor.HasValue)
            {
                if (currentBGColor.HasValue)
                    value = (short)(((short)currentFGColor.Value & 0x0F) | ((short)currentBGColor.Value & 0xF0));
                else
                    value = (short)(((short)currentFGColor.Value & 0x0F) | (value & 0xF0));
            } else if (currentBGColor.HasValue)
                    value = (short)((value & 0x0F) | ((short)currentBGColor.Value & 0xF0));

            return value;
        }

        /// <summary>
        /// Fills in a character on a single point using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<see cref="Point"/> <paramref name="point"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="point">The 2D position of the point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillPoint(Point point, char letter)
        {
			FillPoint(point.x, point.y, letter);
        }

        /// <summary>
        /// Fills in a character on a single point using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked at (<paramref name="x"/>, <paramref name="y"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="x">The x component of the point</param>
        /// <param name="y">The y component of the point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillPoint(int x, int y, char letter)
        {
			if (x < 0 || x >= bufferWidth || y < 0 || y >= bufferHeight)
                return;

            int index = y * bufferWidth + x;
            if (index >= 0 && index < bufferSize)
            {
                FillBufferPoint(index, letter);
            }
        }

	    /// <summary>
	    /// Draws a line from one point in a certain direction using the assigned color attribute and the given <paramref name="letter"/>, 
	    /// marked between (<see cref="Point"/> <paramref name="point"/>) and (<see cref="Point"/> <paramref name="point"/> + <see cref="Point"/> <paramref name="direction"/>).
	    /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
	    /// </summary>
	    /// <param name="point">The 2D position of the starting point</param>
	    /// <param name="direction">The 2D direction of the ray</param>
	    /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
		public static void FillRay(Point point, Point direction, char letter)
	    {
		    FillLine(point, point + direction, letter);
	    }

        /// <summary>
        /// Draws a line between two points using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked between (<see cref="Point"/> <paramref name="point1"/>) and (<see cref="Point"/> <paramref name="point2"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="point1">The 2D position of the first point</param>
        /// <param name="point2">The 2D position of the second point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillLine(Point point1, Point point2, char letter)
        {
            FillLine(point1.x, point1.y, point2.x, point2.y, letter);
        }

        /// <summary>
        /// Draws a line between two points using the assigned color attribute and the given <paramref name="letter"/>, 
        /// marked between (<paramref name="x1"/>, <paramref name="y1"/>) and (<paramref name="x2"/>, <paramref name="y2"/>).
        /// <para>See also: <seealso cref="BackgroundColor"/>, <seealso cref="ForegroundColor"/></para>
        /// </summary>
        /// <param name="x1">The x component of the first point</param>
        /// <param name="y1">The y component of the first point</param>
        /// <param name="x2">The x component of the second point</param>
        /// <param name="y2">The y component of the second point</param>
        /// <param name="letter">The ASCII/Unicode character to use while filling in</param>
        public static void FillLine(int x1, int y1, int x2, int y2, char letter)
        {
            // Generalized Bresenham's Line Drawing Algorithm
            int x = x1;
            int y = y1;
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = Math.Sign(x2 - x1);
            int sy = Math.Sign(y2 - y1);
            bool swap = false;

            if (dy > dx)
            {
                int tmp = dx;
                dx = dy;
                dy = tmp;
                swap = true;
            }

            int D = 2 * dy - dx;
            for (int i = 0; i < dx; i++)
            {
                FillPoint(x, y, letter);
                while (D >= 0)
                {
                    D -= 2 * dx;
                    if (swap) x += sx;
                    else y += sy;
                }
                D += 2 * dy;
                if (swap) y += sy;
                else x += sx;
            }
        }

        /// <summary>
        /// Renders the drawing buffer onto the console window.
        /// </summary>
        public static void Render()
        {
			try
			{
	            // Fix size
	            if (AdaptableSize)
	            {
					if (Console.WindowWidth != bufferWidth || Console.BufferHeight != bufferHeight)
						SetBufferSize(Console.WindowWidth, Console.WindowHeight);
					else if (bufferRect.Width != bufferWidth || bufferRect.Height != bufferHeight)
			            SetBufferSize(bufferRect.Height, bufferRect.Width);
	            }
	            else
	            {
		            // Force console size
		            Console.SetWindowSize(bufferWidth, bufferHeight);
	            }

                Console.SetBufferSize(
                    Math.Max(Console.WindowWidth, bufferWidth),
                    Math.Max(Console.WindowHeight, bufferHeight));
            } catch { }
			
            bool b = Kernel32.WriteConsoleOutput(fileHandler, buffer,
                new Coord(bufferWidth, bufferHeight),
                new Coord(0, 0),
                ref bufferRect);

			// Cursor
            int x = cursorBlinkX;
            int y = cursorBlinkY;
            bool inWindow = x >= 0 && x < bufferWidth && y >= 0 && y < bufferHeight;
            if (inWindow && CursorVisible)
            {
                Console.SetCursorPosition(x, y);
                if (!Console.CursorVisible)
                    Console.CursorVisible = true;
            }
            else
                Console.CursorVisible = false;

        }

        #endregion

        #region Setters methods
        /// <summary>
        /// Change the window size of the drawing buffer to equal the <see cref="Console.WindowWidth"/> and <see cref="Console.WindowHeight"/>.
        /// </summary>
        public static void SetBufferSize()
        {
            SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }

        /// <summary>
        /// Change the window size of the drawing buffer.
        /// </summary>
        /// <param name="width">The new <paramref name="width"/> of the buffer</param>
        /// <param name="height">The new <paramref name="height"/> of the buffer</param>
        public static void SetBufferSize(int width, int height)
        {
			ValidateFileHandler();

            bufferWidth = (short)width;
            bufferHeight = (short)height;
            bufferSize = bufferWidth * bufferHeight;
            buffer = CharInfo.NewBuffer(width, height);
            bufferRect = new SmallRect { Left = 0, Top = 0, Right = bufferWidth, Bottom = bufferHeight };
        }

	    internal static void ValidateFileHandler()
	    {
			if (fileHandler?.IsInvalid ?? true)
			{
				fileHandler?.Dispose();
				fileHandler = Kernel32.CreateFile(@"CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
				SetBufferSize(Console.WindowWidth, Console.WindowHeight);
			}
		}

        /// <summary>
        /// Sets the cursor position on the buffer window.
        /// </summary>
        /// <param name="point">The 2D position</param>
        public static void SetCursorPosition(Point point)
        {
            SetCursorPosition(point.x, point.y);
        }

        /// <summary>
        /// Sets the cursor position on the buffer window for text writing.
        /// <para>See also: <seealso cref="Write(string)"/>, <seealso cref="WriteLine(string)"/></para>
        /// </summary>
        /// <param name="x">The <paramref name="x"/> position</param>
        /// <param name="y">The <paramref name="y"/> position</param>
        public static void SetCursorPosition(int x, int y)
        {
            CursorX = x;
            CursorY = y;
        }

        /// <summary>
        /// Sets the cursor blink position on the buffer window.
        /// </summary>
        /// <param name="point">The 2D position</param>
        public static void SetCursorBlinkPosition(Point point)
        {
            cursorBlinkX = point.x;
            cursorBlinkY = point.y;
        }

        /// <summary>
        /// Sets the cursor blink position on the buffer window.
        /// </summary>
        /// <param name="x">The <paramref name="x"/> position</param>
        /// <param name="y">The <paramref name="y"/> position</param>
        public static void SetCursorBlinkPosition(int x, int y)
        {
            cursorBlinkX = x;
            cursorBlinkY = y;
        }

        /// <summary>
        /// Resets the color attribute to be used on following drawing to default color, 
        /// <seealso cref="Color.GREY"/> for foreground and <seealso cref="Color.BLACK"/> for background.
        /// </summary>
        public static void ResetColor()
        {
            currentFGColor = Color.DEFAULT_FOREGROUND;
            currentBGColor = Color.DEFAULT_BACKGROUND;
        }
        #endregion

    }
}
