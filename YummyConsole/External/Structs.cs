using System.Runtime.InteropServices;

namespace YummyConsole.External
{

	#region Points and rects

	[StructLayout(LayoutKind.Sequential)]
	internal struct Coord
	{
		public short X;
		public short Y;

		public Coord(short X, short Y)
		{
			this.X = X;
			this.Y = Y;
		}
	};

	[StructLayout(LayoutKind.Sequential)]
	internal struct SmallRect
	{
		public short Left;
		public short Top;
		public short Right;
		public short Bottom;

		public short Width => (short)(Right - Left + 1);
		public short Height => (short)(Bottom - Top + 1);
	}

	#endregion

	#region Characters

	[StructLayout(LayoutKind.Explicit)]
	internal struct CharUnion
	{
		[FieldOffset(0)] public char UnicodeChar;
		//[FieldOffset(0)] public byte AsciiChar;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct CharInfo
	{
		private const short DEFAULT_ATTRIBUTE = ((short)Color.DEFAULT_FOREGROUND & 0x0F) | ((short)Color.DEFAULT_BACKGROUND & 0xF0);

		[FieldOffset(0)] public CharUnion Char;
		[FieldOffset(2)] public short Attributes;

		//public static CharInfo NewAsciiChar(byte ascii, short attributes = DEFAULT_ATTRIBUTE)
		//{
		//    var info = new CharInfo();
		//    info.Char.AsciiChar = ascii;
		//    info.Attributes = attributes;
		//    return info;
		//}

		public static CharInfo NewUnicodeChar(char unicode, short attributes = DEFAULT_ATTRIBUTE)
		{
			var info = new CharInfo();
			info.Char.UnicodeChar = unicode;
			info.Attributes = attributes;
			return info;
		}

		public static CharInfo[] NewBuffer(int width, int height)
		{
			int bufferSize = width * height;
			CharInfo[] buffer = new CharInfo[bufferSize];

			// Fill buffer
			var defaultChar = NewUnicodeChar(' ');

			for (int b = 0; b < bufferSize; b++)
			{
				buffer[b] = defaultChar;
			}

			return buffer;
		}
	}

	#endregion
	
}