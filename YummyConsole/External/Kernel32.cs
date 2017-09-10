using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace YummyConsole.External
{
	internal static class Kernel32
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] uint fileAccess,
			[MarshalAs(UnmanagedType.U4)] uint fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] int flags,
			IntPtr template);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern bool WriteConsoleOutput(
			SafeFileHandle hConsoleOutput,
			CharInfo[] lpBuffer,
			Coord dwBufferSize,
			Coord dwBufferCoord,
			ref SmallRect lpWriteRegion);

	}
}
